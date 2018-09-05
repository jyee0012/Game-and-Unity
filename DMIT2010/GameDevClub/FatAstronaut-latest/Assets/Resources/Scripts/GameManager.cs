using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameObject GameGrid;
    float gridXsize = 3, gridYsize = 3, DF_spaceBetweenX = 5f, DF_spaceBetweenY = 5f, spaceBetweenX, spaceBetweenY;
    // max is exclusive
    int minBlock = 0, // Always less than maxBlock by at least 1
                maxBlock = 1, // Always less than (gridX * gridY) / 2
        level;
    bool goalExists = false, spawnExists = false;
    // Use this for initialization
    void Start()
    {
        do
        {
            DoEverything();
        } while (!Requirements());

        SpawnEnemies();
        SpawnHealthPacks();
        SpawnCheese();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SpawnPlayer();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public bool Requirements()
    {
        goalExists = false;
        spawnExists = false;

        foreach (GameObject space in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            if (space.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Goal)
            {
                goalExists = true;
            }
            if (space.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Spawn)
            {
                spawnExists = true;
            }
        }
        return goalExists && spawnExists;
    }
    public void LevelUp()
    {
        level++;
        if (level % 3 == 0)
        {
            gridXsize++;
            gridYsize++;
            maxBlock++;
            //minBlock++;
        }
    }
    #region Do Everything
    public void DoEverything()
    {
        SpawnGrid();
        RandomizeGrid();
        CreateConnections();
        //CreateVicPath();
    }
    #endregion
    #region Spawn Grid
    public void SpawnGrid()
    {
        #region Spawn Variables
        GameGrid = this.gameObject;
        GameObject gamePrefab = Resources.Load<GameObject>("Prefabs/GameSpace") as GameObject;
        gamePrefab.gameObject.SetActive(true);
        //Resources.Load<GameObject>("Gamespace").gameObject.GetComponent<BoxCollider>().size; stupid code
        Vector3 prefabBounds = gamePrefab.gameObject.GetComponent<BoxCollider>().size / 4.5f; // only when in actual gamespace, change back when testing
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
    #endregion
    #region Clear Grid
    public void ClearSpace()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Gamespace");
        for (int i = 0; i < allSpace.Length; i++)
        {
            DestroyImmediate(allSpace[i]);
        }
    }
    #endregion
    #region Randomize Grid
    public void RandomizeGrid()
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

        Debug.Log("REsutks Goal: " + randGoal + "\n Spawn: " + randSpawn + "\n Block: " + randBlock.ToString());

        if (randSpawn == grid.Length)
        {
            if (randGoal >= grid.Length / 2)
            {
                randSpawn = 0;
            }
            else
            {
                randSpawn--;
            }
        }
        if (randGoal == grid.Length)
        {
            if (randSpawn >= grid.Length / 2)
            {
                randGoal = 0;
            }
            else
            {
                randGoal--;
            }
        }

        ClearID();
        for (int i = 0; i < grid.Length - 1; i++)
        {
            GameSpaceScript spaceScript = grid[i].GetComponent<GameSpaceScript>();
            if (spaceScript.thisSpace == GameSpaceScript.SpaceType.Normal)
            {
                if (i == randSpawn)
                {
                    Debug.Log("Set Spawn: " + randSpawn);
                    spaceScript.thisSpace = GameSpaceScript.SpaceType.Spawn;
                }
                else if (i == randGoal)
                {
                    Debug.Log("Set Goal: " + randGoal);
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
    public void ClearID()
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
    public void CreateConnections()
    {
        #region Variables
        Vector3 prefabBounds = Resources.Load<GameObject>("Prefabs/Gamespace").gameObject.GetComponent<BoxCollider>().size;
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
                if (Vector3.Distance(spaces[i].transform.position, spaces[c].transform.position) <= ((spaceBetweenX / 2) + 0.1f) && spaces[i] != spaces[c])
                {
                    //Add Connections
                    spaces[i].GetComponent<GameSpaceScript>().AddConnections(spaces[c]);
                    //EditorUtility.SetDirty(spaces[i].GetComponent<GameSpaceScript>());
                }
            }
        }
    }
    #endregion
    #region Clear Connections
    public void ClearConnections()
    {
        foreach (GameObject space in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            space.GetComponent<GameSpaceScript>().ClearConnections();
        }
    }
    #endregion
    #region VictoryPath
    // Create a menuitem that will generate a path/shortest path from spawn to goal.
    public void CreateVicPath()
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

    }
    #endregion
    #endregion
    #region Settings
    //[MenuItem("Grid Generation/Spawn Settings", priority = 2)]
    // void Init()
    //{
    //    GridGeneration SpawnSettings = (GridGeneration)EditorWindow.GetWindow(typeof(GridGeneration), true, "Spawn Settings");
    //    SpawnSettings.Show();
    //}
    //void OnGUI()
    //{
    //    GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
    //    gridXsize = EditorGUILayout.FloatField("Grid X Size", gridXsize);
    //    gridYsize = EditorGUILayout.FloatField("Grid Y Size", gridYsize);
    //    DF_spaceBetweenX = EditorGUILayout.FloatField("Space Between Grid X", DF_spaceBetweenX);
    //    DF_spaceBetweenY = EditorGUILayout.FloatField("Space Between Grid Y", DF_spaceBetweenY);
    //    maxBlock = EditorGUILayout.IntField("Maximum amount of Blocks", maxBlock);
    //    minBlock = EditorGUILayout.IntField("Minimum amount of Blocks", minBlock);
    //}
    #endregion

    public void SpawnEnemies()
    {
        List<GameObject> gridspaces = new List<GameObject>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            if (obj.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Normal)
                gridspaces.Add(obj);
        }

        int numEnemies = Random.Range(1, gridspaces.Count);
        int[] enemies = new int[numEnemies]; //enemies location (spawn) list

        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = Random.Range(0, gridspaces.Count);
        }

        for (int n = 0; n < enemies.Length; n++)
        {
            for (int i = 0; i < gridspaces.Count; i++)
            {
                //if the gridspace number is equal to the gridspace numbe in the enemies spawn list
                if (i == enemies[n])
                {
                    //create new enemy
                    GameObject enemy = (GameObject)Instantiate(Resources.Load("Prefabs/BugEnemy"));

                    if (enemy.GetComponent<Rigidbody>() != null)
                        enemy.AddComponent<Rigidbody>();

                    enemy.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                    enemy.transform.position = gridspaces[i].transform.position;
                    enemy.name = "Enemy " + n;
                }
            }
        }
    }

    public void SpawnHealthPacks()
    {
        List<GameObject> gridspaces = new List<GameObject>();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            if (obj.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Normal)
                gridspaces.Add(obj);
        }

        int numEnemies = Random.Range(1, gridspaces.Count);
        int[] enemies = new int[numEnemies]; //enemies location (spawn) list

        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = Random.Range(0, gridspaces.Count);
        }

        for (int n = 0; n < enemies.Length; n++)
        {
            for (int i = 0; i < gridspaces.Count; i++)
            {
                //if the gridspace number is equal to the gridspace numbe in the enemies spawn list
                if (i == enemies[n])
                {
                    //create new enemy
                    GameObject enemy = (GameObject)Instantiate(Resources.Load("Prefabs/HealthPack"));

                    enemy.transform.position = gridspaces[i].transform.position + new Vector3(0, -4, 0);
                    enemy.name = "HealthPack " + n;
                }
            }
        }
    }

    public void SpawnCheese()
    {
        GameObject[] spaces = GameObject.FindGameObjectsWithTag("Gamespace");

        for (int i = 0; i < spaces.Length; i++)
        {
            GameObject space = spaces[i];

            if (space.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Goal)
            {
                GameObject bigCheese = (GameObject)Instantiate(Resources.Load("Prefabs/Cheese"));

                bigCheese.transform.position = space.transform.position + new Vector3(0, 4, 0);
            }
        }

        
    }
}

