using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerScript : PathFinder
{
    float timeStamp;
    public enum RunnerState { Start, Wander, Flee, Pickup }
    public RunnerState myState = RunnerState.Start;
    int num = 0;
    bool sprint = false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        States();
        if (amStuck())
        {
            getUnstuck();
        }
        if (myState != RunnerState.Flee)
        {
            if (sprint)
            {
                if (timeStamp < Time.time)
                {
                    sprint = false;
                }
            }
        }
    }
    void Flee()
    {

    }
    void States()
    {
        switch (myState)
        {
            case RunnerState.Start:
                Spawn();
                FindAllAvailableNodes();
                FindAllObjectives();
                num++;
                if (num >= 15)
                {
                    myState = RunnerState.Wander;
                }
                break;
            case RunnerState.Wander:
                Movement();
                if (Detection())
                {
                    if (!isSeeker())
                    {
                        isObjective();
                    }
                    else
                    {
                        myState = RunnerState.Flee;
                    }
                }
                if (target != null)
                {
                    haveTarget();
                }
                break;
            case RunnerState.Flee:
                Movement();
                #region Sprint
                if (!sprint)
                {
                    speed = 5f;
                    timeStamp = Time.time + 2f;
                    sprint = true;
                }
                if (timeStamp < Time.time)
                {
                    speed = 2f;
                    timeStamp = Time.time + 10f;
                    myState = RunnerState.Wander;
                }
                #endregion

                break;
            case RunnerState.Pickup:
                break;
            default:
                break;
        }
    }
}
