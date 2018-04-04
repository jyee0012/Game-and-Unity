﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    protected PrefabPool prefabPool;
    public Transform moveTowardsTarget;
    public int enemiesInWave;
	// Use this for initialization
	void Awake () {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
	}

    void Start()
    {
        SpawnWave();
    }
    // Update is called once per frame
    void Update () {

    }
    protected void SpawnWave()
    {
        Transform[] enemies = new Transform[enemiesInWave];
        for(int i = 0; i < enemiesInWave; i++)
        {
            enemies[i] = prefabPool.Enemy;
            //enemies[i].GetComponent<EnemyController>().moveTarget = moveTowardsTarget;
        }
        
        Vector3 centrePos = new Vector3(0, 0, 32);
        //place the enemies in a circle
        for (int pointNum = 0; pointNum < enemiesInWave; pointNum++)
        {
            float i = (pointNum * 1.0f) / enemiesInWave;
            // get the angle for this step (in radians, not degrees)
            float angle = i * Mathf.PI * 2;
            // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
            float x = Mathf.Sin(angle) * 10;
            float y = Mathf.Cos(angle) * 10;
            Vector3 pos = new Vector3(x, y, 0) + centrePos;
            // no need to assign the instance to a variable unless you're using it afterwards:
            enemies[pointNum].transform.position = pos;
        }
    }
}