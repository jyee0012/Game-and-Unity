using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackFuel : MonoBehaviour {
    public int fuelAmount = 20;

    public void OnTriggerEnter(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.tag.Equals("Player"))
        {
            Debug.Log("FUEL");
            obj.GetComponent<PlayerStats>().Refuel(fuelAmount);

            //Destroy object
            Destroy(gameObject);
        }
    }
}
