using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public bool outOfSpawns { get { return spawnCount >= spawnAmount && transform.childCount <= 0; } }

    [SerializeField]
    GameObject spawnedPrefab = null;
    [SerializeField]
    Vector2 spawnRange = Vector2.one;
    [SerializeField, Range(0,5)]
    float spawnDelay = 1;
    [SerializeField, Range(1,500)]
    int spawnAmount = 100;
    [SerializeField]
    bool canSpawn = true, endlessSpawn = false, drawRange = false;

    [Header("Enemy Info")]
    [SerializeField]
    Camera cameraController = null;
    public bool playerControlled = false;
    public bool hasDestination = false;
    public Vector3 enemyDestination = Vector3.zero;

    int spawnCount = 0;
    float spawnTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimeStamp = spawnDelay;
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
        if (endlessSpawn || spawnCount < spawnAmount)
        {
            Vector3 spawnPos = GetRandomSpawnPoint(spawnRange);
            GameObject spawned = Instantiate(spawnedPrefab, spawnPos, transform.rotation, transform);
            if(spawned.GetComponent<EnemyScript>() != null)
            {
                EnemyScript spawnedEnemy = spawned.GetComponent<EnemyScript>();
                spawnedEnemy.playerControlled = playerControlled;
                spawnedEnemy.cameraController = cameraController;
                if (hasDestination)
                {
                    spawnedEnemy.navAgent.SetDestination(enemyDestination);
                }
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
    private void OnDrawGizmos()
    {
        if (drawRange)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(transform.position, new Vector3(spawnRange.x, 1, spawnRange.y));
        }
    }
}
