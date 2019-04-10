using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
    private int fingerID = -1;
    private void Awake()
    {
        #if !UNITY_EDITOR
            fingerID = 0; 
        #endif
    }
    public NavMeshAgent navAgent = null;
    public Camera cameraController = null;
    public bool playerControlled = false;
    [SerializeField]
    bool showMouse = false, showDestination = false;
    [SerializeField]
    GameObject mouseIndicator = null;
    [SerializeField]
    KeyCode setDestinationKey = KeyCode.Mouse0;
    Vector3 startPos = Vector3.zero;
    PlayerController closestPlayer = null;

    [Header("Retreat Values")]
    [SerializeField]
    bool canRetreat = true;
    [SerializeField]
    float retreatRange = 3;
    [SerializeField, Range(0, 100)]
    int retreatChance = 30;

    // Start is called before the first frame update
    void Start()
    {
        SetupNavAgent();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControlled )
        {
            if (GetPlayerInput())
            {
                PlayerSetDestination();
            }
        }
        else
        {
            EnemyAI();
        }
    }
    void EnemyAI()
    {
        if (closestPlayer != null)
        {
            if (canRetreat)
            {
                //if (Vector3.Distance(closestPlayer.transform.position, transform.position) < retreatRange)
                //{
                //    navAgent.SetDestination(startPos);
                //}
                //else 
                if (Vector3.Distance(startPos, transform.position) < (retreatRange / 2))
                {
                    navAgent.SetDestination(closestPlayer.transform.position);
                }
            }
            else
            {
                navAgent.SetDestination(closestPlayer.transform.position);
            }
        }
        else
        {
            FindClosestPlayer(FindAllPlayers());
        }
    }
    void PlayerSetDestination()
    {
        RaycastHit clickInfo;
        bool hitUI = false;
        Ray mouseRay = cameraController.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject(fingerID))    // is the touch on the GUI
        {
            // GUI Action
            hitUI = true;
        }
        if (Physics.Raycast(mouseRay, out clickInfo) && !hitUI)
        {
            if (showMouse && mouseIndicator != null)
            {
                mouseIndicator.transform.position = clickInfo.point;
            }
            navAgent.SetDestination(clickInfo.point);
            if (transform.parent.GetComponent<SpawnerScript>() != null)
            {
                SpawnerScript spawnParent = transform.parent.GetComponent<SpawnerScript>();
                spawnParent.hasDestination = true;
                spawnParent.enemyDestination = navAgent.destination;
            }

        }
    }
    bool GetPlayerInput()
    {
        return (Input.GetKeyDown(setDestinationKey) && cameraController != null);
    }
    void SetupNavAgent()
    {
        if (navAgent == null) navAgent = GetComponent<NavMeshAgent>();
    }
    void FindClosestPlayer(List<PlayerController> playerList)
    {
        if (playerList.Count > 0)
        {
            GameObject newPlayer = playerList[0].gameObject;
            for (int i = 1; i < playerList.Count; i++)
            {
                float currentDist = Vector3.Distance(transform.position, newPlayer.transform.position),
                    newDist = Vector3.Distance(transform.position, playerList[i].transform.position);
                if (currentDist > newDist)
                {
                    newPlayer = playerList[i].gameObject;
                }
            }


            closestPlayer = newPlayer.GetComponent<PlayerController>();
        }
    }
    List<PlayerController> FindAllPlayers()
    {
        List<PlayerController> playerList = new List<PlayerController>();
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            playerList.Add(player);
        }
        return playerList;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.transform.tag == "Player")
        {
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag != "Enemy")
        {
            navAgent.SetDestination(startPos);
        }
    }
    private void OnDrawGizmos()
    {
        if (showDestination)
        {
            Gizmos.color = Color.yellow;
            Vector3 aboveDest = navAgent.destination;
            aboveDest.y += 5f;
            Gizmos.DrawSphere(aboveDest, 0.5f);
        }
    }
}
