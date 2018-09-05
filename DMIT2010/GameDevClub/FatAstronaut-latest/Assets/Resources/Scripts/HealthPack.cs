using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {
    public int healthAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag.Equals("Player"))
        {
            Debug.Log("HEAL");
            obj.GetComponent<PlayerStats>().Heal(1);

            //destroy self
            Destroy(gameObject);
        }
    }
}
