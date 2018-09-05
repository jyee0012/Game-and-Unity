using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Zombie" || other.transform.tag == "Survivor" || other.transform.tag == "Soldier")
        {
            transform.GetComponentInParent<SoldierAI>().FollowTarget(other.transform);
        }
        //else if (other.transform.tag == "Survivor")
        //{
        //    transform.GetComponentInParent<SurvivorAI>().Flock(other.transform);
        //}
    }
}
