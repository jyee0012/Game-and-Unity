using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    protected PrefabPool prefabPool;
    public Transform moveTowardsTarget;
    public int waveNum = 0, wave1, wave2, currentWave;
    public Text waveText;
	// Use this for initialization
	void Awake () {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
	}

    void Start()
    {
        ClearEnemies();
        currentWave = wave1;
        SpawnWave(currentWave, 20);
    }
    // Update is called once per frame
    void Update () {
        waveText.text = "Wave " + waveNum + ": " + CountActiveEnemy() + "/" + currentWave;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            ClearEnemies();
            currentWave = wave2;
            SpawnWave(currentWave, Random.Range(10, 60));
        }
        if(waveNum == 3)
        {
            WinGame();
        }
    }
    protected void SpawnWave(int enemiesInWave, int distance)
    {
        waveNum++;
        prefabPool.waveNum = waveNum;
        Transform[] enemies = new Transform[enemiesInWave];
        for(int i = 0; i < enemiesInWave; i++)
        {
            enemies[i] = prefabPool.Enemy;
            enemies[i].GetComponent<EnemyController>().target = moveTowardsTarget;
        }
        
        Vector3 centrePos = new Vector3(0, 0, 32);
        //place the enemies in a circle
        for (int pointNum = 0; pointNum < enemiesInWave; pointNum++)
        {
            float i = (pointNum * 1.0f) / enemiesInWave;
            // get the angle for this step (in radians, not degrees)
            float angle = i * Mathf.PI * 2;
            // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
            float x = Mathf.Sin(angle) * distance;
            float y = Mathf.Cos(angle) * distance;
            Vector3 pos = new Vector3(x, y, 0) + centrePos;
            // no need to assign the instance to a variable unless you're using it afterwards:
            enemies[pointNum].transform.position = pos;
        }
    }
    public static int CountActiveEnemy()
    {
        int count = 0;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
    public void WinGame()
    {
        GameObject.Find("PrefabPool").GetComponent<PrefabPool>().win = true;
        GameObject.Find("PrefabPool").GetComponent<PrefabPool>().DisableEverything();
        GameManager.SLoadGameSpace(2);
    }
    public void ClearEnemies()
    {

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.activeInHierarchy)
            {
                enemy.SetActive(false);
            }
        }
    }
}
