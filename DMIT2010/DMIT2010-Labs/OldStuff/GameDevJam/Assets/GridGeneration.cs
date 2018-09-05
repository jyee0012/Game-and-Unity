using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGeneration : EditorWindow
{
    static GameObject GameGrid;
    static float gridXsize = 5, gridYsize = 5, DF_spaceBetween = 0;
    #region Spawn Grid
    [MenuItem("Grid Generation/Spawn Grid", priority = 0)]
    static void SpawnGrid()
    {
        #region Spawn Variables
        GameGrid = Selection.transforms[0].gameObject;
        GameObject gamePrefab = Resources.Load<GameObject>("GameSpace") as GameObject;
        gamePrefab.gameObject.SetActive(true);
        Vector3 prefabBounds = gamePrefab.gameObject.GetComponent<BoxCollider>().size;
        Vector3 prefabExtents = prefabBounds / 2;
        Vector3 gridSpawn = GameGrid.transform.position + prefabExtents;
        int GS_count = 0;
        float spaceBetween = DF_spaceBetween + (prefabBounds.x * 2);
        #endregion
        ClearSpace();
        for (int i = 0; i < gridXsize; i++)
        {
            for (int c = 0; c < gridYsize; c++)
            {
                Vector3 location = gridSpawn + new Vector3((prefabExtents.x * spaceBetween) * i, (prefabExtents.y * spaceBetween) * c, 0);
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
    static void ClearSpace()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        for (int i = 0; i < allSpace.Length; i++)
        {
            DestroyImmediate(allSpace[i]);
        }
    }
    #endregion
    #region Randomize Grid
    [MenuItem("Grid Generation/Randomize Grid", priority = 0)]
    static void RandomizeGrid()
    {
        GameObject[] grid = GameObject.FindGameObjectsWithTag("Gamespace");
        int randSpawn, randGoal, randBlockAmount, blockCount = 0;
        randSpawn = Random.Range(0, grid.Length);
        randGoal = Random.Range(0, grid.Length);
        randBlockAmount = Random.Range(3, 7);
        int[] randBlock = new int[randBlockAmount];
        
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
                else if ((Random.Range(0, 2) % 2 == 0) && blockCount < randBlockAmount)
                {
                    spaceScript.thisSpace = GameSpaceScript.SpaceType.Block;
                    blockCount++;
                }
            }
        }
    }
    #endregion
    #region Clear Grid ID
    [MenuItem("Grid Generation/Clear Space ID", priority = 0)]
    static void ClearID()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        foreach (GameObject space in allSpace)
        {
            GameSpaceScript spaceScript = space.GetComponent<GameSpaceScript>();
            spaceScript.thisSpace = GameSpaceScript.SpaceType.Normal;
        }
    }
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
        DF_spaceBetween = EditorGUILayout.FloatField("Space Between Grid", DF_spaceBetween);
    }
    #endregion
}
