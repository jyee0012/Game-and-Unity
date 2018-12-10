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
    int enemyTotal = 0, enemyAlive = 0, unitAlive = 0, controllerDelay = 20;
    [SerializeField]
    bool win = false, lose = false, universalSleep = false;
    [SerializeField]
    List<GameObject> unitControlled = new List<GameObject>();

    bool unitSleeping = false;
	// Use this for initialization
	void Start () {
        if (findEnemies) FindAllEnemies();
        if (universalSleep != unitSleeping)
        {
            PutAllUnitsToSleep(universalSleep);
        }

        //Debug.Log(enemyTotal + "*");
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerDelay > Time.time) return;
        FindAllEnemies();
        enemyAlive = CheckAllEnemy();
        lose = CheckAllUnits();
        UpdateText();

        if (enemyAlive < 1) win = true;

        if (win || lose)
        {
            int endIndex = (win) ? 2 : 3;
            //Debug.Log(endIndex);
            if (win)
                RegionManager.Instance.Conqure();
            if (lose)
                RegionManager.Instance.Retreat();
            //SceneManager.LoadScene(endIndex);
        }
    }
    void PutAllUnitsToSleep(bool uniSleep)
    {
        foreach (GameObject unitController in unitControlled)
        {
            foreach(PlayableUnits unit in unitController.GetComponentsInChildren<PlayableUnits>())
            {
                unit.asleep = uniSleep;
            }
            foreach(UnitSpawner spawner in unitController.GetComponentsInChildren<UnitSpawner>())
            {
                spawner.canSpawn = !uniSleep;
            }
        }
        unitSleeping = uniSleep;
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
        //ClearEnemyList();
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
    void ClearEnemyList()
    {
        allEnemies.Clear();
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
