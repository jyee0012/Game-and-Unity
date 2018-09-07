using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {

    public float movementSpeed = 2f;
    public bool bConstantRotation = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (bConstantRotation)
        {
            transform.Rotate(Vector3.up * movementSpeed);
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(Vector3.up * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(Vector3.up * movementSpeed);
            }
        }
	}
}
