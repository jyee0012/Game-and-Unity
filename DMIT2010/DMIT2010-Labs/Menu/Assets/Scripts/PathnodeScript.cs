using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathnodeScript : MonoBehaviour
{
    public enum PathNodeType { Regular, DoorOpen, DoorClosed, Drop, Jump }
    public PathNodeType Type = PathNodeType.Regular;
    Color connectionColor;
    public List<GameObject> connections = new List<GameObject>();
    public List<GameObject> forcedConnections = new List<GameObject>();
    public List<GameObject> blockedConnections = new List<GameObject>();
    public bool inUse = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region DrawGizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
        // 10units wide = white
        // 5units wide = green
        // 3units wide = blue
        // <3units wide = red 
        // make sure if one of the connections is one color make the other connection the same color
        foreach (GameObject node in connections)
        {
            RaycastHit SphereCheck;
            connectionColor = Color.white;
            #region OverlapSphere
            if (Physics.OverlapSphere(transform.position + new Vector3(0, 1.5f, 0), 1.5f).Length > 1)
            {
                connectionColor = Color.red;
            }
            else if (Physics.OverlapSphere(transform.position + new Vector3(0, 2.5f, 0), 2.5f).Length > 1)
            {
                connectionColor = Color.blue;
                #region SphereCast
                if (Physics.SphereCast(transform.position, 2.5f, -(transform.position - node.transform.position),out SphereCheck))
                {
                    connectionColor = Color.red;
                }
                #endregion
            }
            else if (Physics.OverlapSphere(transform.position + new Vector3(0, 5f, 0), 5f).Length > 1)
            {
                connectionColor = Color.green;
                #region SphereCast
                if (Physics.SphereCast(transform.position, 2.5f, -(transform.position - node.transform.position), out SphereCheck))
                {
                    Gizmos.color = Color.red;
                }
                else if (Physics.SphereCast(transform.position, 5f, transform.position - node.transform.position, out SphereCheck))
                {
                    Gizmos.color = Color.blue;
                }
                #endregion
            }
            #endregion
            Gizmos.color = connectionColor;
            if (Gizmos.color != Color.white)
            Gizmos.DrawLine(transform.position, node.transform.position + new Vector3(0, 0.5f, 0));
        }
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 1.5f);
        foreach (GameObject node in connections)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), node.transform.position + new Vector3(0, 1.5f, 0));
        }
    }
    #endregion
    #region Connections
    public void AddConnection(GameObject node)
    {
        if (!Physics.Linecast(transform.position + new Vector3(0, 3, 0), node.transform.position + new Vector3(0, 3, 0)))
        {
            if (!blockedConnections.Contains(node))
            {
                connections.Add(node);
            }
        }
    }
    public void ClearConnections()
    {
        connections.Clear();
    }
    #region Force & Blocked
    public void BlockConnection(GameObject node)
    {
        ClearForced(node);

        blockedConnections.Add(node);
        connections.Remove(node);
    }
    public void ForceConnection(GameObject node)
    {
        ClearBlocked(node);

        forcedConnections.Add(node);
        connections.Add(node);
    }
    public void ForceAllConnections()
    {
        foreach(GameObject forced in forcedConnections)
        {
            connections.Add(forced);
        }
    }
    public void ClearForced(GameObject node)
    {
        forcedConnections.Remove(node);
        connections.Remove(node);
    }
    public void ClearAllForced()
    {
        forcedConnections.Clear();
    }
    public void ClearBlocked(GameObject node)
    {
        blockedConnections.Remove(node);
        connections.Add(node);
    }
    public void ClearAllBlocked()
    {
        blockedConnections.Clear();
    }
    #endregion
    public void TrueClearConnections()
    {
        ClearConnections();
        ClearAllBlocked();
        ClearAllForced();
    }
    #endregion
    #region Check if InUse
    public bool amUse()
    {
        return inUse;
    }
    public void notUse()
    {
        inUse = false;
    }
    public void Use()
    {
        inUse = true;
    }
    #endregion
}
