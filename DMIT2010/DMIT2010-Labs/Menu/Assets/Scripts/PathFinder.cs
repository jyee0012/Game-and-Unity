using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    #region Variables
    public GameObject currentNode, targetNode, lastNode, destinationNode = null;
    public Vector3 destination;
    protected GameObject finalDest, target = null;
    GameObject[] allNodes;
    List<GameObject> pathList;
    PathnodeScript currentNodeScript;

    public float speed = 2f;
    bool smartMove = false;

    protected RaycastHit hitObject;
    protected Vector3 fwd, right, left, givenDirection;
    protected float minForwardDist = 10.0f;
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

    }
    #endregion
    #region Spawn
    // If no currentNode, find closest Node and set it as currentNode.
    public void Spawn()
    {
        if (currentNode == null)
        {
            allNodes = GameObject.FindGameObjectsWithTag("Pathnode");

            currentNode = allNodes[0];
            for (int i = 1; i < allNodes.Length; i++)
            {
                if (Vector3.Distance(transform.position, allNodes[i].transform.position) < Vector3.Distance(transform.position, currentNode.transform.position))
                {
                    currentNode = allNodes[i];
                }
            }
            transform.position = currentNode.transform.position;
            targetNode = currentNode;
            destinationNode = targetNode;
        }
    }
    #endregion
    // break down movement into smaller parts to code inbetween
    #region Movement
    // Movement
    public void Movement()
    {
        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
        {
            #region Update Variables

            fwd = transform.TransformDirection(Vector3.forward);
            right = transform.TransformDirection(Vector3.right);
            left = transform.TransformDirection(Vector3.left);
            #region Updating Nodes
            transform.position = targetNode.transform.position;
            lastNode = currentNode;
            if (Vector3.Distance(destination, transform.position) < 0.5f)
            {
                destination = Vector3.zero;
            }
            currentNode = targetNode;
            currentNodeScript = currentNode.GetComponent<PathnodeScript>();
            #endregion
            #endregion
            #region Smart Movement
            #region StupidMovement
            if (smartMove)
            {
                for (int i = 0; i < currentNodeScript.connections.Count; i++)
                {
                    GameObject node = null;
                    node = currentNodeScript.connections[Random.Range(0, currentNodeScript.connections.Count)];
                    // if i have a destination
                    if (destinationNode != null)
                    {
                        // check if my next node is closer than out of all of them?
                        if (Vector3.Distance(destinationNode.transform.position, node.transform.position) > Vector3.Distance(destinationNode.transform.position, currentNodeScript.connections[i].transform.position))
                        {
                            node = currentNodeScript.connections[i];
                        }
                    }
                    // is it the lastnode i was on?
                    if (node != lastNode)
                    {
                        targetNode = node;
                    }
                }
            }
            else
            #endregion
            {
                #region Generate Random Destination
                if (destination == Vector3.zero)
                {
                    int randomDest = Random.Range(0, allNodes.Length);
                    for (int i = 0; i < allNodes.Length; i++)
                    {
                        if (i == randomDest)
                        {
                            destinationNode = allNodes[i];
                            destination = destinationNode.transform.position;
                        }
                    }
                }
                #endregion

                #region Move Towards Destination
                // have a starting node in case that the loop chooses nothing
                targetNode = currentNodeScript.connections[0];
                // loop through to check if there is a better targetNode
                foreach (GameObject node in currentNodeScript.connections)
                {
                    if (Vector3.Distance(destination, targetNode.transform.position) >
                        Vector3.Distance(destination, node.transform.position) && node != lastNode)
                    {
                        targetNode = node;
                    }
                }
                #endregion
            }
            #endregion
        }
        else
        {
            #region Between Node Movement
            if (target == null)
            {
                transform.LookAt(targetNode.transform.position);
            }
            else
            {
                transform.LookAt(target.transform.position);
            }
            // move forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            #region Failed Movement
            // three different methods to move around

            // moves normally
            // target minus me equals distance between
            //transform.Translate(Vector3.Normalize(targetNode.transform.position - transform.position) * Time.deltaTime * speed);

            // slides to targetNode then waits
            //transform.position = Vector3.Lerp(transform.position, targetNode.transform.position, Time.deltaTime * speed);

            // zips right off the world
            //transform.Translate(Vector3.MoveTowards(transform.position, targetNode.transform.position, 0.0000001f));
            #endregion
            #endregion
        }
    }
    #endregion

    #region FindPath
    void FindPath(GameObject target)
    {
        finalDest = destinationNode;
        pathList.Add(finalDest);
        foreach (GameObject node in pathList)
        {
            foreach (GameObject connect in node.GetComponent<PathnodeScript>().connections)
            {

            }
        }
    }
    #endregion

    void myTarget(GameObject target)
    {
        destination = target.transform.position;
    }
    void Detection()
    {
        if (Physics.SphereCast(transform.position, 0.5f, fwd, out hitObject, minForwardDist))
        {
            
        }
    }
    #region Move
    void MoveTowards(Vector3 direction)
    {
        destination = transform.position + (direction * 2);
    }
    void MoveAway(Vector3 direction)
    {
        destination = transform.position - (direction * 2);
    }
    #endregion
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(destination + new Vector3(0,5,0), 0.5f);
    }
}
