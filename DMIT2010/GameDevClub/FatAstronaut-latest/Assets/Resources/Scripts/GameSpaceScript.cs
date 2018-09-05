using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpaceScript : MonoBehaviour
{
    #region Variables
    public enum SpaceType { Normal, Block, Spawn, Goal, Other, Path }
    public SpaceType thisSpace = SpaceType.Normal;
    public List<GameObject> connections, VicPath;
    int amountBlocked = 0;
    public bool VicPathDone = false, beenTo = false;
    #endregion
    #region Start
    // Use this for initialization
    void Start()
    {

    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        //ColorManage();
        LockPrevent();
    }
    #endregion
    #region Gizmos
    void onDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (GameObject connect in connections)
        {
            Gizmos.DrawLine(transform.position, connect.transform.position);
        }
    }
    #endregion
    #region Color Management
    void ColorManage()
    {
        switch (thisSpace)
        {
            case SpaceType.Normal:
                //Color.white;
                this.gameObject.GetComponent<Renderer>().material = Resources.Load("White") as Material;
                break;
            case SpaceType.Block:
                //Color.red;
                this.gameObject.GetComponent<Renderer>().material = Resources.Load("Red") as Material;
                break;
            case SpaceType.Goal:
                //Color.green;
                this.gameObject.GetComponent<Renderer>().material = Resources.Load("Green") as Material;
                break;
            case SpaceType.Spawn:
                //Color.blue;
                this.gameObject.GetComponent<Renderer>().material = Resources.Load("Blue") as Material;
                break;
            case SpaceType.Path:
                this.gameObject.GetComponent<Renderer>().material = Resources.Load("Yellow") as Material;
                break;
            default:
                break;
        }
    }
    #endregion
    #region Lock Prevention
    void LockPrevent()
    {
        // check if I am either goal or spawn
        if (thisSpace == SpaceType.Spawn || thisSpace == SpaceType.Goal)
        {
            // for each connection check if it is a block
            foreach (GameObject connect in connections)
            {
                GameSpaceScript connectScript = connect.GetComponent<GameSpaceScript>();
                if (connectScript.thisSpace == SpaceType.Block)
                {
                    amountBlocked++;
                }
            }
            // if my amount of block is equal to the amount of connections I have
            if (amountBlocked == connections.Count)
            {
                // choose a random connection and make it not a block.
                connections[Random.Range(0, connections.Count - 1)].GetComponent<GameSpaceScript>().thisSpace = SpaceType.Normal;
            }
        }
    }
    #endregion
    #region Connections
    public void AddConnections(GameObject space)
    {
        connections.Add(space);
    }
    public void RemoveConnections(GameObject space)
    {
        connections.Remove(space);
    }
    public void ClearConnections()
    {
        connections.Clear();
    }
    public bool CheckLock()
    {
        foreach (GameObject connect in connections)
        {
            GameSpaceScript connectScript = connect.GetComponent<GameSpaceScript>();
            if (connectScript.thisSpace == SpaceType.Block)
            {
                amountBlocked++;
            }
        }
        return (amountBlocked == connections.Count);
    }
    public bool CheckLock(List<GameObject> connectList)
    {
        foreach (GameObject connect in connectList)
        {
            GameSpaceScript connectScript = connect.GetComponent<GameSpaceScript>();
            if (connectScript.thisSpace == SpaceType.Block)
            {
                amountBlocked++;
            }
        }
        return (amountBlocked == connectList.Count);
    }
    #endregion
    #region VicPath
    public void CreateVicPath()
    {
        GameObject goalSpace = null, spawnSpace = null;
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");

        // find and save goal and spawn. **Functional
        #region Save Goal&Spawn
        foreach (GameObject space in allSpace)
        {
            GameSpaceScript spaceScript = space.GetComponent<GameSpaceScript>();
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Spawn)
            {
                // saves the variable for spawn
                spawnSpace = space;
            }
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Goal)
            {
                // saves the variable for goal
                goalSpace = space;
            }
        }
        #endregion
        // Takes the goal and finds the space closest to spawn and connects to it.
        // repeat the process for each connection it makes until it reaches spawn.
        #region Pathing
        // adds the goal to the list if it isn't already there.
        if (!VicPath.Contains(goalSpace))
        {
            VicPath.Add(goalSpace);
        }
        // for loop that goes through the size of the victory path.
        for (int i = 0; i < VicPath.Count; i++)
        {
            bool breakloop = false;
            GameObject oneConnect = null;
            //Debug.Log(VicPath[i].gameObject.name + ':' + VicPath.Count);
            GameSpaceScript spotScript = VicPath[i].GetComponent<GameSpaceScript>();
            // for each connection of the current space
            foreach (GameObject connect in spotScript.connections)
            {
                // check if it is a block
                if (connect.GetComponent<GameSpaceScript>().thisSpace != GameSpaceScript.SpaceType.Block)
                {
                    // if the distance between my current space is closer than one of my connections and it doesnt already exist inside my list
                    if (Vector3.Distance(spawnSpace.transform.position, VicPath[i].transform.position) >
                          Vector3.Distance(spawnSpace.transform.position, connect.transform.position) && !VicPath.Contains(connect))
                    {
                        // makes this space the temp connection
                        oneConnect = connect;

                    }
                    else
                    {
                        breakloop = true;
                        break;
                    }
                }
            }

            if (breakloop)
                break;
            // checks if it is spawn
            if (VicPath[i] == spawnSpace)
            {
                //Debug.Log("Did I break?");
                VicPathDone = true;
                break;
            }
            else
            {
                // add the space into the victory path list
                VicPath.Add(oneConnect);
            }
            // since the victory path list increased while still inside the list, the list continues to repeat.
        }
        #endregion

    }
    #endregion
    #region VicPath Check
    public int VicPathLength()
    {
        // returns the victory path length including the goal and spawn
        return VicPath.Count;
    }
    public bool isVicPathDone()
    {
        // returns boolean that makes sure if the victory path has been completed or not
        return VicPathDone;
    }
    public void DrawVicPath()
    {
        foreach (GameObject path in VicPath)
        {
            if (path.GetComponent<GameSpaceScript>().thisSpace == SpaceType.Normal)
            {
                path.GetComponent<GameSpaceScript>().thisSpace = SpaceType.Path;
            }
        }
    }
    #endregion
    public void IveBeenTo()
    {
        beenTo = true;
    }
}
