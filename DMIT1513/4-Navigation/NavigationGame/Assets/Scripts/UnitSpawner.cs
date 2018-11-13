using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour {

    [SerializeField]
    GameObject templateUnit = null;
    [SerializeField]
    Vector2 minBoundary = Vector2.one * 10, maxBoundary = Vector2.one * 10;
    [SerializeField]
    float spawnAmount = 1, spawnDelay = 3, spawnHeight = 1;
    [SerializeField]
    bool drawSpawnArea = false;
    public bool canSpawn = true;
    float timeStamp = 0;
	// Use this for initialization
	void Start () {
        timeStamp += spawnDelay;
        if (minBoundary == Vector2.zero) minBoundary = Vector2.one;
        if (maxBoundary == Vector2.zero) maxBoundary = Vector2.one;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeStamp && canSpawn)
        {
            SpawnUnit();
            timeStamp = Time.time + spawnDelay;
        }
	}
    void SpawnUnit()
    {
        if (templateUnit == null || !canSpawn) return;

        for(int i = 0; i < spawnAmount; i++)
        {
            Instantiate(templateUnit, GetRandomSpawnLoc(minBoundary, maxBoundary), templateUnit.transform.rotation, null);
        }

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
