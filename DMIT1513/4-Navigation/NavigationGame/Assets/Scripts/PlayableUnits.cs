using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnits : MonoBehaviour {

    public GameObject target;
    public bool isSelected = false;
    ShootingScript shootScript = null;

	// Use this for initialization
	void Start () {
		if (shootScript == null)
        {
            if (GetComponent<ShootingScript>() != null)
            {
                shootScript = GetComponent<ShootingScript>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (CheckTarget() && shootScript != null)
        {
            shootScript.AIFire();
        }
	}
    bool CheckTarget()
    {
        bool hasTarget = false;
        if (target != null)
        {
            if (Physics.Linecast(transform.position, target.transform.position))
            {
                hasTarget = true;
            }
        }
        return hasTarget;
    }
}
