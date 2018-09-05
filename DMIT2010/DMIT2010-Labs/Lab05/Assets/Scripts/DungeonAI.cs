using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAI : MonoBehaviour
{

    enum State { Explore, Loot, Attack, Rest, GoHome, Sleep, GearUp }
    State myState = State.Explore;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
