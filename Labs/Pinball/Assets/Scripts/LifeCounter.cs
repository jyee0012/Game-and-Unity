using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour {

    GameObject pinball, lifeBall;
	// Use this for initialization
	void Start () {
        pinball = GameObject.FindGameObjectWithTag("Ball");
        lifeBall = Resources.Load("BallLife") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
