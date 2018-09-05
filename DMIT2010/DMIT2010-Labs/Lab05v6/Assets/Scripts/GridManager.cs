using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Variables
    public GameObject GameGrid;
    public float gridXsize = 20, gridYsize = 20, DF_spaceBetweenX = 0f, DF_spaceBetweenY = 0f, spaceBetweenX, spaceBetweenY;
    // max is exclusive
    public int minRoom = 30, // Always less than maxBlock by at least 1
                maxRoom = 150, // Always less than (gridX * gridY) / 2
                safeRooms = 3;
    #endregion
    // Use this for initialization
    void Start()
    {
        //Begin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeGrid();
        }
    }
    #region Spawn Grid
    public void SpawnGrid()
    {
        #region Spawn Variables
        GameGrid = this.gameObject;
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
    #endregion
    #region Clear Grid
    public void ClearSpace()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Block");
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
        GameObject[] grid = GameObject.FindGameObjectsWithTag("Block");
        int randLootAmount = 0, randMobAmount = 0, randRestAmount, restCount = 0, mobCount = 0, lootCount = 0;
        int[] randLoot = new int[maxRoom], randMob = new int[maxRoom], randRest = new int[maxRoom];
        #endregion
        #region Randoms
        randLootAmount = Random.Range(minRoom, maxRoom + 1);
        randMobAmount = Random.Range(minRoom, maxRoom + 1);
        randRestAmount = safeRooms;
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
    public void ClearID()
    {
        GameObject[] allSpace = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject space in allSpace)
        {
            SpaceBlockScript spaceScript = space.GetComponent<SpaceBlockScript>();
            spaceScript.thisBlock = SpaceBlockScript.BlockID.Empty;
        }
    }
    #endregion
    #region Connections
    #region Create Connections
    public void CreateConnections()
    {
        #region Variables
        Vector3 prefabBounds = Resources.Load<GameObject>("Block").gameObject.GetComponent<BoxCollider>().size;
        //  divide by 4.5f only when in actual gamespace, change back when testing
        Vector3 prefabExtents = prefabBounds / 2;
        spaceBetweenX = DF_spaceBetweenX + (prefabBounds.x * 2);

        GameObject[] spaces = GameObject.FindGameObjectsWithTag("Block");
        #endregion
        ClearConnections();
        for (int i = 0; i < spaces.Length; i++)
        {
            for (int c = 0; c < spaces.Length; c++)
            {
                if (Vector3.Distance(spaces[i].transform.position, spaces[c].transform.position) <= (1f) && spaces[i] != spaces[c])
                {
                    //Add Connections
                    spaces[i].GetComponent<SpaceBlockScript>().AddConnections(spaces[c]);
                    //EditorUtility.SetDirty(spaces[i].GetComponent<SpaceBlockScript>());
                }
            }
        }
    }
    #endregion
    #region Clear Connections
    public void ClearConnections()
    {
        foreach (GameObject space in GameObject.FindGameObjectsWithTag("Block"))
        {
            space.GetComponent<SpaceBlockScript>().ClearConnections();
        }
    }
    #endregion
    #endregion
    public void Begin()
    {
        SpawnGrid();
        RandomizeGrid();
        CreateConnections();
    }
}
