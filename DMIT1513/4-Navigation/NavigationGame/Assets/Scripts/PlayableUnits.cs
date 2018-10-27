using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableUnits : MonoBehaviour {

    public GameObject target, shootingBase;
    public bool isSelected = false;
    ShootingScript shootScript = null;
    NavMeshAgent navAgent;
    [SerializeField]
    float shootDist = 5, detectRad=5;
    [SerializeField]
    bool lockOnTarget = false, // once target != null, set destination to target and eliminate 
        canInterrupt = false; // if shooting @ target, if true then can move and ignore target

    bool firstEncounter = true;
    // Use this for initialization
    void Start () {
		if (shootScript == null)
        {
            if (GetComponent<ShootingScript>() != null)
            {
                shootScript = GetComponent<ShootingScript>();
            }
        }
        if (navAgent == null)
        {
            if (GetComponent<NavMeshAgent>() != null)
            {
                navAgent = GetComponent<NavMeshAgent>();
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CheckTarget(target, "hasTarget"))
        {
            if (CheckTarget(target))
            {
                if (TargetWithinRange(shootDist))
                {
                    if (!canInterrupt || firstEncounter)
                    {
                        //navAgent.isStopped = true;
                        firstEncounter = false;
                    }
                    ShooterBaseLookAt(target);
                    if (shootScript != null)
                    {
                        //Debug.Log(gameObject.name + ": Ai has shot");
                        shootScript.AIFire();
                    }
                }
            }
        }
	}
    bool CheckTarget(GameObject currentTarget, string checkText = "")
    {
        bool hasTarget = false;
        if (currentTarget != null)
        {
            if (checkText == "hasTarget")
            {
                return true;
            }
            if (Physics.Linecast(transform.position, currentTarget.transform.position))
            {
                hasTarget = true;
            }
        }
        return hasTarget;
    }
    GameObject EnemyDetection()
    {
        GameObject newTarget = null;
        Collider[] detectionSphere = Physics.OverlapSphere(transform.position, detectRad);
        float currentTargetDist = Vector3.Distance(transform.position, detectionSphere[0].transform.position);
        foreach (Collider detected in detectionSphere)
        {
            float newDist = Vector3.Distance(transform.position, detected.transform.position);
            bool shorterDist = newDist < currentTargetDist;
            if (detected.tag == "EnemyUnit" && shorterDist)
            {
                newTarget = detected.gameObject;
                currentTargetDist = newDist;
            }
        }
        return newTarget;
    }
    bool TargetWithinRange(float range = 5)
    {
        bool withinRange = false;
        if (target != null)
        {
            withinRange = Vector3.Distance(transform.position, target.transform.position) <= range;
        }
        return withinRange;
    }
    void ShooterBaseLookAt(GameObject lookTarget)
    {
        if (shootingBase == null) return;

        shootingBase.transform.LookAt(lookTarget.transform);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, detectRad);
    }
}
