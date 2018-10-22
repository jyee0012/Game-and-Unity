﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour {
    
    [SerializeField]
    GameObject projectilePrefab = null, fireLocationL = null, fireLocationR = null;
    [SerializeField]
    KeyCode fireKey = KeyCode.Mouse0; 
    [SerializeField]
    bool projectileGravity = true, projecileCanExplode = false, alternateFire = false, individualFire = false, useAmmo = false;
    [SerializeField]
    float projectileExplodeRadius, projectileForce = 100, timeBetweenShot = 0.5f;
    [SerializeField]
    GameObject[] projectileArray;
    [SerializeField]
    int projectileAmount = 16, maxAmmo = 30;
    [SerializeField]
    Text ammoText, weaponText;
    [SerializeField]
    string weaponName;
    [SerializeField]
    AudioSource fireSound, explodeSound;
    [SerializeField]
    ParticleSystem fireParticle, explodeParticle;

    bool bHasProjectile = false, bShotL = false;
    float lastShot = 0, ammo = 0;

    public bool canFire = true;
    #region Start
    // Use this for initialization
    void Start () {
        bHasProjectile = projectilePrefab != null;
        if (fireKey == KeyCode.None) fireKey = KeyCode.Mouse0;
        if (individualFire) ammo = projectileAmount;
        if (useAmmo) ammo = maxAmmo;
        PrintAmmoText();
        PrintWeaponText();
        SetExplodeParticles();
        //projectileArray = new GameObject[projectileAmount];
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update () {
	}
    private void FixedUpdate()
    {
        Fire();
    }
    #endregion
    #region Shoot
    bool Shoot(Transform fireLocation, string fireDirectionS = "up")
    {
        #region Set Fire Direction
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
        #endregion
        #region Spawn Projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, fireLocation.rotation, null);
        //Debug.Log("Spawned");
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        BombProjectile bombProj = projectile.GetComponent<BombProjectile>();
        bombProj.ignoreObj = this.gameObject;
        if (projecileCanExplode)
        {
            bombProj.bCanExplode = true;
            bombProj.explodeRadius = projectileExplodeRadius;
            if (explodeSound != null) bombProj.boomSound = explodeSound;
        }
        projectileRBody.useGravity = projectileGravity;
        projectileRBody.AddForce(fireDirection * projectileForce);
        Destroy(projectile, 3);
        lastShot = Time.time;
        #endregion
        return true;
    }
    #endregion
    #region AltShoot
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
    #endregion
    #region Fire
    void Fire()
    {
        //Debug.Log(Time.time + " - " + lastShot + " = " + (Time.time - lastShot));
        if (Input.GetKey(fireKey) && Time.time - lastShot > timeBetweenShot && canFire)
        {
            if (alternateFire) AlternateShoot();
            else if (individualFire) ShootIndividual();
            else if (useAmmo)
            {
                if (ammo > 0)
                {
                    Shoot(fireLocationR.transform);
                    ammo--;
                    PrintAmmoText();
                    PlayFireAudio();
                    PlayFireParticle();
                }
            }
            else Shoot(fireLocationR.transform);
        }
    }
    #endregion
    #region Reload
    public void Reload()
    {
        if (individualFire)
        {
            foreach (GameObject proj in projectileArray)
            {
                if (!proj.activeInHierarchy)
                {
                    proj.SetActive(true);
                }
            }
        }
        if (useAmmo)
        {
            ammo = maxAmmo;
        }
        PrintAmmoText();
    }
    #endregion
    #region Shoot Single
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
    #endregion
    #region Ammo/Weapon
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
            int currentAmmo = 0, totalAmmo = 0;
            if (individualFire)
            {
                currentAmmo = GetAmmoCount();
                totalAmmo = projectileAmount;
            }
            else
            {
                currentAmmo = Mathf.RoundToInt(ammo);
                totalAmmo = maxAmmo;
            }
            ammoText.text = "";
            //ammoText.text += "Ammo: ";
            ammoText.text += currentAmmo + "/" + totalAmmo;
        }
    }
    void PrintWeaponText()
    {
        if (weaponText != null)
        {
            weaponText.text = weaponName;
        }
    }
    #endregion
    #region Audio
    void PlayFireAudio()
    {
        if (fireSound == null) return;
        fireSound.loop = false;
        fireSound.Play();
    }
    #endregion
    #region Particles
    void PlayFireParticle()
    {
        if (fireParticle == null) return;
        fireParticle.Play();
    }
    void SetExplodeParticles()
    {
        if (explodeParticle == null) return;
        projectilePrefab.GetComponent<BombProjectile>().explosionEffect = explodeParticle;
    }
    #endregion
}
