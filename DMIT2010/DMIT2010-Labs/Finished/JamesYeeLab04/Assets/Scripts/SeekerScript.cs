using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerScript : PathFinder
{
    public enum SeekerState { Start, Search, Chase }
    public SeekerState myState = SeekerState.Start;
    int num = 0;
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
    }
    void States()
    {
        switch (myState)
        {
            case SeekerState.Start:
                Spawn();
                FindAllAvailableNodes();
                FindAllObjectives();
                num++;
                if (num >= 15)
                {
                    myState = SeekerState.Search;
                }
                break;
            case SeekerState.Search:
                Movement();
                if (Detection())
                {
                    isRunner();
                }
                if (target != null)
                {
                    myState = SeekerState.Chase;
                }
                break;
            case SeekerState.Chase:
                Movement();
                if (target != null)
                {
                    haveTarget(true);
                }
                else
                {
                    myState = SeekerState.Search;
                }
                break;
        }
    }
}
