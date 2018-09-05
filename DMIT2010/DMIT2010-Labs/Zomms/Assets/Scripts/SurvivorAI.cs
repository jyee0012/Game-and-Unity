using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAI : MonoBehaviour {

    public float speed = 1.0f;
    float minForwardDist = 1.0f;
    float minSideDist = 1.0f;

    RaycastHit hitForward, hitRight, hitLeft, searchHit;
    bool rightDetect, leftDetect;
    enum AI { Wander, Flee, Flock }
    new AI Surv = AI.Wander;
    // Use this for initialization
    void Start ()
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
                if (Random.Range(0, 2) == 0)
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        #endregion
    }

    // Update is called once per frame
    void Update () {
		
	}
    void CheckSides()
    {
        rightDetect = Physics.SphereCast(transform.position, 0.4f, transform.TransformDirection(Vector3.right), out hitRight, minSideDist);
        leftDetect = Physics.SphereCast(transform.position, 0.4f, transform.TransformDirection(Vector3.left), out hitLeft, minSideDist);
    }
}
