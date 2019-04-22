using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public bool outOfSpawns { get { return spawnCount >= spawnAmount && transform.childCount <= 0; } }

    [SerializeField]
    GameObject spawnedPrefab = null;
    [SerializeField]
    Vector3 spawnRange = Vector3.one;
    [SerializeField, Range(0,5)]
    float spawnDelay = 1;
    [SerializeField]
    float startSpawnDelay = 0;
    [Range(1,20)]
    public int spawnAmount = 4;
    [Tooltip("Maximum amount of spawned allowed at a time")]
    [Range(1, 20)]
    public int maxSpawnAmount = 4;
    public bool canSpawn = true, endlessSpawn = false, drawRange = false;

    int spawnCount = 0;
    float spawnTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimeStamp = spawnDelay + startSpawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && spawnTimeStamp < Time.time)
        {
            SpawnPrefab();
        }
    }
    void SpawnPrefab()
    {
        if (spawnedPrefab == null) return;
        if (transform.childCount >= maxSpawnAmount) return;
        if (endlessSpawn || spawnCount < spawnAmount)
        {
            Vector3 spawnPos = GetRandomSpawnPoint(spawnRange);
            GameObject spawned = Instantiate(spawnedPrefab, spawnPos, transform.rotation, transform);
            if(spawned.GetComponent<PongBallScript>() != null)
            {
                PongBallScript spawnedBall = spawned.GetComponent<PongBallScript>();
            }
            spawnCount++;
            spawned.name = spawnedPrefab.name + " " + spawnCount;
            spawnTimeStamp = Time.time + spawnDelay;
        }
    }
    Vector3 GetRandomSpawnPoint(Vector2 range)
    {
        Vector3 spawnPoint = transform.position;
        float randX = Random.Range(0, range.x), randY = Random.Range(0, range.y);
        spawnPoint.x += randX;
        spawnPoint.z += randY;
        return spawnPoint;
    }
    Vector3 GetRandomSpawnPoint(Vector3 range)
    {
        Vector3 spawnPoint = transform.position;
        float randX = Random.Range(0, range.x), randY = Random.Range(0, range.y), randZ = Random.Range(0, range.z);
        spawnPoint.x += randX;
        spawnPoint.y += randY;
        spawnPoint.z += randZ;
        return spawnPoint;
    }
    private void OnDrawGizmos()
    {
        if (drawRange)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(transform.position, new Vector3(spawnRange.x, spawnRange.y, spawnRange.z));
        }
    }
}
