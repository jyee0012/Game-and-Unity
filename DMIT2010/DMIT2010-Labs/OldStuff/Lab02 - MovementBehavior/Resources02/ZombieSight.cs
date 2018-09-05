using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSight : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Survivor")
        {
            transform.GetComponentInParent<ZombieAI>().FollowTarget(other.transform.position);
        }
    }
}
