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
    public List<GameObject> unitList;

    [Space]
    [Space]
    [SerializeField]
    List<GameObject> targetList;
    [SerializeField]
    UnitTemplate[] foundTemplates;
    
    [SerializeField]
    bool universalCanSpawn = false, useUnit = false, useTemplate = false;
	// Use this for initialization
	void Start () {
        if (useUnit)
        {
            FindAllUnits();
            SpawnAllUnits();
        }
        if (useTemplate)
        {
            FindAllTemplates();
            SetAllTemplatesSpawn(universalCanSpawn);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    #region Unit Functions (For manager to fully control what the templates do)
    void FindAllUnits()
    {
        if (targetList.Count > 0)
        {
            foreach (GameObject target in targetList)
            {
                GameObject unitObj = null;
                if (target.GetComponent<UnitTemplate>().gameObject != null) unitObj = target.GetComponent<UnitTemplate>().gameObject;
                if (target.GetComponentsInChildren<UnitTemplate>().Length > 0)
                {
                    foreach (UnitTemplate unit in target.GetComponentsInChildren<UnitTemplate>())
                    {
                        AddToUnitList(unit.gameObject);
                    }
                }

                if (unitObj != null) AddToUnitList(unitObj);
            }
        }
        else
        {
            UnitTemplate[] templateArray = FindObjectsOfType<UnitTemplate>();
            foundTemplates = templateArray;
            if (templateArray.Length > 0) AddToUnitList(templateArray);
        }
    }
    void AddToUnitList(GameObject unit)
    {
        if (!unitList.Contains(unit))
        {
            unitList.Add(unit);
        }
    }
    void AddToUnitList(UnitTemplate[] array)
    {
        foreach(UnitTemplate temp in array)
        {
            AddToUnitList(temp.gameObject);
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

        UnitInfo currentUnit = GetUnit(template.unitType);
        if (currentUnit == null)
        {
            Debug.Log(unit.name + " does not have Unit Type");
            return;
        }
        CreateUnit(currentUnit.prefab, unit);
        Destroy(unit);
    }
    GameObject CreateUnit(GameObject unitPrefab, Vector3 spawnPos, Quaternion spawnRot, Transform spawnedParent = null)
    {
        GameObject spawned = Instantiate(unitPrefab, spawnPos, spawnRot, spawnedParent);
        return spawned;
    }
    GameObject CreateUnit(GameObject unitPrefab, Vector3 spawnPos)
    {
        return CreateUnit(unitPrefab, spawnPos, Quaternion.identity);
    }
    GameObject CreateUnit(GameObject unitPrefab, GameObject unitTemplate)
    {
        UnitTemplate template = unitTemplate.GetComponent<UnitTemplate>();
        if (unitPrefab == null)
        {
            Debug.Log("No " + template.unitType + " Prefab.");
            return null;
        }
        GameObject spawned = CreateUnit(unitPrefab, unitTemplate.transform.position, unitTemplate.transform.rotation);
        PlayableUnits spawnedUnitScript = spawned.GetComponent<PlayableUnits>();
        if (template.patrolPoints.Count > 0)
        {
            spawnedUnitScript.SetPatrolPointList(template.patrolPoints);
        }
        return spawned;
    }
    #endregion

    #region Template Functions (For controlling all templates)
    void FindAllTemplates()
    {
        foreach(GameObject target in targetList)
        {
            foreach (UnitTemplate targetTemplate in target.GetComponents<UnitTemplate>())
            {
                targetList.Add(targetTemplate.gameObject);
            }
            foreach (UnitTemplate targetTemplate in target.GetComponentsInChildren<UnitTemplate>())
            {
                targetList.Add(targetTemplate.gameObject);
            }
        }
    }
    void SetAllTemplatesSpawn(bool bSpawn)
    {
        foreach (GameObject template in targetList)
        {
            UnitTemplate tempScript = template.GetComponent<UnitTemplate>();
            tempScript.canSpawn = bSpawn;
        }
    }
    void AddUniqueTemplateToList(GameObject template)
    {
        if (template == null) return;
        if (!targetList.Contains(template))
        {
            targetList.Add(template);
        }
    }
    #endregion
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
    private void OnDrawGizmos()
    {
    }
}
