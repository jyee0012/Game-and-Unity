///////////////////////////////////////////////////////////////////////////////////////
//
// This script will allow an AI object to move forward and avoid objects in the way.
// The AI will move 90 degrees right or left depending on any objects to the side when
// an obstacle is detected. If there are objects to both sides it will reverse direciton.
//
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAvoid : MonoBehaviour
{
    // Place empty gameObjects to the front, back, left, right and center of the mover
    public GameObject centerDetector;
    public GameObject frontDetector;
    public GameObject backDetector;
    public GameObject rightDetector;
    public GameObject leftDetector;

    public int speed;
    float minForwardDist = 1.0f; // The distance that the mover is checking for an object in front
    float minSideDist = 2.0f; // The distance that the mover is checking for an object to the side

    // Use this for initialization
    void Start ()
    {
        speed = 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        // Detect if any object in in front.
        bool rightDetect = Physics.Raycast(rightDetector.transform.position, fwd, minForwardDist);
        bool leftDetect = Physics.Raycast(leftDetector.transform.position, fwd, minForwardDist);
        bool centerDetect = Physics.Raycast(centerDetector.transform.position, fwd, minForwardDist);

        // If there is an object detected by both the right and left detectors or the center one. This is in 
        // case the object detected is quite thin. There is still some possiblility that the object will not be
        // detected if it is too thin. Another solution might be needed like a spherecast.
        if ((rightDetect && leftDetect) || centerDetect)
        {
            // Check to see if there is anything to either side.
            rightDetect = Physics.Raycast(frontDetector.transform.position, right, minSideDist);
            leftDetect = Physics.Raycast(frontDetector.transform.position, left, minSideDist);

            // If something is to the right then turn left.
            if (rightDetect && !leftDetect)
            {
                transform.Rotate(Vector3.up, -90);
            }
            // If something is to the left then turn right.
            else if (!rightDetect && leftDetect)
            {
                transform.Rotate(Vector3.up, 90);
            }
            // If something is to the right and left then reverse direction.
            else if (rightDetect && leftDetect)
            {
                transform.Rotate(Vector3.up, 180);
            }
            // Otherwise pick a random direction.
            else if (!rightDetect && !leftDetect)
            {
                if (Random.Range(0, 2) == 0)
                {
                    transform.Rotate(Vector3.up, 90);
                }
                else
                {
                    transform.Rotate(Vector3.up, -90);
                }
            }
        }
        // If only the right detector found an obstacle then turn left.
        else if(rightDetect && !leftDetect)
        {
            transform.Rotate(Vector3.up, -90);
        }
        // If only the left detector found an obstacle then turn right.
        else if (!rightDetect && leftDetect)
        {
            transform.Rotate(Vector3.up, 90);
        }
        // If nothing was detected then continue moving forward.
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
	}
}
