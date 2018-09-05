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

public class ZombieAI : MonoBehaviour
{
    // Variables to store what object was detected in front of the AI.
    RaycastHit hitObjectRight;
    RaycastHit hitObjectLeft;
    RaycastHit hitObjectCenter;
    RaycastHit searchHit;

    public float speed;
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

    enum AIState { wander, attack };
    AIState myState;

    Transform myTarget;

    // Use this for initialization
    void Start ()
    {
        speed = 1.0f;
        myState = AIState.wander;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fwd = transform.TransformDirection(Vector3.forward);
        right = transform.TransformDirection(Vector3.right);
        left = transform.TransformDirection(Vector3.left);

        centerDetect = false;
        rightDetect = false;
        leftDetect = false;

        if (myTarget != null)
        {
            Transform lastTarget = myTarget;
            FollowTarget(myTarget.position);

            if (lastTarget != null)
            {
                if (Vector3.Distance(transform.position, lastTarget.position) < 2.0f)
                {
                    SurvivorAI script = lastTarget.GetComponentInParent<SurvivorAI>();
                    if (script != null)
                    {
                        script.Damage();
                    }
                    lastTarget = null;
                }
            }
        }
        
        // Detect if any object in in front.
        if (Physics.SphereCast(transform.position, 0.4f, fwd, out hitObjectCenter, minForwardDist))
        {
            if (hitObjectCenter.transform.tag != "Survivor" && !(hitObjectCenter.transform.tag == "Zombie" && myTarget != null))
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
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

    public void FollowTarget(Vector3 target)
    {
        int mask = 1 << 11 | 1 << 8;
        //mask = ~mask;

        //Debug.Log("check line of sight");
        if (Physics.Linecast(transform.position, target, out searchHit, mask))
        {
            //Debug.Log("check tag = " + searchHit.transform.tag);
            if (searchHit.transform.tag == "Survivor")
            {
                //Debug.Log("follow");
                myTarget = hitObjectCenter.transform;

                transform.LookAt(target);
            }
            else
            {
                myTarget = null;
            }
        }
    }
}
