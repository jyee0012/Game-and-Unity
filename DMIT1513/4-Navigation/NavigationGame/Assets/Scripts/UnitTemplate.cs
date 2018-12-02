using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitTemplate : MonoBehaviour
{

    public UnitManager.UnitType unitType = UnitManager.UnitType.None;
    [SerializeField]
    Material currentMat;
    [SerializeField]
    GameObject manager = null, currentUnit = null;

    public bool canSpawn = false, personalFunction = true;
    public List<Vector2> patrolPoints;

    UnitManager managerScript = null;
    // Use this for initialization
    void Start()
    {
        if (personalFunction)
        {
            if (manager == null) manager = GameObject.Find("UnitManager");
            if (manager != null)
            {
                managerScript = manager.GetComponent<UnitManager>();
            }
            if (unitType != UnitManager.UnitType.None && managerScript != null)
            {
                currentMat = managerScript.GetMaterial(unitType);
                currentUnit = managerScript.GetPrefab(unitType);
            }

            ChangeMaterial(currentMat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnUnit(currentUnit, canSpawn);
    }
    void ChangeMaterial(Material changeMat = null)
    {
        if (changeMat != null)
        {
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material = currentMat;
            }
        }
    }
    void SpawnUnit(GameObject unit, bool spawn = true)
    {
        if (unit == null || !spawn) return;

        Instantiate(unit, transform.position, unit.transform.rotation);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        switch (unitType)
        {
            case UnitManager.UnitType.Soldier:
                Gizmos.color = Color.white;
                break;
            case UnitManager.UnitType.EnemySoldier:
                Gizmos.color = Color.red;
                break;
            case UnitManager.UnitType.Sniper:
                Gizmos.color = Color.yellow;
                break;
            case UnitManager.UnitType.Heavy:
                Gizmos.color = Color.grey;
                break;
            case UnitManager.UnitType.EnemyElite:
                Gizmos.color = Color.magenta;
                break;
            case UnitManager.UnitType.Knight:
                Gizmos.color = Color.green;
                break;
        }
        Gizmos.DrawCube(transform.position, transform.localScale * 1.5f);
    }
}
