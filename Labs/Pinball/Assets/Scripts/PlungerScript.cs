using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerScript : BumperScript {

    float powValue = 0, powMax = 100;
	// Use this for initialization
	void Start () {
        bumperForce = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void PlungerShoot()
    {
        bumperForce = 5 * powValue;
        powValue = 0;
    }
}
