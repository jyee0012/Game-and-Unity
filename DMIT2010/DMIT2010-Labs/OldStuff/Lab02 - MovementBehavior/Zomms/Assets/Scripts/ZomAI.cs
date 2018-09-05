//
// This script will allow an AI object to move forward and avoid objects in the way.
// The Ai will move parallel 
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomAI : MonoBehaviour
{
    public float speed = 1.0f;
    float minForwardDist = 1.0f;
    float minSideDist = 1.0f;

    RaycastHit hitForward, hitRight, hitLeft, searchHit;
    Transform myTarget;
    bool rightDetect, leftDetect;
    enum AI { Wander, Attack }
    new AI Zoms = AI.Wander;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        rightDetect = false;
        leftDetect = false;
        if (Physics.SphereCast(transform.position, 0.4f, transform.TransformDirection(Vector3.forward), out hitForward, minForwardDist))
        {
            Quaternion rotation;
            CheckSides();
            if (!rightDetect && leftDetect)
            {
                rotation = Quaternion.LookRotation(hitForward.normal);
                transform.rotation = rotation;

                transform.Rotate(Vector3.up, -90);
            }
            else if (rightDetect && !leftDetect)
            {
                rotation = Quaternion.LookRotation(hitForward.normal);
                transform.rotation = rotation;

                transform.Rotate(Vector3.up, 90);
            }
            else
            {
                if (Random.Range(0,2) == 0)
                {
                    rotation = Quaternion.LookRotation(hitForward.normal);
                    transform.rotation = rotation;

                    transform.Rotate(Vector3.up, 90);
                }
                else
                {
                    rotation = Quaternion.LookRotation(hitForward.normal);
                    transform.rotation = rotation;

                    transform.Rotate(Vector3.up, -90);
                }
            }

        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        #endregion
    }

    void CheckSides()
    {
        rightDetect = Physics.SphereCast(transform.position, 0.4f, transform.TransformDirection(Vector3.right), out hitRight, minSideDist);
        leftDetect = Physics.SphereCast(transform.position, 0.4f, transform.TransformDirection(Vector3.left), out hitLeft, minSideDist);
    }
    public void FollowTarget(Vector3 target)
    {
        int mask = 1 << 11 | 1 << 8;
        if (Physics.Linecast(transform.position, target, out searchHit, mask))
        {
            if (searchHit.transform.tag == "Survivor")
            {
                myTarget = hitForward.transform;

                transform.LookAt(target);
            }
            else
            {
                myTarget = null;
            }
        }
    }
    void ZombieAI()
    {
        switch (Zoms)
        {
            case AI.Wander:
                break;
            case AI.Attack:
                break;
            default:
                break;
        }
    }
}
