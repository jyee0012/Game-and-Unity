using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvaderSpawner : MonoBehaviour
{

    GameObject invader;
    public float level = 1;
    public Text levelText;
    // Use this for initialization
    void Start()
    {
        invader = Resources.Load("Invader") as GameObject;
        SpawnInvaders(3, 7);
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Level: " + level;
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
                theInvader.GetComponent<InvaderScript>().level = this.level;
            }
        }
    }
}
