using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputValues : MonoBehaviour
{

    [SerializeField]
    Text axis1, axis2, axis3, axis4, axis5, axis6, axis7, axis8, axis9, axis10, button0, button1, button2, button3, button4, button5, button6, button7;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
