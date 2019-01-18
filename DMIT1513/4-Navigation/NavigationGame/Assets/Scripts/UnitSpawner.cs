﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour {

    [SerializeField]
    GameObject templateUnit = null;
    [SerializeField]
    Vector2 minBoundary = Vector2.one * 10, maxBoundary = Vector2.one * 10;
    [SerializeField]
    float spawnAmount = 1, spawnDelay = 3, spawnHeight = 1;
    [SerializeField]
    bool drawSpawnArea = false, spawnOnce = false;
    public bool canSpawn = true;
    [SerializeField]
    Text timerText;
    [SerializeField]
    string timerString = "Time Remaining";

    float timeStamp = 0, countdownTimeLeft;
    bool once = true;
	// Use this for initialization
	void Start () {
        timeStamp += spawnDelay;
        if (minBoundary == Vector2.zero) minBoundary = Vector2.one;
        if (maxBoundary == Vector2.zero) maxBoundary = Vector2.one;
        countdownTimeLeft = spawnDelay;
        HideText();
    }
	
	// Update is called once per frame
	void Update () {
        if (canSpawn)
        {
            countdownTimeLeft -= Time.deltaTime;
            UpdateText();
        }
        else
        {
            HideText();
        }
		if (countdownTimeLeft < 1 && canSpawn) // Time.time > timeStamp 
        {
            SpawnUnit();
            timeStamp = Time.time + spawnDelay;
            countdownTimeLeft = spawnDelay;
            if (spawnOnce) canSpawn = false;
        }
	}
    void HideText()
    {
        if (timerText != null) timerText.text = "";
    }
    void UpdateText()
    {
        string minSec = string.Format("{0}:{1:00}", (int)countdownTimeLeft / 60, (int)countdownTimeLeft % 60);
        if (timerText != null) timerText.text = timerString + ": " + minSec;
    }
    void SpawnUnit()
    {
        if (templateUnit == null || !canSpawn) return;

        for(int i = 0; i < spawnAmount; i++)
        {
            GameObject tempUnit = Instantiate(templateUnit, GetRandomSpawnLoc(minBoundary, maxBoundary), templateUnit.transform.rotation, null);
            tempUnit.SetActive(true);
        }

    }
    public static void StaticSpawnUnits(GameObject spawnUnit, Vector3 spawnPoint, float spawnAmount = 1, float verticalBoundary = 5, float horizontalBoundary = 5)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 min = new Vector2(0,0),
                max = new Vector2(0, 0);
            min.x += horizontalBoundary;
            min.y += verticalBoundary;
            max.x += horizontalBoundary;
            max.y += verticalBoundary;
            GameObject tempUnit = Instantiate(spawnUnit, GetRandomStaticSpawn(min, max, spawnPoint), spawnUnit.transform.rotation, null);
            //Debug.Log("Spawn: " + spawn + " | Min: " + min + " | Max: " + max + " | Click: " + spawnPoint);
            tempUnit.SetActive(true);
        }
    }
    static Vector3 GetRandomStaticSpawn(Vector2 min, Vector2 max, Vector3 spawnPos)
    {
        Vector3 spawnLoc = Vector3.zero;

        float randX = Random.Range(-min.x, max.x),
            randZ = Random.Range(-min.y, max.y),
            spawnY = spawnPos.y + 1,
            randXCord = spawnPos.x + randX,
            randZCord = spawnPos.z + randZ;


        spawnLoc = new Vector3(randXCord, spawnY, randZCord);

        return spawnLoc;
    }
    Vector3 GetRandomSpawnLoc(Vector2 min, Vector2 max)
    {
        Vector3 spawnLoc = Vector3.zero;

        float randX = Random.Range(-min.x, max.x),
            randZ = Random.Range(-min.y, max.y),
            spawnY = transform.position.y + spawnHeight,
            randXCord = transform.position.x + randX,
            randZCord = transform.position.z + randZ;


        spawnLoc = new Vector3(randXCord, spawnY, randZCord);

        return spawnLoc;
    }
    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (drawSpawnArea)
        {
            Vector3 spawnAreaSize = new Vector3(maxBoundary.x + minBoundary.x, 1, maxBoundary.y + minBoundary.y);
            Gizmos.DrawCube(transform.position, spawnAreaSize);
        }
    }
    #endregion
}