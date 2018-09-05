using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehavior : MonoBehaviour {
    public float rotationSpeed = 10.0f;

    RaycastHit hit;
	
	// Update is called once per frame
	void Update ()
    {
        //create a plane based on the object position and 
        Plane playerPlane = new Plane(Vector3.forward, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;

        if (playerPlane.Raycast(ray, out hitdist))
        {
            //Debug.Log("HITTING");
            Vector3 targetPoint = ray.GetPoint(hitdist);

            //determines the direction from which the object is looking
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.left);

            //lock the x and y axis rotations
            targetRotation.x = 0;
            targetRotation.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //if (transform.rotation.z > 0 && transform.rotation.z < 1)
                

            //Debug.Log("Rotation: " + transform.rotation.z);
        }

        //
    }
}
