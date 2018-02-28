﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Respawn(-20);
    }
    void Respawn(float limit)
    {
        if(transform.position.y < limit)
        {
            transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
