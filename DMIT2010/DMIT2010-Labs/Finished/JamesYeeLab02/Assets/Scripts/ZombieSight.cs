using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSight : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Survivor" || other.transform.tag == "Soldier")
        {
            transform.GetComponentInParent<ZombieAIOpt>().FollowTarget(other.transform);
        }
    }
}
