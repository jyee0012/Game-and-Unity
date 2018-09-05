///////////////////////////////////////////////////////////////////////////////////////
//
// This script will allow an AI object to move forward and avoid objects in the way.
// The AI will move parallel to any object it runs into depending on what is detected to.
// the sides. If there are objects to both sides it will reverse direciton.
//
///////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAIOpt : MonoBehaviour
{
    #region Variables
    // Variables to store what object was detected in front of the AI.
    RaycastHit hitObjectRight;
    RaycastHit hitObjectLeft;
    RaycastHit hitObjectCenter;
    RaycastHit searchHit;

    Collider[] survivors;

    public float speed = 1.0f;
    float minForwardDist = 1.0f; // The distance that the mover is checking for an object in front
    float minSideDist = 1.0f; // The distance that the mover is checking for an object to the side

    Vector3 fwd;
    Vector3 right;
    Vector3 left;

    bool centerDetect;
    bool rightDetect;
    bool leftDetect;

    enum direction { Left, Right, Back };
    direction turnDirection;

    public enum AIState { wander, attack };
    public AIState myState;
    #endregion

    #region Custom Variables
    public Transform myTarget;
    public int hp = 20;
    float timeStamp;
    bool hitOnce = true, causeDeath = false;
    public GameObject blood;
    #endregion

    // Use this for initialization
    void Start()
    {
        //speed = 1.0f;
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
        if (!hitOnce)
        {
            if (Time.time >= timeStamp)
            {
                hitOnce = true;
            }
        }
        else if (causeDeath)
        {
            if (Time.time >= timeStamp)
            {
                Debug.Log("I reset causeDeath variable");
                causeDeath = false;
            }
        }
        if (!causeDeath)
        {
            #region Detect
            // Detect if any object in in front.
            if (Physics.SphereCast(transform.position, 0.2f, fwd, out hitObjectCenter, minForwardDist))
            {
                // if (hit is not Survivor and not Soldier) and not(hit is zombie and my target isn't null)
                if ((hitObjectCenter.transform.tag != "Survivor" && hitObjectCenter.transform.tag != "Soldier") && !(hitObjectCenter.transform.tag == "Zombie" && myTarget != null))
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
                    speed = 1f;
                    survivors = Physics.OverlapSphere(transform.position, 3.0f);

                    foreach (Collider survivor in survivors)
                    {
                        if (survivor.tag == "Survivor")
                        {
                            if (Physics.Linecast(transform.position, survivor.transform.position, out searchHit))
                            {
                                if (searchHit.transform == survivor.transform)
                                {
                                    myTarget = survivor.transform;
                                }
                            }
                        }
                        else if (survivor.tag == "Soldier")
                        {
                            if (Physics.Linecast(transform.position, survivor.transform.position, out searchHit))
                            {
                                if (searchHit.transform == survivor.transform)
                                {
                                    myTarget = survivor.transform;
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
                    //Debug.Log(Vector3.Distance(transform.position, myTarget.position));

                    if (Vector3.Distance(transform.position, myTarget.position) < 2.0f)
                    {
                        #region Damage Enemy
                        if (myTarget.transform.tag == "Survivor")
                        {
                            SurvivorAI script = myTarget.GetComponent<SurvivorAI>();
                            if (script != null)
                            {
                                if (hitOnce)
                                {
                                    script.Damage();
                                    timeStamp = Time.time + 1.0f;
                                    hitOnce = false;
                                }
                            }
                        }
                        else if (myTarget.transform.tag == "Soldier")
                        {
                            SoldierAI script = myTarget.GetComponent<SoldierAI>();
                            if (script != null)
                            {
                                if (hitOnce)
                                {
                                    script.Damage();
                                    timeStamp = Time.time + 2.0f;
                                    hitOnce = false;
                                }
                            }
                        }
                        #endregion
                        MakeNoise();
                        //myTarget = null;
                    }
                }
                else
                {
                    myState = AIState.wander;
                }
                if (myTarget == null)
                {
                    FollowTarget(myTarget);
                }
                break;
            #endregion
            default:
                break;
        }
    }

    #region Detection Methods
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
    #endregion

    #region FollowTarget
    public void FollowTarget(Transform target)
    {
        if (target != null)
        {
            if (Physics.Linecast(transform.position, target.transform.position, out searchHit))
            {
                if ((searchHit.transform.tag == "Survivor" || searchHit.transform.tag == "Soldier") /*&& !myTarget*/)
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
    public void FollowTarget(Transform target, int noiseNum)
    {
        if (target != null)
        {
            if (Physics.SphereCast(transform.position, 3.0f, transform.forward, out searchHit))
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
    #endregion

    void MakeNoise()
    {
        Collider[] noise = Physics.OverlapSphere(transform.position, 5.0f);
        foreach (Collider entity in noise)
        {
            if (entity.tag == "Soldier")
            {
                entity.GetComponent<SoldierAI>().FollowTarget(gameObject.transform);
            }
            else if (entity.tag == "Zombie" && entity.transform.position != transform.position)
            {
                entity.GetComponent<ZombieAIOpt>().FollowTarget(gameObject.transform);
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
            //speed -= 0.02f;
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

    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, 5.0f);
    }

    public void Eat()
    {
        timeStamp = Time.time + 2.0f;
        causeDeath = true;
    }
}
