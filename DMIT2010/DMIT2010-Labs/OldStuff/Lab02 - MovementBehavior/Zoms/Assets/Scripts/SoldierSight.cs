using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSight : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Zombie")
        {
            transform.GetComponentInParent<SoldierAI>().FollowTarget(other.transform);
        }
        //else if (other.transform.tag == "Survivor")
        //{
        //    transform.GetComponentInParent<SurvivorAI>().Flock(other.transform);
        //}
    }
}
