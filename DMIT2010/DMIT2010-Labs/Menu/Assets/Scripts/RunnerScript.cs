using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerScript : PathFinder
{
    float timeStamp;
    public enum RunnerState { Wander, Flee, Search }
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
    }
    void Flee()
    {

    }
}
