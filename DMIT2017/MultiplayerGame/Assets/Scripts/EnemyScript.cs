using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent navAgent = null;
    [SerializeField]
    List<PlayerController> playerList = new List<PlayerController>();
    [SerializeField]
    Camera cameraController = null;
    [SerializeField]
    bool playerControlled = false;

    PlayerController closestPlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        SetupNavAgent();
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            playerList.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControlled)
        {
            if (cameraController != null)
            {

            }
        }
        else
        {
            if (closestPlayer != null)
            {
                navAgent.SetDestination(closestPlayer.transform.position);
            }
            else
            {
                FindClosestPlayer();
            }
        }
    }
    void SetupNavAgent()
    {
        if (navAgent == null)
        {
            if (GetComponent<NavMeshAgent>() != null)
            {
                navAgent = GetComponent<NavMeshAgent>();
            }
            else
            {
                navAgent = gameObject.AddComponent<NavMeshAgent>();
            }
        }
    }
    void FindClosestPlayer()
    {
        if (playerList.Count > 0)
        {
            GameObject newPlayer = playerList[0].gameObject;
            for(int i = 1; i < playerList.Count; i++)
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
}
