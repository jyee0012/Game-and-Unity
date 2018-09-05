using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    //PathNode[] connections;
    Transform target;
    GameObject[] otherNodes;
    public List<GameObject> connections;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BuildPaths();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
    void BuildPaths()
    {
        connections = new List<GameObject>();

        otherNodes = GameObject.FindGameObjectsWithTag("PathNode");

        foreach(GameObject target in otherNodes)
        {
            if (target != null && target.transform != this.transform)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= 20.0f)
                {
                    Debug.DrawLine(transform.position, target.transform.position, Color.red, 200, false);
                    connections.Add(target);
                }
            }
        }
    }
}
