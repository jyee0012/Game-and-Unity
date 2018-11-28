using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitTemplate : MonoBehaviour {
    
    public UnitManager.UnitType unitType = UnitManager.UnitType.None;
    [SerializeField]
    Material currentMat;
    [SerializeField]
    GameObject manager = null;

    UnitManager managerScript = null;
	// Use this for initialization
	void Start () {
        if (manager == null) manager = GameObject.Find("UnitManager");
        if (manager != null)
        {
            managerScript = manager.GetComponent<UnitManager>();
        }
        if (unitType != UnitManager.UnitType.None && managerScript != null)
        {
            currentMat = managerScript.GetMaterial(unitType);
        }

        ChangeMaterial(currentMat);
    }
	
	// Update is called once per frame
	void Update () {
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
}
