using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableUnits : MonoBehaviour
{
    #region Variables
    public enum UnitAI
    {
        Sleep, Attack, Wander, Move
    }
    public UnitAI aichan = UnitAI.Sleep;
    public GameObject target, shootingBase, healthController;
    public bool isSelected = false;

    ShootingScript shootScript = null;
    NavMeshAgent navAgent;
    HealthScript healthScript = null;

    [SerializeField]
    float shootDist = 5, detectRad = 5, wakeDelay = 0, patrolRange = 0;
    [SerializeField]
    bool lockOnTarget = false, // once target != null, set destination to target and eliminate 
        canInterrupt = false, // if shooting @ target, if true then can move and ignore target
        isEnemy = false,
        hasPatrol = false,
        getRandPatrol = false,
        drawDetect = true,
        asleep = false;
    [SerializeField]
    List<Vector2> patrolPoints;
    [SerializeField]
    GameObject selectedIndicator;

    bool firstEncounter = true;
    int currentPatrol = 0;
    Vector3 currentPatrolPoint;
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        #region Variable Defaults
        if (shootScript == null)
            if (GetComponent<ShootingScript>() != null)
                shootScript = GetComponent<ShootingScript>();

        if (navAgent == null)
            if (GetComponent<NavMeshAgent>() != null)
                navAgent = GetComponent<NavMeshAgent>();

        if (healthScript == null)
            if (GetComponent<HealthScript>() != null)
                healthScript = GetComponent<HealthScript>();

        if (!isEnemy)
            if (transform.tag == "EnemyUnit")
                isEnemy = true;

        if (getRandPatrol)
        {
            hasPatrol = true;
            if (patrolRange == 0)
                AddPatrolPoint(GetRandomPatrol());
            else
                AddPatrolPoint(GetRandomPatrolWithinRange(patrolRange));
        }
        #endregion

        // set first patrol point
        if (hasPatrol)
            currentPatrolPoint = new Vector3(patrolPoints[currentPatrol].x, transform.position.y, patrolPoints[currentPatrol].y);

        if (canInterrupt)
            navAgent.stoppingDistance = shootDist - 1;


        wakeDelay += Time.time;

    }
    #endregion
    
    #region Update
    // Update is called once per frame
    void Update()
    {
        selectedIndicator.SetActive(isSelected && !isEnemy);

        bool healthControlActive = (isSelected || isEnemy) ? true : false;
        if (healthController.activeInHierarchy != healthControlActive)
        {
            healthController.SetActive(healthControlActive);
        }

        AiState();
    }
    #endregion
    
    #region AI State
    void AiState()
    {
        switch (aichan)
        {
            #region Sleep
            case UnitAI.Sleep:
                if (!asleep && wakeDelay < Time.time)
                    aichan = UnitAI.Wander;
                break;
            #endregion
            #region Attack
            case UnitAI.Attack:
                // double check if my target is there
                if (CheckTarget(target))
                {
                    // if my target is within my shoot radius
                    if (TargetWithinRange(shootDist))
                    {
                        // can i interrupt and is it my first encounter?
                        if (!canInterrupt || firstEncounter)
                        {
                            navAgent.isStopped = true;
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
                else
                {
                    firstEncounter = true;
                    navAgent.isStopped = false;
                    aichan = UnitAI.Move;
                }
                break;
            #endregion
            #region Wander
            case UnitAI.Wander:
                aichan = UnitAI.Move;
                break;
            #endregion
            #region Move
            case UnitAI.Move:
                #region Check Target
                // if i have a target
                if (CheckTarget(target, "hasTarget"))
                {
                    // if i have a target and it is within my detect radius
                    if (CheckTarget(target))
                    {
                        aichan = UnitAI.Attack;
                    }
                    else
                    {
                        navAgent.SetDestination(target.transform.position);
                    }
                }
                else
                {
                    if (isEnemy) target = EnemyDetection("Moveable");
                    else target = EnemyDetection();
                }
                #endregion
                #region Patrol
                if (hasPatrol)
                {
                    //Debug.Log(gameObject.name + ": I'm patroling");
                    if (navAgent.remainingDistance < 0.2)
                    {
                        //Debug.Log(gameObject.name + ": Got to my destination");
                        currentPatrol++;
                        if (currentPatrol >= patrolPoints.Count)
                        {
                            currentPatrol = 0;
                        }
                        currentPatrolPoint = new Vector3(patrolPoints[currentPatrol].x, transform.position.y, patrolPoints[currentPatrol].y);
                        //Debug.Log(gameObject.name + ": My new destination is " + currentPatrolPoint);
                        navAgent.SetDestination(currentPatrolPoint);
                    }
                    else
                    {
                        //Debug.Log(gameObject.name + ": My current destination is " + navAgent.destination);
                    }
                }
                #endregion
                break;
                #endregion
        }
    }
    #endregion

    public void WakeUp()
    {
        aichan = UnitAI.Move;
    }
    #region Targeting
    bool CheckTarget(GameObject currentTarget, string checkText = "")
    {
        bool hasTarget = false;
        if (currentTarget != null)
        {
            //Debug.Log(currentTarget.transform.root.name + ":" + currentTarget.transform.root.gameObject.activeInHierarchy);

            if (!currentTarget.transform.root.gameObject.activeInHierarchy)
            {
                return false;
            }
            if (checkText == "hasTarget")
            {
                return true;
            }
            if (Physics.Linecast(transform.position, currentTarget.transform.position))
            {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) < shootDist)
                {
                    hasTarget = true;
                }
            }

        }
        return hasTarget;
    }
    GameObject EnemyDetection(string targetTag = "EnemyUnit")
    {
        GameObject newTarget = null;
        // create an overlap sphere for detection
        Collider[] detectionSphere = Physics.OverlapSphere(transform.position, detectRad);
        // set first element in overlap sphere as default distance check
        float currentTargetDist = Vector3.Distance(transform.position, detectionSphere[0].transform.position);
        foreach (Collider detected in detectionSphere)
        {
            // check if new detected is closer than old
            float newDist = Vector3.Distance(transform.position, detected.transform.position);
            bool shorterDist = newDist < currentTargetDist;
            if (detected.transform.tag == targetTag && newTarget == null)
            {
                newTarget = detected.gameObject;
                currentTargetDist = newDist;
            }
            //Debug.Log(gameObject.name + ": " + currentTargetDist + ":" + newDist + " - " + detected.gameObject.name);
            if ((detected.transform.tag == targetTag && shorterDist))
            {
                // check if active
                if (!detected.transform.root.gameObject.activeInHierarchy) continue;

                // if so then set the new target and new distance check
                newTarget = detected.gameObject;
                currentTargetDist = newDist;
            }
        }
        //Debug.Log(gameObject.name +": " + detectionSphere[0].gameObject.name +" | "+currentTargetDist);
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
    #endregion
    #region Patrol
    Vector3 GetRandomPatrol(float walkRadius = 100)
    {
        Vector3 randPatrolPoint = transform.position, randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit navhit;
        NavMesh.SamplePosition(randomDirection, out navhit, walkRadius, 1);
        randPatrolPoint = navhit.position;
        return randPatrolPoint;
    }
    Vector3 GetRandomPatrolWithinRange(float range)
    {
        Vector3 randPatrolPoint = transform.position;
        float randx = Random.Range(-range, range + 1), randy = Random.Range(-range, range + 1);
        randPatrolPoint.x += randx;
        randPatrolPoint.z += randy;
        return randPatrolPoint;
    }
    void AddPatrolPoint(Vector3 patrolPt)
    {
        Vector2 newPatrolPt = new Vector2(patrolPt.x, patrolPt.z);
        AddPatrolPoint(newPatrolPt);
    }
    void AddPatrolPoint(Vector2 patrolPt)
    {
        if (!patrolPoints.Contains(patrolPt))
            patrolPoints.Add(patrolPt);
    }
    void ClearPatrolPoints()
    {
        patrolPoints.Clear();
    }
    #endregion
    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (drawDetect) Gizmos.DrawSphere(transform.position, detectRad);

        if (patrolPoints.Count > 0)
        {
            Gizmos.color = Color.yellow;
            foreach (Vector2 patrolPt in patrolPoints)
            {
                Vector3 patrolPos = new Vector3(patrolPt.x, transform.position.y + 2, patrolPt.y);
                Gizmos.DrawSphere(patrolPos, 0.5f);
            }
        }
    }
    #endregion
}
