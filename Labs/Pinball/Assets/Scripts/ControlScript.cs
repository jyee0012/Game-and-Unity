using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour {
    public GameObject flipperR, flipperL;
    public KeyCode controlR, controlL;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Controls();
    }
    void Controls()
    {
        MotorControl(flipperR, Input.GetKey(controlR));
        MotorControl(flipperL, Input.GetKey(controlL));
    }
    void MotorControl(GameObject Flipper, bool activate)
    {
        Flipper.GetComponent<HingeJoint2D>().useMotor = activate; 
    }
}
