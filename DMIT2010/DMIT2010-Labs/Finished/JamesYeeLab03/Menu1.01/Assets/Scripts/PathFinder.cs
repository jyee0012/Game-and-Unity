using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    #region Variables
    public GameObject currentNode, targetNode, lastNode, destinationNode;
    public Vector3 destination;
    protected GameObject finalDest, target = null;
    GameObject[] allNodes;
    List<GameObject> pathList;
    PathnodeScript currentNodeScript;

    public float speed = 2f;
    bool smartMove = false, isLast = false;

    protected RaycastHit hitObject;
    protected Vector3 fwd, right, left, givenDirection, direction = Vector3.zero;
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
            if (destinationNode != null)
            {
                destination = destinationNode.transform.position;
            }
            if (Vector3.Distance(destination, transform.position) < 0.1f)
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
                    destinationNode = GenerateRandDestNode();
                    destination = destinationNode.transform.position;
                }
                #endregion

                #region Move Towards Destination
                // have a starting node in case that the loop chooses nothing
                //targetNode = currentNodeScript.connections[0];
                // loop through to check if there is a better targetNode
                foreach (GameObject node in currentNodeScript.connections)
                {
                    if (Vector3.Distance(destination, targetNode.transform.position) >
                                Vector3.Distance(destination, node.transform.position)
                                && node != lastNode)

                    {
                        targetNode = node;
                    }
                    #region Some Stupid Smart Deadend Shit
                    //if (currentNodeScript.connections.Count == 1)
                    //{
                    //    isLast = true;
                    //}
                    //else if (currentNodeScript.connections.Count > 2 && isLast)
                    //{
                    //    isLast = false;
                    //}
                    //if (!isLast)
                    //{
                    //    if (node != lastNode)
                    //    {
                    //        targetNode = node;
                    //    }
                    //    if (Vector3.Distance(destination, targetNode.transform.position) >
                    //            Vector3.Distance(destination, node.transform.position)
                    //            && node != lastNode)

                    //    {
                    //        targetNode = node;
                    //    }
                    //}
                    //else
                    //{
                    //    targetNode = node;
                    //}
                    #endregion
                }
                #endregion
            }
            #endregion
        }
        else
        {
            BetweenNodeMovement();
        }
    }
    #endregion

    #region FindPath
    public void FindPath(GameObject target)
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
    #region Between Node Movement
    void BetweenNodeMovement()
    {
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
    }
    #endregion
    #region Generate Random Destination Node
    GameObject GenerateRandDestNode()
    {
        allNodes = GameObject.FindGameObjectsWithTag("Pathnode");
        GameObject randNode= null;
        int randomDest = Random.Range(0, allNodes.Length);
        for (int i = 0; i < allNodes.Length; i++)
        {
            if (i == randomDest)
            {
                randNode = allNodes[i];
            }
        }
        return randNode;
    }
    #endregion
    public void myTarget(GameObject target)
    {
        destination = target.transform.position;
    }
    #region Detection
    public void Detection()
    {
        bool doAthing = false;
        foreach (Collider thing in Physics.OverlapSphere(transform.position + (fwd * 5), minForwardDist))
        {
            if (thing.tag != "Floor" || thing.tag != "Wall" || thing.tag != "Runner")
            {
                doAthing = true;
                direction = Vector3.Normalize(thing.transform.position - transform.position);
            }
        }
        if (doAthing)
        {
            Debug.Log(Physics.OverlapSphere(transform.position + (fwd * 5), minForwardDist).Length);
            // can I see it?
            if (Physics.SphereCast(transform.position, 0.5f, fwd, out hitObject, minForwardDist))
            {
                if (hitObject.transform.tag == "Objective")
                {
                    Debug.Log("It's an Objective!");
                    destination = hitObject.transform.position;
                    destinationNode = FindNearestPathNode(destination);
                }
            }
            doAthing = false;
        }
    }
    #endregion
    #region Find the Nearest Node given a Vector3(Destination) and returns a GameObject(PathNode)
    GameObject FindNearestPathNode(Vector3 findMe)
    {
        GameObject theNode = GameObject.FindGameObjectsWithTag("Pathnode")[0];
        foreach (GameObject node in GameObject.FindGameObjectsWithTag("Pathnode"))
        {
            if (Vector3.Distance(node.transform.position, findMe) < Vector3.Distance(theNode.transform.position, findMe))
            {
                theNode = node;
            }
        }
        return theNode;
    }
    #endregion
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
    #region Gismos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(destination + new Vector3(0, 5, 0), 0.5f);
        Gizmos.DrawSphere(transform.position + (fwd * 5), minForwardDist);
    }
    #endregion
}
