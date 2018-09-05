using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvaderSpawner : MonoBehaviour
{

    GameObject invader, newVader;
    public float level = 1;
    public Text levelText;
    float uniqueTimer = 15;
    // Use this for initialization
    void Start()
    {
        invader = Resources.Load("Invader") as GameObject;
        newVader = Resources.Load("UFO") as GameObject;
        SpawnInvaders(3, 7);
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Level: " + level;
        if (uniqueTimer < Time.time)
        {
            uniqueTimer = Time.time + 20f;
            SpawnNewInvader();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnInvaders(3, 7);
        }
    }
    public void SpawnInvaders(int row, int col)
    {
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                // height of one invader = 0.2, default height spawn = 4, width of one invader = 1.25
                // x =  - ((col) * -1.25f) + c*1.25f?
                Vector3 spawnPos = new Vector3((0 - ((2/col) * -1.25f)) + (c * 1.25f), 3 - (0.2f*r), -5);
                GameObject theInvader = Instantiate(invader, spawnPos, Quaternion.identity, gameObject.transform);
                theInvader.GetComponent<InvaderScript>().level = this.level;
            }
        }
    }
    public void SpawnNewInvader()
    {
        GameObject newInvader = Instantiate(newVader, new Vector3(-8, 4.7f,-5), Quaternion.identity, gameObject.transform);
    }
}
