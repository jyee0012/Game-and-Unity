using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    //PathNode[] connections;
    Transform target;
    GameObject[] otherNodes;
    //public List<GameObject> connections;
    public List<PathNode> connections = new List<PathNode>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
    #region BuildPaths
    /*
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
    */
    #endregion
    public void AddConnection(PathNode node)
    {
        connections.Add(node);
    }

    void ShowPaths()
    {
        foreach (PathNode target in connections)
        {
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z),
                           new Vector3(target.transform.position.x, target.transform.position.y + 0.25f, target.transform.position.z),
                           Color.white, 0.01f, false);
        }
    }
}
