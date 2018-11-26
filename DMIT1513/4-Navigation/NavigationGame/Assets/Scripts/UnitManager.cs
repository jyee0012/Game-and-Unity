using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public enum UnitType
    {
        None,
        Soldier,
        EnemySoldier,
        Heavy,
        Sniper,
        EnemyElite,
        Knight
    }
    public GameObject soldierPrefab, enemyPrefab, heavyPrefab, sniperPrefab, enemyElitePrefab, knightPrefab;
    public Material soldierMat, enemyMat, heavyMat, sniperMat, enemyEliteMat, knightMat;
    List<GameObject> unitList;

    [SerializeField]
    List<GameObject> targetList;
	// Use this for initialization
	void Start () {
        FindAllUnits();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void FindAllUnits()
    {
        if (targetList.Count > 0)
        {
            foreach(GameObject target in targetList)
            {
                GameObject unitObj = null;
                if (target.GetComponent<UnitTemplate>().gameObject != null) unitObj = target.GetComponent<UnitTemplate>().gameObject;
                if (target.GetComponentsInChildren<UnitTemplate>().Length > 0)
                {
                    foreach(UnitTemplate unit in target.GetComponentsInChildren<UnitTemplate>())
                    {
                        AddToUnitList(unit.gameObject);
                    }
                }

                if (unitObj != null) AddToUnitList(unitObj);
            }
        }
        else
        {
        }
    }
    void AddToUnitList(GameObject unit)
    {
        if (!unitList.Contains(unit))
        {
            unitList.Add(unit);
        }
    }
    void SpawnAllUnits()
    {
        foreach (GameObject unit in unitList)
        {
            SpawnSingleUnit(unit);
        }
    }
    void SpawnSingleUnit(GameObject unit)
    {
        UnitTemplate template = unit.GetComponent<UnitTemplate>();

        switch (template.unitType)
        {
            case UnitType.Soldier:
                CreateUnit(soldierPrefab, unit);
                break;
            case UnitType.EnemySoldier:
                CreateUnit(enemyPrefab, unit);
                break;
            case UnitType.Sniper:
                CreateUnit(sniperPrefab, unit);
                break;
            case UnitType.Heavy:
                CreateUnit(heavyPrefab, unit);
                break;
            case UnitType.EnemyElite:
                CreateUnit(enemyElitePrefab, unit);
                break;
            case UnitType.Knight:
                CreateUnit(knightPrefab, unit);
                break;
            default:
                Debug.Log(unit.name + " does not have Unit Type");
                break;
        }
    }
    void CreateUnit(GameObject unitPrefab, Vector3 spawnPos, Quaternion spawnRot, Transform spawnedParent = null)
    {
        GameObject spawned = Instantiate(unitPrefab, spawnPos, spawnRot, spawnedParent);
    }
    void CreateUnit (GameObject unitPrefab, Vector3 spawnPos)
    {
        CreateUnit(unitPrefab, spawnPos, Quaternion.identity);
    }
    void CreateUnit(GameObject unitPrefab, GameObject unitTemplate)
    {
        UnitTemplate template = unitTemplate.GetComponent<UnitTemplate>();
        if (unitPrefab == null)
        {
            Debug.Log("No " + template.unitType + " Prefab.");
            return;
        }
        CreateUnit(unitPrefab, unitTemplate.transform.position, unitTemplate.transform.rotation);
    }
}
