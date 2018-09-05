using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorSight : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Zombie")
        {
            transform.GetComponentInParent<SurvivorAI>().Flee(other.transform.position);
        }
        else if (other.transform.tag == "Survivor" || other.transform.tag == "Soldier")
        {
            transform.GetComponentInParent<SurvivorAI>().FollowTarget(other.transform);
        }
    }
}
