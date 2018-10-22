using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    float vInput;
    public float rotationSpeed = 100;
    Vector3 angles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        vInput = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.right, vInput * Time.deltaTime * -rotationSpeed);
        angles = transform.rotation.eulerAngles;

        if (angles.x > 30 && angles.x < 90)
        {
            transform.rotation = Quaternion.Euler(30, angles.y, angles.z);
        }
	}
}
