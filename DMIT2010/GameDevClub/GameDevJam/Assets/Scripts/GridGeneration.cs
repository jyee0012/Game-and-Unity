using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGeneration : EditorWindow
{
    static GameObject GameGrid;
    static float gridXsize = 7, gridYsize = 7, DF_spaceBetweenX = 0.5f, DF_spaceBetweenY = 0.5f, spaceBetweenX, spaceBetweenY;
    // max is exclusive
    static int  minBlock = 3, // Always less than maxBlock by at least 1
                maxBlock = 7; // Always less than (gridX * gridY) / 2
    #region Spawn Grid
    [MenuItem("Grid Generation/Spawn Grid", priority = 0)]
    public static void SpawnGrid()
    {
        #region Spawn Variables
        GameGrid = Selection.transforms[0].gameObject;
        GameObject gamePrefab = Resources.Load<GameObject>("GameSpace") as GameObject;
        gamePrefab.gameObject.SetActive(true);
        //Resources.Load<GameObject>("Gamespace").gameObject.GetComponent<BoxCollider>().size; stupid code
        Vector3 prefabBounds = gamePrefab.gameObject.GetComponent<BoxCollider>().size; // / 4.5f only when in actual gamespace, change back when testing
        Vector3 prefabExtents = prefabBounds / 2;
        Vector3 gridSpawn = GameGrid.transform.position + prefabExtents;
        int GS_count = 0;
        spaceBetweenX = DF_spaceBetweenX + (prefabBounds.x * 2);
        spaceBetweenY = DF_spaceBetweenY + (prefabBounds.y * 2);
        #endregion
        ClearSpace();
        for (int i = 0; i < gridXsize; i++)
        {
            for (int c = 0; c < gridYsize; c++)
            {
                Vector3 location = gridSpawn + new Vector3((prefabExtents.x * spaceBetweenX) * i, (prefabExtents.y * spaceBetweenY) * c, 0);
                GameObject spaceInstance = Instantiate(gamePrefab, location, GameGrid.transform.rotation, GameGrid.transform) as GameObject;
                spaceInstance.name = "GameSpace " + GS_count;
                GS_count++;
            }
        }
    }
    [MenuItem("Grid Generation/Spawn Grid", true)]
    static bool ValidateSpawnGrid()
    {
        GameObject[] items = Selection.gameObjects;
        int itemCount = 0;
        foreach (GameObject item in items)
        {
            itemCount++;
        }
        return itemCount == 1;
    }
    #endregion
    #region Clear Grid
    [MenuItem("Grid Generation/Clear Game Space", priority = 0)]
    public static void ClearSpace()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        for (int i = 0; i < allSpace.Length; i++)
        {
            DestroyImmediate(allSpace[i]);
        }
    }
    #endregion
    #region Randomize Grid
    [MenuItem("Grid Generation/Randomize Grid %x", priority = 0)]
    public static void RandomizeGrid()
    {
        #region Variables
        GameObject[] grid = GameObject.FindGameObjectsWithTag("Gamespace");
        int randSpawn = 0, randGoal = 0, randBlockAmount, blockCount = 0;
        int[] randBlock = new int[maxBlock];
        #endregion
        #region Randoms
        randSpawn = Random.Range(0, grid.Length);
        randGoal = Random.Range(0, grid.Length);
        while (randGoal == randSpawn)
        {
            randGoal = Random.Range(0, grid.Length);
        }
        randBlockAmount = Random.Range(minBlock, maxBlock + 1);
        for (int i = 0; i < randBlock.Length; i++)
        {
            randBlock[i] = Random.Range(0, grid.Length);
            while (randBlock[i] == randSpawn && randBlock[i] == randGoal)
            {
                randBlock[i] = Random.Range(0, grid.Length);
            }
        }
        #endregion

        ClearID();
        for (int i = 0; i < grid.Length - 1; i++)
        {
            GameSpaceScript spaceScript = grid[i].GetComponent<GameSpaceScript>();
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Normal)
            {
                if (i == randSpawn)
                {
                    spaceScript.thisSpace = GameSpaceScript.SpaceType.Spawn;
                }
                else if (i == randGoal)
                {
                    spaceScript.thisSpace = GameSpaceScript.SpaceType.Goal;
                }
                //else if ((Random.Range(0, 2) % 2 == 0) && blockCount < randBlockAmount)
                else
                {
                    for (int c = 0; c < randBlock.Length; c++)
                    {
                        if (i == randBlock[c])
                        {
                            spaceScript.thisSpace = GameSpaceScript.SpaceType.Block;
                            blockCount++;
                        }
                    }
                }
            }
        }
    }
    #endregion
    #region Clear Grid ID
    [MenuItem("Grid Generation/Clear Space ID", priority = 0)]
    public static void ClearID()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        foreach (GameObject space in allSpace)
        {
            GameSpaceScript spaceScript = space.GetComponent<GameSpaceScript>();
            spaceScript.thisSpace = GameSpaceScript.SpaceType.Normal;
        }
    }
    #endregion
    #region Connections
    #region Create Connections
    [MenuItem("Grid Generation/Connections/Create Connections", priority = 1)]
    public static void CreateConnections()
    {
        #region Variables
        Vector3 prefabBounds = Resources.Load<GameObject>("Gamespace").gameObject.GetComponent<BoxCollider>().size;
        //  divide by 4.5f only when in actual gamespace, change back when testing
        Vector3 prefabExtents = prefabBounds / 2;
        spaceBetweenX = DF_spaceBetweenX + (prefabBounds.x * 2);

        GameObject[] spaces = GameObject.FindGameObjectsWithTag("Gamespace");
        #endregion
        ClearConnections();
        for (int i = 0; i < spaces.Length; i++)
        {
            for (int c = 0; c < spaces.Length; c++)
            {
                if (Vector3.Distance(spaces[i].transform.position, spaces[c].transform.position) <= ((spaceBetweenX /2) + 0.1f) && spaces[i] != spaces[c])
                {
                    //Add Connections
                    spaces[i].GetComponent<GameSpaceScript>().AddConnections(spaces[c]);
                    EditorUtility.SetDirty(spaces[i].GetComponent<GameSpaceScript>());
                }
            }
        }
    }
    #endregion
    #region Clear Connections
    [MenuItem("Grid Generation/Connections/Clear Connections", priority = 1)]
    public static void ClearConnections()
    {
        foreach (GameObject space in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            space.GetComponent<GameSpaceScript>().ClearConnections();
        }
    }
    #endregion
    #region VictoryPath
    // Create a menuitem that will generate a path/shortest path from spawn to goal.
    [MenuItem("Grid Generation/Connections/Create Victory Path", priority = 1)]
    public static void CreateVicPath()
    {
        GameObject goalSpace = null, spawnSpace = null;
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        //List<GameObject> VicPath = new List<GameObject>();
        foreach (GameObject space in allSpace)
        {
            GameSpaceScript spaceScript = space.GetComponent<GameSpaceScript>();
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Spawn)
            {
                spawnSpace = space;
            }
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Goal)
            {
                goalSpace = space;
            }
        }
        spawnSpace.GetComponent<GameSpaceScript>().CreateVicPath();
        //spawnSpace.GetComponent<GameSpaceScript>().DrawVicPath();
        #region Comment
        /*
        // find and save goal and spawn.
        #region Save Goal&Spawn
        foreach (GameObject space in allSpace)
        {
            GameSpaceScript spaceScript = space.GetComponent<GameSpaceScript>();
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Spawn)
            {
                spawnSpace = space;
            }
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Goal)
            {
                goalSpace = space;
            }
        }
        #endregion
        // Add goal, and start pathing to spawn
        #region Pathing
        VicPath.Add(goalSpace);
        foreach (GameObject spot in VicPath)
        {
            GameSpaceScript spotScript = spot.GetComponent<GameSpaceScript>();
            foreach (GameObject connect in spotScript.connections)
            {
                if (connect.GetComponent<GameSpaceScript>().thisSpace != GameSpaceScript.SpaceType.Block)
                {
                    if (Vector3.Distance(spawnSpace.transform.position, spot.transform.position) >
                          Vector3.Distance(spawnSpace.transform.position, connect.transform.position) && !VicPath.Contains(connect))
                    {
                        VicPath.Add(connect);
                    }
                }
            }
            if (spot == spawnSpace)
            {
                break;
            }
        }
        #endregion
        //spawnSpace.GetComponent<GameSpaceScript>().VicPath = VicPath;
        */
        #endregion
    }
    #endregion
    #endregion
    #region Settings
    [MenuItem("Grid Generation/Spawn Settings", priority = 2)]
    static void Init()
    {
        GridGeneration SpawnSettings = (GridGeneration)EditorWindow.GetWindow(typeof(GridGeneration), true, "Spawn Settings");
        SpawnSettings.Show();
    }
    void OnGUI()
    {
        GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
        gridXsize = EditorGUILayout.FloatField("Grid X Size", gridXsize);
        gridYsize = EditorGUILayout.FloatField("Grid Y Size", gridYsize);
        DF_spaceBetweenX = EditorGUILayout.FloatField("Space Between Grid X", DF_spaceBetweenX);
        DF_spaceBetweenY = EditorGUILayout.FloatField("Space Between Grid Y", DF_spaceBetweenY);
        maxBlock = EditorGUILayout.IntField("Maximum amount of Blocks", maxBlock);
        minBlock = EditorGUILayout.IntField("Minimum amount of Blocks", minBlock);
    }
    #endregion
}
