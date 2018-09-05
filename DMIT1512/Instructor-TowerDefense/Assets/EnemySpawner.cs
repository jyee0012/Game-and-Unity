using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    protected PrefabPool prefabPool;

    public Transform moveTowardsTarget;
    public int numEnemiesWave01;

	private void Awake ()
    {
        GameObject gameObjectFromScene = GameObject.Find("PrefabPool");
        prefabPool = gameObjectFromScene.GetComponent<PrefabPool>();
	}
    private void Start()
    {
        SpawnWave01();
    }

    protected void SpawnWave01()
    {
        Transform [] enemies = new Transform[numEnemiesWave01];
        for(int c = 0; c < numEnemiesWave01; c++)
        {
            enemies[c] = prefabPool.Enemy;
            enemies[c].GetComponent<EnemyController>().target = moveTowardsTarget;
        }

        Vector3 centrePos = new Vector3(0, 0, 32);
        //place the enemies in a circle
        for (int pointNum = 0; pointNum < numEnemiesWave01; pointNum++)
        {
            float i = (pointNum * 1.0f) / numEnemiesWave01;
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
