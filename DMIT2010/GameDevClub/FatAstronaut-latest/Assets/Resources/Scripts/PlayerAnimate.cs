using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(GetComponent<PlayerMovement>().leftKey) || Input.GetKey(GetComponent<PlayerMovement>().rightKey))
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

    }
}
