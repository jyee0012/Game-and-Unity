using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
public class EditorMenu : EditorWindow
{
    static GameObject node;
    static GameObject[] nodes;
    static RaycastHit hitSurface;
    static float PN_space = 5, PN_buildDist = 7.5f;
    #region Generate Floor Nodes
    // Works as long as trigger is has Floor tag and is in ignore raycast layer
    [MenuItem("Grid Manager/Generate Floor Nodes", priority = 0)]
    static void SpawnNodes()
    {
        Transform floor;
        int PN_count = 0;
        Vector3 floorExtents;
        //spawner = GameObject.FindGameObjectsWithTag("Spawn");
        node = Resources.Load("PathNode") as GameObject;

        ClearNodes();
        //foreach (GameObject spawn in spawner)
        //{
        //    Instantiate(sphere, spawn.transform.position, spawn.transform.rotation);
        //}
        floor = Selection.transforms[0];
        float paddingX = floor.gameObject.GetComponent<BoxCollider>().bounds.extents.x % PN_space,
            paddingZ = floor.gameObject.GetComponent<BoxCollider>().bounds.extents.z % PN_space;

        if (paddingX > -1 && paddingX < 1)
        {
            paddingX = PN_space / 2;
        }
        if (paddingZ > -1 && paddingZ < 1)
        {
            paddingZ = PN_space / 2;
        }
        floorExtents = floor.gameObject.GetComponent<BoxCollider>().bounds.extents - new Vector3((0.5f * 2) - (paddingX / 2), 0, (0.5f * 2) - (paddingZ / 2));

        for (float x = floorExtents.x * -2; x <= 0; x += PN_space)
        {
            for (float z = floorExtents.x * -2; z <= 0; z += PN_space)
            {
                Vector3 location = floor.position + floorExtents + new Vector3(x, 0, z);
                if (floor.gameObject.GetComponent<BoxCollider>().isTrigger)
                {
                    Physics.Raycast(location, Vector3.down, out hitSurface);
                    location = floor.position + floorExtents + new Vector3(x, -hitSurface.distance, z);
                    if (hitSurface.collider != null)
                    {
                        GameObject PathnodeInstance = Instantiate(node, location, floor.rotation, floor.transform) as GameObject;
                        PathnodeInstance.name = "PathNode " + PN_count;
                        PN_count++;
                    }
                }
                else
                {
                    GameObject PathnodeInstance = Instantiate(node, location, floor.rotation, floor.transform) as GameObject;
                    PathnodeInstance.name = "PathNode " + PN_count;
                    PN_count++;
                }
            }
        }
    }
    [MenuItem("Grid Manager/Generate Floor Nodes", true)]
    static bool ValidateSpawnNodes()
    {
        bool validate = true;
        if (Selection.transforms.Length == 1)
        {
            foreach (Transform thing in Selection.transforms)
            {
                if (thing.tag != "Floor")
                {
                    validate = false;
                }
            }
        }
        else
        {
            validate = false;
        }
        return validate;
    }
    #endregion
    #region Clear Floor Nodes
    // Works as long as trigger is has Floor tag and is in ignore raycast layer
    [MenuItem("Grid Manager/Clear Floor Nodes", priority = 0)]
    static void ClearNodes()
    {
        Transform floor = Selection.transforms[0];
        int totalChildren = floor.childCount;
        for (int c = 0; c < totalChildren; c++)
        {
            if (floor.GetChild(0).tag == "Pathnode")
            {
                DestroyImmediate(floor.GetChild(0).gameObject);
            }
        }
    }
    [MenuItem("Grid Manager/Clear Floor Nodes", true)]
    static bool ValidateClearNodes()
    {
        bool validate = true;
        if (Selection.transforms.Length == 1)
        {
            foreach (Transform thing in Selection.transforms)
            {
                if (thing.tag != "Floor")
                {
                    validate = false;
                }
            }
        }
        else
        {
            validate = false;
        }
        return validate;
    }
    #endregion
    #region Clear All Nodes
    [MenuItem("Grid Manager/Clear All Nodes", priority = 0)]
    static void ClearAllNodes()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                DestroyImmediate(nodes[i]);
            }
        }
    }
    #endregion
    #region Path Building
    #region Build Paths
    [MenuItem("Grid Manager/Path Building/Build Paths", priority = 1)]
    public static void BuildPaths()
    {
        ClearPaths();
        nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        #region For Loop
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int c = 0; c < nodes.Length; c++)
            {
                // fix pathnodes inside walls
                bool inSomething = true;
                Collider[] overlapSpheres = Physics.OverlapSphere(nodes[i].transform.position, 0.5f);
                if (overlapSpheres.Length > 1)
                {
                    inSomething = false;
                }

                if (inSomething)
                {
                    if (Vector3.Distance(nodes[i].transform.position, nodes[c].transform.position) <= (PN_buildDist + 0.1f) && nodes[i] != nodes[c])
                    {
                        //Add Connections
                        nodes[i].GetComponent<PathnodeScript>().AddConnection(nodes[c]);
                        EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                    }
                }
            }
        }
        #endregion
        foreach (GameObject node in nodes)
        {
            node.GetComponent<PathnodeScript>().ForceAllConnections();
        }
    }
    #endregion
    #region Clear Paths
    [MenuItem("Grid Manager/Path Building/Clear Paths", priority = 1)]
    static void ClearPaths()
    {
        nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int c = 0; c < nodes.Length; c++)
            {
                if (Vector3.Distance(nodes[i].transform.position, nodes[c].transform.position) <= 5.0f && nodes[i] != nodes[c])
                {
                    nodes[i].GetComponent<PathnodeScript>().ClearConnections();
                    EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                }
            }
        }
    }
    #endregion
    #region Clear Path Memory
    [MenuItem("Grid Manager/Path Building/Clear Path Memory", priority = 1)]
    static void ClearMemory()
    {
        foreach (GameObject nodes in GameObject.FindGameObjectsWithTag("Pathnode"))
        {
            nodes.GetComponent<PathnodeScript>().TrueClearConnections();
        }
    }
    #endregion
    #region Force Paths
    [MenuItem("Grid Manager/Path Building/Force Paths", priority = 1)]
    static void ForcePaths()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                check = true;
            }
        }
        if (check)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                for (int c = 0; c < nodes.Length; c++)
                {
                    nodes[i].GetComponent<PathnodeScript>().ForceConnection(nodes[c]);
                    EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                }
            }
        }
    }
    [MenuItem("Grid Manager/Path Building/Force Paths", true)]
    static bool ValidateForcedPaths()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC >= 2;
    }
    #endregion
    #region Clear Forced Paths
    [MenuItem("Grid Manager/Path Building/Clear Force Paths", priority = 1)]
    static void ClearForcePaths()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        if (nodes.Length >= 2)
        {
            foreach (GameObject node in nodes)
            {
                if (node.tag == "Pathnode")
                {
                    check = true;
                }
            }
            if (check)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int c = 0; c < nodes.Length; c++)
                    {
                        nodes[i].GetComponent<PathnodeScript>().ClearForced(nodes[c]);
                        EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                    }
                }
            }
        }
        else
        {
            nodes[0].GetComponent<PathnodeScript>().ClearAllForced();
        }
        BuildPaths();
    }
    [MenuItem("Grid Manager/Path Building/Clear Force Paths", true)]
    static bool ValidateClearForcedPaths()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC >= 1;
    }
    #endregion
    #region Block Paths
    [MenuItem("Grid Manager/Path Building/Block Paths", priority = 1)]
    static void BlockPaths()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                check = true;
            }
        }
        if (check)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                for (int c = 0; c < nodes.Length; c++)
                {
                    nodes[i].GetComponent<PathnodeScript>().BlockConnection(nodes[c]);
                    EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                }
            }
        }
    }
    [MenuItem("Grid Manager/Path Building/Block Paths", true)]
    static bool ValidateBlockedPaths()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC >= 2;
    }
    #endregion
    #region Clear Blocked Paths
    [MenuItem("Grid Manager/Path Building/Clear Blocked Paths", priority = 1)]
    static void ClearBlockedPaths()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        if (nodes.Length >= 2)
        {
            foreach (GameObject node in nodes)
            {
                if (node.tag == "Pathnode")
                {
                    check = true;
                }
            }
            if (check)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int c = 0; c < nodes.Length; c++)
                    {
                        nodes[i].GetComponent<PathnodeScript>().ClearBlocked(nodes[c]);
                        EditorUtility.SetDirty(nodes[i].GetComponent<PathnodeScript>());
                    }
                }
            }
        }
        else
        {
            nodes[0].GetComponent<PathnodeScript>().ClearAllBlocked();
        }
        BuildPaths();
    }
    [MenuItem("Grid Manager/Path Building/Clear Blocked Paths", true)]
    static bool ValidateClearBlockedPaths()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC >= 1;
    }
    #endregion
    #region One Way Path Forward
    [MenuItem("Grid Manager/Path Building/Build One Way Path Forward", priority = 1)]
    static void OneWayPath()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                check = true;
            }
        }
        if (check)
        {
            //Build One Way
            nodes[0].GetComponent<PathnodeScript>().AddConnection(nodes[1]);
            nodes[0].GetComponent<PathnodeScript>().ForceConnection(nodes[1]);
            nodes[1].GetComponent<PathnodeScript>().connections.Remove(nodes[0]);
            nodes[1].GetComponent<PathnodeScript>().BlockConnection(nodes[0]);
        }
    }
    [MenuItem("Grid Manager/Path Building/Build One Way Path Forward", true)]
    static bool ValidateOneWayPath()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC == 2;
    }
    #endregion
    #region One Way Path Backward
    [MenuItem("Grid Manager/Path Building/Build One Way Path Backward", priority = 1)]
    static void OneWayPathB()
    {
        nodes = Selection.gameObjects;
        bool check = false;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                check = true;
            }
        }
        if (check)
        {
            //Build One Way
            nodes[1].GetComponent<PathnodeScript>().AddConnection(nodes[0]);
            nodes[1].GetComponent<PathnodeScript>().ForceConnection(nodes[0]);
            nodes[0].GetComponent<PathnodeScript>().connections.Remove(nodes[1]);
            nodes[0].GetComponent<PathnodeScript>().BlockConnection(nodes[1]);
        }
    }
    [MenuItem("Grid Manager/Path Building/Build One Way Path Backward", true)]
    static bool ValidateOneWayPathB()
    {
        nodes = Selection.gameObjects;
        int pathnodeC = 0;
        foreach (GameObject node in nodes)
        {
            if (node.tag == "Pathnode")
            {
                pathnodeC++;
            }
        }
        return pathnodeC == 2;
    }
    #endregion
    #region Draw What
    [MenuItem("Grid Manager/Path Building/Draw All", priority = 1)]
    static void DrawAll()
    {
        nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        foreach (GameObject node in nodes)
        {
            node.GetComponent<PathnodeScript>().drawWhat = PathnodeScript.DrawState.All;
        }
    }
    [MenuItem("Grid Manager/Path Building/Draw Not White", priority = 1)]
    static void DrawNotWhite()
    {
        nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        foreach (GameObject node in nodes)
        {
            node.GetComponent<PathnodeScript>().drawWhat = PathnodeScript.DrawState.NotWhite;
        }
    }
    [MenuItem("Grid Manager/Path Building/Draw Nothing", priority = 1)]
    static void DrawNothing()
    {
        nodes = GameObject.FindGameObjectsWithTag("Pathnode");
        foreach (GameObject node in nodes)
        {
            node.GetComponent<PathnodeScript>().drawWhat = PathnodeScript.DrawState.Nothing;
        }
    }
    #endregion
    #endregion
    #region Node Settings
    [MenuItem("Grid Manager/Node Settings", priority = 2)]
    static void Init()
    {
        EditorMenu NodeSettings = (EditorMenu)EditorWindow.GetWindow(typeof(EditorMenu), true, "Node Settings");
        NodeSettings.Show();
    }
    void OnGUI()
    {
        GUILayout.Label("Node Settings", EditorStyles.boldLabel);
        PN_space = EditorGUILayout.FloatField("Path Node Spacing", PN_space);
        PN_buildDist = EditorGUILayout.FloatField("Path Building Distance", PN_buildDist);
    }
    #endregion
}
