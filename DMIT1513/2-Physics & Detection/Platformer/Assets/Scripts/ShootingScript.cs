using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    Text ammoText;
    
    bool bHasProjectile, bShotL;
    float lastShot, ammo;
    // Use this for initialization
    void Start () {
        bHasProjectile = projectilePrefab != null;
        if (fireKey == KeyCode.None) fireKey = KeyCode.Mouse0;
        ammo = projectileAmount;
        PrintAmmoText();
        //projectileArray = new GameObject[projectileAmount];
    }
	
	// Update is called once per frame
	void Update () {
        Fire();
	}

    bool Shoot(Transform fireLocation, string fireDirectionS = "up")
    {
        Vector3 spawnPos = fireLocation.position, fireDirection = fireLocation.up;
        switch (fireDirectionS)
        {
            case "up":
                fireDirection = fireLocation.up;
                break;
            case "down":
                fireDirection = -fireLocation.up;
                break;
            case "left":
                fireDirection = -fireLocation.right;
                break;
            case "right":
                fireDirection = fireLocation.right;
                break;
            case "forward":
                fireDirection = fireLocation.forward;
                break;
            case "back":
                fireDirection = -fireLocation.forward;
                break;
            default:
                fireDirection = fireLocation.up;
                break;
        }
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, fireLocation.rotation, null);
        //Debug.Log("Spawned");
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        BombProjectile bombProj = projectile.GetComponent<BombProjectile>();
        bombProj.ignoreObj = this.gameObject;
        if (projecileCanExplode)
        {
            bombProj.bCanExplode = true;
            bombProj.explodeRadius = projectileExplodeRadius;
        }
        projectileRBody.useGravity = projectileGravity;
        projectileRBody.AddForce(fireDirection * projectileForce);
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
            else if (individualFire) ShootIndividual();
            else Shoot(fireLocationR.transform);
        }
    }
    public void Reload()
    {
        foreach(GameObject proj in projectileArray)
        {
            if (!proj.activeInHierarchy)
            {
                proj.SetActive(true);
            }
        }
        PrintAmmoText();
    }
    void ShootIndividual()
    {
        foreach (GameObject proj in projectileArray)
        {
            if (proj.activeInHierarchy)
            {
                Shoot(proj.transform, "forward");
                proj.SetActive(false);
                break;
            }
        }
        PrintAmmoText();
    }
    int GetAmmoCount()
    {
        int ammo = 0;
        for (int i = 0;i< projectileArray.Length; i++)
        {
            if (projectileArray[i].activeInHierarchy)
            {
                ammo++;
            }
        }
        return ammo;
    }
    void PrintAmmoText()
    {
        if (ammoText != null)
        {
            int currentAmmo = GetAmmoCount();
            ammoText.text = "Ammo: " + currentAmmo + "/" + projectileAmount;
        }
    }
}
