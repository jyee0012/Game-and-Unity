using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpaceScript : MonoBehaviour
{
    public enum SpaceType { Normal, Block, Spawn, Goal, Other }
    public SpaceType thisSpace = SpaceType.Normal;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void onDrawGizmos()
    {
        switch (thisSpace)
        {
            case SpaceType.Normal:
                Gizmos.color = Color.white;
                break;
            case SpaceType.Block:
                Gizmos.color = Color.red;
                break;
            case SpaceType.Goal:
                Gizmos.color = Color.green;
                break;
            case SpaceType.Spawn:
                Gizmos.color = Color.blue;
                break;
        }
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
