using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour {
    public float bumperForce = 10, timeStamp, hitPoints = 1000;
    public Material hitMAT;
    void Start()
    {
        if (hitMAT == null)
        {
            hitMAT = Resources.Load("GreenMAT") as Material;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D ballRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        ballRigidBody.AddForce(collision.relativeVelocity * bumperForce);
        gameObject.GetComponent<MeshRenderer>().material = hitMAT;
    }
    void Update()
    {
        if (timeStamp < Time.time)
        {
            gameObject.GetComponent<MeshRenderer>().material = Resources.Load("DefaultMAT") as Material;
            timeStamp = Time.time + 1;
        }
    }
}
