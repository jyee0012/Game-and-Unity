using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderSpawner : MonoBehaviour
{

    GameObject invader;
    // Use this for initialization
    void Start()
    {
        invader = Resources.Load("Invader") as GameObject;
        SpawnInvaders(5, 9);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SpawnInvaders(5, 9);
        //}
    }
    public void SpawnInvaders(int row, int col)
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                // height of one invader = 0.2, default height spawn = 4, width of one invader = 1.25
                // x =  - ((col) * -1.25f) + c*1.25f?
                Vector3 spawnPos = new Vector3((0 - ((2/col) * -1.25f)) + (c * 1.25f), 4 - (0.2f*r), -5);
                GameObject theInvader = Instantiate(invader, spawnPos, Quaternion.identity, gameObject.transform);
            }
        }
    }
}
