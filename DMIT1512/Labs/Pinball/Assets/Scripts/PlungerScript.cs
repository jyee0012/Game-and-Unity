using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungerScript : BumperScript {
    
    Vector3 currentPos;
	// Use this for initialization
	void Start () {
        //bumperForce = 0;
        currentPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = currentPos + new Vector3(0, 0.5f, 0);
            //gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.position = currentPos;
            //gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
        }
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        ballRigidBody.AddForce(new Vector2(0,1) * 10000);
    }
}
