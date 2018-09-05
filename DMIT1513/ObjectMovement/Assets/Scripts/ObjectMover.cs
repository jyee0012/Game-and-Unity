using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour {

    public float movementSpeed = 5f, rotationSpeed = 2f; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        //Debug.Log(speed);
	}
    #region Movement
    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            movementSpeed++;
        }
        if (Input.GetKey(KeyCode.E))
        {
            movementSpeed--;
        }


        #region Reset
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = Vector3.zero + new Vector3(0,10,0);
            //transform.rotation = Quaterion.zero;
            movementSpeed = 2f;
            rotationSpeed = 2f;
        }
        #endregion
    }
    #endregion
}
