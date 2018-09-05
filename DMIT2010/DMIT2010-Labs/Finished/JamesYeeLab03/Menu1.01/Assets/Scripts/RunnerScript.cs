using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerScript : PathFinder
{
    float timeStamp;
    public enum RunnerState { Wander, Flee, Pickup }
    RunnerState myState = RunnerState.Wander;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
        Movement();
        Detection();
    }
    void Flee()
    {

    }
}
