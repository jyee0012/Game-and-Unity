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

public class SurvivorAI : MonoBehaviour
{
    #region Varibles
    // Variables to store what object was detected in front of the AI.
    RaycastHit hitObjectRight;
    RaycastHit hitObjectLeft;
    RaycastHit hitObjectCenter;
    RaycastHit searchHit;

    public int speed;
    float minForwardDist = 1.0f; // The distance that the mover is checking for an object in front
    float minBackwardDist = 3.0f; // The distance that the mover is checking for an object behind
    float minSideDist = 1.0f; // The distance that the mover is checking for an object to the side

    Vector3 fwd, right, left;

    bool centerDetect, rightDetect, leftDetect;

    enum direction { Left, Right, Back };
    direction turnDirection;

    public GameObject blood;

    public int hp = 10;
    public Transform myTarget;
    enum AIState { Wander, Flock, Cover}
    new AIState myState = AIState.Wander;
    bool dead = false;
    Collider[] soldiers;
    #endregion

    // Use this for initialization
    void Start()
    {
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        fwd = transform.TransformDirection(Vector3.forward);
        right = transform.TransformDirection(Vector3.right);
        left = transform.TransformDirection(Vector3.left);

        centerDetect = false;
        rightDetect = false;
        leftDetect = false;

        AI();

        #region Detection
        // Detect if any object in in front.
        if (Physics.SphereCast(transform.position, 0.5f, fwd, out hitObjectCenter, minForwardDist))
        {
            //Gizmos.DrawSphere(transform.position, 0.6f);
            if ((hitObjectCenter.transform.tag != "Survivor" || hitObjectCenter.transform.tag != "Soldier") && !(hitObjectCenter.transform.tag == "Zombie" && myTarget != null))
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
                rotation = Quaternion.LookRotation(theNormal);
                transform.rotation = rotation;
                transform.Rotate(Vector3.up, 90);
                break;
            case direction.Right:
                rotation = Quaternion.LookRotation(theNormal);
                transform.rotation = rotation;
                transform.Rotate(Vector3.up, -90);
                break;
            case direction.Back:
                transform.Rotate(Vector3.up, 180);
                break;
            default:

                break;
        }
    }
    #endregion

    public void Flee(Vector3 target)
    {
        int mask = 1 << 11 | 1 << 9;
        myState = AIState.Wander;

        if (Physics.Linecast(transform.position, target, out searchHit, mask))
        {
            if (searchHit.transform.tag == "Zombie")
            {
                transform.LookAt(target);

                // Check to see if anything is to either side
                CheckSides();

                // If something is to the right then face the direction of the normal for the object and then turn 90 degrees
                if (rightDetect && !leftDetect)
                {
                    transform.Rotate(Vector3.up, 90);
                }
                // If something is to the left then face the direction of the normal for the object and then turn -90 degrees
                else if (!rightDetect && leftDetect)
                {
                    transform.Rotate(Vector3.up, -90);
                }
                // If nothing is detected to either side then randomly rotate 90 or -90 from the normal.
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        transform.Rotate(Vector3.up, 90);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, 180);
                    }
                }
            }
        }
    }

    void AI()
    {
        switch (myState)
        {
            #region State: Wander
            case AIState.Wander:
                #region Target
                if (myTarget != null)
                {
                    myState = AIState.Cover;
                }
                else
                {
                    soldiers = Physics.OverlapSphere(transform.position, 6.0f);

                    foreach (Collider soldier in soldiers)
                    {
                        if (soldier.tag == "Soldier")
                        {
                            if (Physics.Linecast(transform.position, soldier.transform.position, out searchHit))
                            {
                                if (searchHit.transform == soldier.transform)
                                {
                                    myTarget = soldier.transform;
                                }
                            }
                        }
                    }
                }
                #endregion
                break;
            #endregion
            #region State: Flock
            case AIState.Flock:
                
                break;
            #endregion
            #region State: Cover
            case AIState.Cover:
                if (myTarget != null)
                {
                    transform.LookAt(myTarget);

                    if (Vector3.Distance(transform.position, myTarget.position) < 6.0f)
                    {
                        if (Vector3.Distance(transform.position, myTarget.position) < 4.0f)
                        {

                        }
                        //myTarget = null;
                    }
                }
                else
                {
                    myState = AIState.Wander;
                }

                FollowTarget(myTarget);
                break;
            #endregion
            default:
                break;
        }
    }
    public void Flock()
    {
        myState = AIState.Flock;
    }
    public void Flock(Transform target)
    {
        int mask = 1 << 11 | 1 << 9;
        if (myTarget != null)
        {
            transform.LookAt(myTarget);

            if (Vector3.Distance(transform.position, myTarget.position) < 5.0f)
            {
                //Death();
                SurvivorAI script = myTarget.GetComponent<SurvivorAI>();
                if (script != null)
                {
                    script.Flock();
                    Flock();
                }
                myTarget = null;
            }
        }
        if (Physics.Linecast(transform.position, target.position, out searchHit, mask))
        {
            if (searchHit.transform.tag == "Survivor")
            {
                transform.LookAt(target);
                FollowTarget(target);
            }
        }
    }

    #region Damage
    public void Death()
    {
        dead = true;
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

    public void FollowTarget(Transform target)
    {
        if (target != null)
        {
            if (Physics.Linecast(transform.position, target.transform.position, out searchHit))
            {
                if ((searchHit.transform.tag == "Survivor" || searchHit.transform.tag == "Soldier") && !myTarget)
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
    bool IsDead()
    {
        return dead;
    }
}
