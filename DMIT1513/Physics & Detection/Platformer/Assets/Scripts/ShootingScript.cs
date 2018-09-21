using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour {

    [SerializeField]
    GameObject projectilePrefab, fireLocationL, fireLocationR;
    [SerializeField]
    KeyCode fireKey = KeyCode.Mouse0; 
    [SerializeField]
    bool projectileGravity = true, projecileCanExplode = false, alternateFire = false, individualFire = false, useAmmo = false;
    [SerializeField]
    float projectileExplodeRadius, projectileForce = 100, timeBetweenShot = 0.5f;
    [SerializeField]
    GameObject[] projectileArray;
    [SerializeField]
    int projectileAmount = 16;

    bool bHasProjectile, bShotL;
    float lastShot, ammo;
    // Use this for initialization
    void Start () {
        bHasProjectile = projectilePrefab != null;
        if (fireKey == KeyCode.None) fireKey = KeyCode.Mouse0;
        ammo = projectileAmount;
        projectileArray = new GameObject[projectileAmount];
    }
	
	// Update is called once per frame
	void Update () {
        Fire();
	}

    bool Shoot(Transform fireLocation)
    {
        Vector3 spawnPos = fireLocation.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity, gameObject.transform);
        //Debug.Log("Spawned");
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        if (projecileCanExplode)
        {
            BombProjectile bombProj = projectile.GetComponent<BombProjectile>();
            bombProj.bCanExplode = true;
            bombProj.explodeRadius = projectileExplodeRadius;
        }
        projectileRBody.useGravity = projectileGravity;
        projectileRBody.AddForce(fireLocation.up * projectileForce);
        Destroy(projectile, 3);
        lastShot = Time.time;
        return true;
    }
    void AlternateShoot()
    {
        if (!alternateFire) return;
        if (bShotL)
        {
            Shoot(fireLocationL.transform);
        }
        else
        {
            Shoot(fireLocationR.transform);
        }
        bShotL = !bShotL;
    }
    void Fire()
    {
        //Debug.Log(Time.time + " - " + lastShot + " = " + (Time.time - lastShot));
        if (Input.GetKey(fireKey) && Time.time - lastShot > timeBetweenShot)
        {
            if (alternateFire) AlternateShoot();
            else if (individualFire) Shoot(transform);
            else Shoot(fireLocationR.transform);
        }
    }
    void Reload()
    {
        foreach(GameObject proj in projectileArray)
        {
            if (!proj.activeInHierarchy)
            {
                proj.SetActive(true);
            }
        }
    }
    void ShootIndividual()
    {
        foreach (GameObject proj in projectileArray)
        {
            if (!proj.activeInHierarchy)
            {
                //proj.SetActive(true);
            }
        }
    }
}
