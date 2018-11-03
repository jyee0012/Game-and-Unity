using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyControllerScript : MonoBehaviour {

    public List<GameObject> allEnemies = new List<GameObject>();
    [SerializeField]
    bool findEnemies = true;
    [SerializeField]
    Text enemyText;
    [SerializeField]
    int enemyTotal = 0, enemyAlive = 0, unitAlive = 0;
    [SerializeField]
    bool win = false, lose = false;
	// Use this for initialization
	void Start () {
        if (findEnemies) FindAllEnemies();


        //Debug.Log(enemyTotal + "*");
	}
	
	// Update is called once per frame
	void Update () {
        enemyAlive = CheckAllEnemy();
        lose = CheckAllUnits();
        UpdateText();

        if (enemyAlive < 1) win = true;

        if (win || lose)
        {
            int endIndex = (win) ? 2 : 3;
            //Debug.Log(endIndex);
            SceneManager.LoadScene(endIndex);
        }
    }
    void UpdateText()
    {
        if (enemyText == null) return;
        enemyText.text = "Enemies: " + CheckAllEnemy() + "/" + enemyTotal;
    }
    bool CheckEnemy(GameObject enemy)
    {
        return enemy.activeInHierarchy;
    }
    bool CheckEnemy(int enemyIndex)
    {
        return allEnemies[enemyIndex].activeInHierarchy;
    }
    int CheckAllEnemy()
    {
        int enemyCount = 0;
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy.activeInHierarchy) enemyCount++;
        }
        return enemyCount;
    }
    void FindAllEnemies()
    {
        GameObject[] allFoundEnemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach (GameObject enemy in allFoundEnemies)
        {
            AddEnemy(enemy);
        }
        enemyTotal = allEnemies.Count;
    }
    void AddEnemy(GameObject enemy)
    {
        GameObject enemyRoot = enemy.transform.root.gameObject;

        if (allEnemies.Contains(enemyRoot)) return;
        else allEnemies.Add(enemyRoot);
    }
    bool CheckAllUnits()
    {
        bool lost = false;
        int unitCount = 0;
        GameObject[] allFoundUnits = GameObject.FindGameObjectsWithTag("Moveable");
        foreach (GameObject unit in allFoundUnits)
        {
            if (unit.activeInHierarchy)
                unitCount++;
        }
        if (unitCount < 1) lost = true;
        //Debug.Log("units: "+ unitCount);
        unitAlive = unitCount;
        return lost;
    }
}
