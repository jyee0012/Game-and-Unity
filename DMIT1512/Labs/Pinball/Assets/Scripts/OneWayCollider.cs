using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    GameObject ball;
    // Use this for initialization
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = !(ball.transform.position.y >= transform.position.y - 1 && ball.transform.position.x > transform.position.x);
    }
}
