using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAI : MonoBehaviour
{
    #region Variables
    // Convert everything into Soldier rather than zombie code.
    // Variables to store what object was detected in front of the AI.
    RaycastHit hitObjectRight;
    RaycastHit hitObjectLeft;
    RaycastHit hitObjectCenter;
    RaycastHit searchHit;

    Collider[] zombies;

    public float speed = 2.0f;
    float defaultSpeed = 2.0f;
    float minForwardDist = 1.0f; // The distance that the mover is checking for an object in front
    float minSideDist = 1.0f; // The distance that the mover is checking for an object to the side

    Vector3 fwd, right, left;

    bool centerDetect, rightDetect, leftDetect;

    enum direction { Left, Right, Back };
    direction turnDirection;
    #endregion
    #region Custom Variables
    public GameObject blood;
    public int hp = 20;
    public enum AIState { wander, attack, shoot };
    public AIState myState;
    public float bulletSpeed = 10.0f;
    public GameObject bullet, BulletSpawn;
    public Transform myTarget;
    bool shot = false;
    float shotTimer;
    int shotCount = 0;

    Transform Noise;
    #endregion
    // Use this for initialization
    void Start()
    {
        // speed = 2.0f;
        myState = AIState.wander;
    }

    // Update is called once per frame
    void Update()
    {
        #region Detect Reset
        fwd = transform.TransformDirection(Vector3.forward);
        right = transform.TransformDirection(Vector3.right);
        left = transform.TransformDirection(Vector3.left);

        centerDetect = false;
        rightDetect = false;
        leftDetect = false;
        #endregion
        AI();
        if (shot)
        {
            if (Time.time > shotTimer)
            {
                shot = false;
            }
        }
        if (Physics.SphereCast(transform.position, 10.0f, fwd, out hitObjectCenter, 6.0f))
        {
            if (hitObjectCenter.transform.tag == "Zombie")
            {
                FollowTarget(hitObjectCenter.transform);
            }
        }
        #region Detect
        // Detect if any object in in front.
        if (Physics.SphereCast(transform.position, 0.4f, fwd, out hitObjectCenter, minForwardDist))
        {
            if (hitObjectCenter.transform.tag != "Survivor" && !(hitObjectCenter.transform.tag == "Soldier" && myTarget != null))
            {
                if (myTarget == null)
                {
                    // Check to see if anything is to either side
                    CheckSides();

                    // If something is to the right then face the direction of the normal for the object and then turn 90 degrees
                    if (rightDetect && !leftDetect)
                    {
                        Turn(hitObjectCenter.normal, direction.Left);
                    }
                    // If something is to the left then face the direction of the normal for the object and then turn -90 degrees
                    else if (!rightDetect && leftDetect)
                    {
                        Turn(hitObjectCenter.normal, direction.Right);
                    }
                    // If nothing is detected to either side then randomly rotate 90 or -90 from the normal.
                    else
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            Turn(hitObjectCenter.normal, direction.Left);
                        }
                        else
                        {
                            Turn(hitObjectCenter.normal, direction.Right);
                        }
                    }
                }
            }

        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        #endregion
    }

    void AI()
    {
        switch (myState)
        {
            #region Wander
            case AIState.wander:
                if (myTarget != null)
                {
                    myState = AIState.attack;
                }
                else
                {
                    //speed = defaultSpeed;
                    zombies = Physics.OverlapSphere(transform.position, 6.0f);

                    foreach (Collider zombie in zombies)
                    {
                        if (zombie.tag == "Zombie")
                        {
                            if (Physics.Linecast(transform.position, zombie.transform.position, out searchHit))
                            {
                                if (searchHit.transform == zombie.transform)
                                {
                                    myTarget = zombie.transform;
                                }
                            }
                        }
                    }
                }
                break;
            #endregion
            #region Attack
            case AIState.attack:
                if (myTarget != null)
                {
                    transform.LookAt(myTarget);

                    if (Vector3.Distance(transform.position, myTarget.position) < 6.0f)
                    {
                        if (Vector3.Distance(transform.position, myTarget.position) < 4.0f)
                        {
                            //if (!Physics.Raycast(transform.position, -transform.forward, 1f))
                            {
                                speed = -1.5f * defaultSpeed;
                            }
                        }
                        else
                        {
                            speed = 0.25f * defaultSpeed;
                        }
                        #region Burst Shot
                        if (!shot)
                        {
                            Fire();
                            shotCount++;
                            shotTimer = Time.time + 0.01f;
                            shot = true;
                            if (shotCount >= 3)
                            {
                                shotCount = 0;
                                shotTimer = Time.time + 1.0f;
                                shot = true;
                            }
                        }
                        #endregion
                        //myTarget = null;
                    }
                }
                else
                {
                    myState = AIState.wander;
                }

                FollowTarget(myTarget);
                break;
            #endregion
            default:
                break;
        }
    }
    void CheckSides()
    {
        // If an object is detected by the right and left or just the center see if there is anything to either side.
        rightDetect = Physics.SphereCast(transform.position, 0.4f, right, out hitObjectRight, minSideDist);
        leftDetect = Physics.SphereCast(transform.position, 0.4f, left, out hitObjectLeft, minSideDist);
    }

    void Turn(Vector3 theNormal, direction newDirection)
    {
        Quaternion rotation;

        switch (newDirection)
        {
            case direction.Left:
                rotation = Quaternion.LookRotation(-theNormal);
                transform.rotation = rotation;
                transform.Rotate(Vector3.up, -90);
                break;
            case direction.Right:
                rotation = Quaternion.LookRotation(-theNormal);
                transform.rotation = rotation;
                transform.Rotate(Vector3.up, 90);
                break;
            case direction.Back:
                transform.Rotate(Vector3.up, 180);
                break;
            default:

                break;
        }
    }

    public void FollowTarget(Transform target)
    {
        if (target != null)
        {
            if (Physics.Linecast(transform.position, target.transform.position, out searchHit))
            {
                if (searchHit.transform.tag == "Zombie" && !myTarget)
                {
                    myTarget = target;
                }
                else
                {
                    myTarget = null;
                }
            }
        }
    }

    #region Damage
    public void Death()
    {
        Instantiate(blood, transform.position + (Vector3.up * 0.5f), transform.rotation);
        DestroyObject(this.gameObject);
    }
    public void Damage()
    {
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            hp--;
        }
    }
    public void Damage(int dmg)
    {
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            hp -= dmg;
        }
    }
    #endregion

    void Fire()
    {
        GameObject bulletClone = Instantiate(bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().AddForce(bulletSpeed * transform.forward, ForceMode.Acceleration);
    }

    void CallOut()
    {

    }
}
