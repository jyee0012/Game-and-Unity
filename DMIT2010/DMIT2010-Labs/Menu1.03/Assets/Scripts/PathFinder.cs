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
    public List<GameObject> pathList, myNodes;
    PathnodeScript currentNodeScript;

    public float speed = 2f;
    public int objectiveCount = -100;
    bool smartMove = false;

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
    public void FindAllAvailableNodes()
    {
        // make a duplicate of myNodes list to edit
        if (!myNodes.Contains(currentNode))
        {
            myNodes.Add(currentNode);
        }
        for (int i = 0; i < myNodes.Count; i++)
        {
            foreach (GameObject connect in myNodes[i].GetComponent<PathnodeScript>().connections)
            {
                if (!myNodes.Contains(connect))
                {
                    myNodes.Add(connect);
                }
            }
        }
    }
    public void FindAllObjectives()
    {
        objectiveCount = GameObject.FindGameObjectsWithTag("Objective").Length;
    }
    #endregion
    // break down movement into smaller parts to code inbetween
    #region Movement
    // Movement
    public void Movement()
    {
        // Update all nodes when I reach target node
        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
        {
            #region Update Variables

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
            if (target != null)
            {
                myTarget();
            }
            currentNode = targetNode;
            currentNodeScript = currentNode.GetComponent<PathnodeScript>();
            currentNodeScript.Use();
            lastNode.GetComponent<PathnodeScript>().notUse();
            #endregion
            #region Smart Movement
            #region StupidMovement
            if (smartMove)
            {
                targetNode = StupidMovement(currentNodeScript);
            }
            else
            #endregion
            {
                #region Generate Random Destination
                if (target == null && destination == Vector3.zero)
                {
                    destinationNode = GenerateRandDestNode();
                    destination = destinationNode.transform.position;
                }
                #endregion

                #region Move Towards Destination
                // loop through to check if there is a better targetNode
                foreach (GameObject node in currentNodeScript.connections)
                {
                    if (Vector3.Distance(destination, targetNode.transform.position) >
                                Vector3.Distance(destination, node.transform.position)
                                && (node != lastNode))
                    //node.GetComponent<PathnodeScript>().ifUse() // breaks movement

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
            // Between Node Movement
            fwd = transform.TransformDirection(Vector3.forward);
            right = transform.TransformDirection(Vector3.right);
            left = transform.TransformDirection(Vector3.left);
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
        transform.LookAt(targetNode.transform.position);
        #region Fly Towards the Objective
        //if (target == null)
        //{
        //    transform.LookAt(targetNode.transform.position);
        //}
        //else
        //{
        //    transform.LookAt(target.transform.position);
        //}
        #endregion
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
        GameObject randNode = null;
        int randomDest;
        if (myNodes.Count < 5)
        {
            randomDest = Random.Range(0, allNodes.Length);
            for (int i = 0; i < allNodes.Length; i++)
            {
                if (i == randomDest)
                {
                    randNode = allNodes[i];
                }
            }
        }
        else
        {
            randomDest = Random.Range(0, myNodes.Count);
            for (int i = 0; i < myNodes.Count; i++)
            {
                if (i == randomDest)
                {
                    randNode = myNodes[i];
                }
            }
        }
        return randNode;
    }
    #endregion
    #region Stupid Movement
    // Given a nodeScript and returns a node
    GameObject StupidMovement(PathnodeScript currentNodeScript)
    {
        GameObject theOnlyNode = null;
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
                theOnlyNode = node;
            }
        }
        return theOnlyNode;
    }
    #endregion
    #region My Target
    public void myTarget(GameObject target)
    {
        destinationNode = FindNearestPathNode(target.transform.position);
        destination = destinationNode.transform.position;
        if (this.target == null)
        {
            this.target = target;
        }
    }
    public void myTarget()
    {
        destination = destinationNode.transform.position;
    }
    public void avoidTarget(GameObject target)
    {
        destinationNode = FindFurthestPathNode(target.transform.position);
        destination = destinationNode.transform.position;
    }
    #endregion
    #region Detection
    public bool Detection()
    {
        bool returnValue = false;
        foreach (Collider thing in Physics.OverlapSphere(transform.position + (fwd * 5), minForwardDist))
        {
            //Debug.Log(thing.tag); // What have I detected?
            if (thing.tag != "Floor" && thing.tag != "Wall" && thing.tag != transform.tag)
            {
                returnValue = true;
            }

        }
        return returnValue;
    }
    public void isObjective()
    {
        if (Physics.SphereCast(transform.position + (fwd * 4) + new Vector3(0, 5, 0), 5.0f, fwd, out hitObject, minForwardDist, 1 << 9))
        {
            if (hitObject.transform.tag == "Objective" || (hitObject.transform.tag == "Goal" && objectiveCount <= 0))
            {
                if (target == null)
                {
                    myTarget(hitObject.transform.gameObject);
                }

            }
        }
    }
    public void isRunner()
    {
        if (Physics.SphereCast(transform.position + (fwd * 4) + new Vector3(0, 5, 0), 5.0f, fwd, out hitObject, minForwardDist, 1 << 10))
        {
            if (hitObject.transform.tag == "Runner")
            {
                if (target == null)
                {
                    myTarget(hitObject.transform.gameObject);
                }

            }
        }
    }
    public bool isSeeker()
    {
        bool value = false;
        if (Physics.SphereCast(transform.position + (fwd * 4) + new Vector3(0, 5, 0), 5.0f, fwd, out hitObject, minForwardDist, 1 << 11))
        {
            if (hitObject.transform.tag == "Seeker")
            {
                value = true;
                avoidTarget(hitObject.transform.gameObject);
            }
        }
        return value;
    }
    public void haveTarget()
    {
        //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
        if (Vector3.Distance(transform.position, target.transform.position) < 3.0f)
        {
            if (target.transform.tag == "Objective")
            {
                objectiveCount--;
                Destroy(target);
                target = null;
            }
            if (objectiveCount == 0)
            {
                // You Win!
                GameObject.FindGameObjectWithTag("Door").transform.Translate(new Vector3(0, 10, 0));
                EditorMenu.BuildPaths();
            }
        }
    }
    public void haveTarget(bool isSeeker)
    {
        if (isSeeker)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            if (Vector3.Distance(transform.position, target.transform.position) < 3.0f)
            {
                // You Lose!
                Destroy(target.transform.parent.gameObject);
                target = null;
            }
        }
    }
    public void findTarget()
    {

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
    #region Find the Furthest Node given a Vector3(Destination) and returns a GameObject(PathNode)
    GameObject FindFurthestPathNode(Vector3 findMe)
    {
        GameObject theNode = myNodes[0];
        foreach (GameObject node in myNodes)
        {
            if (Vector3.Distance(node.transform.position, findMe) > Vector3.Distance(theNode.transform.position, findMe))
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
        Gizmos.DrawSphere(destination + new Vector3(0, 5, 0), 0.5f); // Destination
        Gizmos.DrawWireSphere(transform.position + (fwd * 5), minForwardDist); // Overlap Sphere
        //Gizmos.DrawWireSphere(transform.position + (fwd * 4) + new Vector3(0, 5, 0), 5.0f); // SphereCast
    }
    #endregion
    #region Stuck
    public bool amStuck()
    {
        return targetNode == currentNode && currentNode == lastNode;
    }
    public void getUnstuck()
    {
        targetNode = currentNodeScript.connections[Random.Range(0, currentNodeScript.connections.Count)];
    }
    #endregion
}
