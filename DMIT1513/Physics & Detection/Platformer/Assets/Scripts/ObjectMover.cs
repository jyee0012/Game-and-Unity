using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour {

    public float movementSpeed = 5f, rotationSpeed = 2f;
    float vInput, hInput;
    public Text speedText = null;
    bool bHasText = false;
    public bool bCanJump = false, bCanAdjustSpeed = true;
    // Use this for initialization
    void Start () {
		if (speedText != null)
        {
            bHasText = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        if (bHasText)
        {
            speedText.text = "Speed = " + movementSpeed;
        }
        //Debug.Log("Current Location: " + transform.position);
        //Debug.Log(speed);
	}
    #region Movement
    void Movement()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * Time.deltaTime * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed);
        }
        if (bCanAdjustSpeed)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                movementSpeed++;
            }
            if (Input.GetKey(KeyCode.E))
            {
                movementSpeed--;
            }
        }
        if (bCanJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
                this.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
            }
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
