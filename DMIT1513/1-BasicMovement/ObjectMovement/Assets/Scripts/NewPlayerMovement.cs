using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour {

    float vInput, hInput, rInput;
    public float movementSpeed = 2, rotationSpeed = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
    }
    void Movement()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rInput = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up, rInput * Time.deltaTime * rotationSpeed);
        transform.Translate(new Vector3(hInput * Time.deltaTime * movementSpeed, 0, vInput * Time.deltaTime * movementSpeed));
    }
}
