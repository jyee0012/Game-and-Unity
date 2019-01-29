using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour {

    [SerializeField]
    float movementSpeed = 2f, engineForce = 10;
    float leftVAnalog, leftHAnalog, rightVAnalog, aBtn;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        #region Controls
        /*
        axis1.text = Input.GetAxis("Axis1").ToString(); //left analog horizontal
        axis2.text = Input.GetAxis("Axis2").ToString(); //left analog vertical
        axis3.text = Input.GetAxis("Axis3").ToString(); //offset of triggers
        axis4.text = Input.GetAxis("Axis4").ToString(); //right analog horizontal
        axis5.text = Input.GetAxis("Axis5").ToString(); //right analog vertical
        axis6.text = Input.GetAxis("Axis6").ToString(); //d-pad horizontal
        axis7.text = Input.GetAxis("Axis7").ToString(); //d-pad vertical
        axis8.text = Input.GetAxis("Axis8").ToString();
        axis9.text = Input.GetAxis("Axis9").ToString(); //left trigger
        axis10.text = Input.GetAxis("Axis10").ToString(); //right trigger
        button0.text = Input.GetAxis("Button0").ToString(); //a button
        button1.text = Input.GetAxis("Button1").ToString(); //b button
        button2.text = Input.GetAxis("Button2").ToString(); //x button
        button3.text = Input.GetAxis("Button3").ToString(); //y button
        button4.text = Input.GetAxis("Button4").ToString(); //left bumper
        button5.text = Input.GetAxis("Button5").ToString(); //right bumper
        button6.text = Input.GetAxis("Button6").ToString(); //back button
        button7.text = Input.GetAxis("Button7").ToString(); //start button
        */
        #endregion
        leftHAnalog = Input.GetAxis("Axis1");
        leftVAnalog = Input.GetAxis("Axis2");
        rightVAnalog = Input.GetAxis("Axis5");
        aBtn = Input.GetAxis("Button0");
        transform.Rotate(-Vector3.forward * (leftHAnalog));
        transform.Rotate(Vector3.left * (leftVAnalog));
        //transform.Translate(-Vector3.forward * (leftVAnalog) * Time.deltaTime * engineForce);
        //transform.Translate(-Vector3.left * (leftHAnalog) * Time.deltaTime * engineForce);
        transform.Translate(Vector3.forward * (aBtn) * Time.deltaTime * movementSpeed * engineForce);
        
    }
}
