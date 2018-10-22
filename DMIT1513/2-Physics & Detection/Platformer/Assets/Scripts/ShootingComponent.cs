using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingComponent : MonoBehaviour {

    public GameObject ShootingProjectile = null;
    bool bCanShoot = false;
	// Use this for initialization
	void Start () {
		if (ShootingProjectile != null)
        {
            bCanShoot = true;
        }
	}

    // Update is called once per frame
    void Update() {
        if (bCanShoot)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SpawnProjectile();
            }
        }
	}
    void SpawnProjectile()
    {
        GameObject projectile = Instantiate(ShootingProjectile, this.transform);
        projectile.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100f);
        //projectile.tag = "Projectile";
    }
}
