using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Projectile") return;
        GameObject.Find("MissilePod").GetComponent<ShootingScript>().Reload();
        Destroy(this.gameObject);
    }
}
