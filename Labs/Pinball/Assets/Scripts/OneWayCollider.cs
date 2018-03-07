using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour {

    float timeStamp;
    bool boxCollide;
	// Use this for initialization
	void Start () {
        boxCollide = gameObject.GetComponent<BoxCollider2D>().enabled;

    }
	
	// Update is called once per frame
	void Update () {
        if (!boxCollide)
        {
            if (timeStamp < Time.time)
            {
                timeStamp = Time.time + 1f;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.position.x > transform.position.x)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
