using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerScript : PathFinder
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
        Movement();
        if (Physics.SphereCast(transform.position, 0.5f, fwd, out hitObject, minForwardDist))
        {
            if (hitObject.transform.tag == "Runner")
            {
                Follow(hitObject.transform.gameObject);
            }
        }
    }
    void Follow(GameObject followTarget)
    {
        if (Physics.Linecast(transform.position, followTarget.transform.position))
        {
            target = followTarget.gameObject;
            destination = followTarget.transform.position;
        }
        //else
        //{
        //    target = null;
        //}
    }
}
