using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomSight : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Survivor")
        {
            transform.GetComponentInParent<ZomAI>().FollowTarget(other.transform.position);
        }
    }
}
