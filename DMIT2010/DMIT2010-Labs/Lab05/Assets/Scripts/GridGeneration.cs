using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGeneration : EditorWindow
{
    static GameObject GameGrid;
    static float gridXsize = 7, gridYsize = 7, DF_spaceBetweenX = 0.5f, DF_spaceBetweenY = 0.5f, spaceBetweenX, spaceBetweenY;
    // max is exclusive
    static int minBlock = 3, // Always less than maxBlock by at least 1
                maxBlock = 7; // Always less than (gridX * gridY) / 2
    #region Spawn Grid
    [MenuItem("Grid Generation/Spawn Grid", priority = 0)]
    public static void SpawnGrid()
    {
        #region Spawn Variables
        GameGrid = Selection.transforms[0].gameObject;
        GameObject gamePrefab = Resources.Load<GameObject>("Block") as GameObject;
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
                Vector3 location = gridSpawn + new Vector3((prefabExtents.x * spaceBetweenX) * i, 0, (prefabExtents.y * spaceBetweenY) * c);
                GameObject spaceInstance = Instantiate(gamePrefab, location, GameGrid.transform.rotation, GameGrid.transform) as GameObject;
                spaceInstance.name = "Block " + GS_count;
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
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Block");
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
        GameObject[] grid = GameObject.FindGameObjectsWithTag("Block");
        int randLootAmount = 0, randMobAmount = 0, randRestAmount, restCount = 0, mobCount = 0, lootCount = 0;
        int[] randLoot = new int[maxBlock], randMob = new int[maxBlock], randRest = new int[maxBlock];
        #endregion
        #region Randoms
        randLootAmount = Random.Range(minBlock, maxBlock + 1);
        randMobAmount = Random.Range(minBlock, maxBlock + 1);
        randRestAmount = Random.Range(minBlock, maxBlock + 1);
        for (int i = 0; i < randRest.Length; i++)
        {
            randRest[i] = Random.Range(0, grid.Length);
            //while (randRest[i] == randSpawn && randBlock[i] == randGoal)
            //{
            //    randRest[i] = Random.Range(0, grid.Length);
            //}
        }
        for (int i = 0; i < randLoot.Length; i++)
        {
            randLoot[i] = Random.Range(0, grid.Length);
        }
        for (int i = 0; i < randMob.Length; i++)
        {
            randMob[i] = Random.Range(0, grid.Length);
        }
        #endregion

        ClearID();
        for (int i = 0; i < grid.Length - 1; i++)
        {

            SpaceBlockScript spaceScript = grid[i].GetComponent<SpaceBlockScript>();
            if (spaceScript.thisBlock == SpaceBlockScript.BlockID.Empty)
            {
                for (int c = 0; c < randRest.Length; c++)
                {
                    if (i == randRest[c])
                    {
                        grid[i].GetComponent<SpaceBlockScript>().thisBlock = SpaceBlockScript.BlockID.SafeRoom;
                        restCount++;
                    }
                }
                for (int c = 0; c < randMob.Length; c++)
                {
                    if (i == randMob[c])
                    {
                        grid[i].GetComponent<SpaceBlockScript>().thisBlock = SpaceBlockScript.BlockID.Monster;
                        mobCount++;
                    }
                }
                for (int c = 0; c < randLoot.Length; c++)
                {
                    if (i == randLoot[c])
                    {
                        grid[i].GetComponent<SpaceBlockScript>().thisBlock = SpaceBlockScript.BlockID.Loot;
                        lootCount++;
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
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject space in allSpace)
        {
            SpaceBlockScript spaceScript = space.GetComponent<SpaceBlockScript>();
            spaceScript.thisBlock = SpaceBlockScript.BlockID.Empty;
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
        DF_spaceBetweenX = EditorGUILayout.FloatField("Space Between Grid X", DF_spaceBetweenX);
        DF_spaceBetweenY = EditorGUILayout.FloatField("Space Between Grid Y", DF_spaceBetweenY);
        maxBlock = EditorGUILayout.IntField("Maximum amount of Blocks", maxBlock);
        minBlock = EditorGUILayout.IntField("Minimum amount of Blocks", minBlock);
    }
    #endregion
}
