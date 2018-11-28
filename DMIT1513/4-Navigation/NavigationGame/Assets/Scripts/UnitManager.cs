using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo
{
    public GameObject prefab;
    public Material material;
}

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
    // [Header("Unit Types")]
    public UnitInfo soldier, enemy, heavy, sniper, enemyElite, knight;
    List<GameObject> unitList;

    [Space]
    [Space]
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
                CreateUnit(soldier.prefab, unit);
                break;
            case UnitType.EnemySoldier:
                CreateUnit(enemy.prefab, unit);
                break;
            case UnitType.Sniper:
                CreateUnit(sniper.prefab, unit);
                break;
            case UnitType.Heavy:
                CreateUnit(heavy.prefab, unit);
                break;
            case UnitType.EnemyElite:
                CreateUnit(enemyElite.prefab, unit);
                break;
            case UnitType.Knight:
                CreateUnit(knight.prefab, unit);
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
    public GameObject GetPrefab(UnitType type)
    {
        return GetUnit(type).prefab;
    }
    public Material GetMaterial(UnitType type)
    {
        return GetUnit(type).material;
    }
    UnitInfo GetUnit (UnitType type)
    {
        UnitInfo foundInfo = null;
        switch (type)
        {
            case UnitType.None:
                break;
            case UnitType.Soldier:
                foundInfo = soldier;
                break;
            case UnitType.EnemySoldier:
                foundInfo = enemy;
                break;
            case UnitType.Sniper:
                foundInfo = sniper;
                break;
            case UnitType.Heavy:
                foundInfo = heavy;
                break;
            case UnitType.EnemyElite:
                foundInfo = enemyElite;
                break;
            case UnitType.Knight:
                foundInfo = knight;
                break;
            default:
                break;
        }
        return foundInfo;
    }
}
