using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class WeaponData
{
    public enum WeaponType { Melee, Range }
    public string name, description;
    public float damage, attackRate, attackRange;
    public float currentAmmo, maxAmmo;
    public float projectileSpeed;
    public bool shootProjectile;

    public ParticleSystem weaponParticles;
    public GameObject weaponProjectile, muzzleObject;

    public WeaponData()
    {
        shootProjectile = true;
        name = "Nameless";
        description = "Nothing";
        damage = 1f;
        attackRate = 1f;
        attackRange = 10f;
        maxAmmo = 100;
        currentAmmo = maxAmmo;

        projectileSpeed = 10f;

        weaponParticles = null;
        weaponProjectile = null;
        muzzleObject = null;
    }
    public WeaponData(bool canShoot, string wepName, string wepDesc, float wepDmg, float wepAtkRate, float wepAtkRange, float wepMaxAmmo, float wepSpread, float wepReloadTime, float wepAcc, float wepRecoil, float wepAimSpd, float wepClipSize)
    {
        shootProjectile = canShoot;
        name = wepName;
        description = wepDesc;
        damage = wepDmg;
        attackRate = wepAtkRate;
        attackRange = wepAtkRange;
        maxAmmo = wepMaxAmmo;
        currentAmmo = maxAmmo;
    }
    public WeaponData(WeaponData newWeapon)
    {
        shootProjectile = newWeapon.shootProjectile;
        name = newWeapon.name;
        description = newWeapon.description;
        damage = newWeapon.damage;
        attackRate = newWeapon.attackRate;
        attackRange = newWeapon.attackRange;
        currentAmmo = newWeapon.currentAmmo;
        maxAmmo = newWeapon.maxAmmo;
    }
    public void PlayAnimation()
    {

    }
    public void PlayParticle()
    {
        if (weaponParticles != null)
        {
            weaponParticles.Play();
        }
    }
    public void UseAmmo(float amount = 1)
    {
        currentAmmo -= amount;
    }
    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
    }
}
[Serializable]
public class RangeWeaponData
{
    public float weaponSpread, reloadTime, accuracy, recoil, aimSpd;
    public float currentClip, maxClip;

    public bool canFire { get { return currentClip > 0; } }

    public RangeWeaponData()
    {
        weaponSpread = 0f;
        reloadTime = 1f;
        accuracy = 100;
        recoil = 0;
        aimSpd = 100;
        maxClip = 10;
        currentClip = maxClip;
    }
    public RangeWeaponData(float wepSpread, float wepReloadTime, float wepAcc, float wepRecoil, float wepAimSpd, float wepClipSize)
    {
        weaponSpread = wepSpread;
        reloadTime = wepReloadTime;
        accuracy = wepAcc;
        recoil = wepRecoil;
        aimSpd = wepAimSpd;
        maxClip = wepClipSize;
        currentClip = maxClip;
    }
    public RangeWeaponData(RangeWeaponData newWeapon)
    {
        weaponSpread = newWeapon.weaponSpread;
        reloadTime = newWeapon.reloadTime;
        accuracy = newWeapon.accuracy;
        recoil = newWeapon.recoil;
        aimSpd = newWeapon.aimSpd;
        maxClip = newWeapon.maxClip;
        currentClip = newWeapon.currentClip;
    }
    public void UseAmmo(float amount = 1)
    {
        currentClip -= amount;
    }
    public void ResetClip()
    {
        currentClip = maxClip;
    }
}


public class Weapon : MonoBehaviour
{
    public WeaponData myWeaponData = new WeaponData();

    [SerializeField]
    Text ammoText = null;
    [SerializeField]
    protected string debugText = "";

    protected bool useAmmo = false;
    protected float reloadTimeStamp = 0, fireDelay = 0;
    public virtual void Use()
    {

        if (useAmmo)
        {
            if (GetCurrentAmmo() > 0)
            {
                FireProjectile(myWeaponData.weaponProjectile);
                myWeaponData.UseAmmo();
                fireDelay = Time.time + myWeaponData.attackRate;
            }
            else
            {
                Reload();
            }
        }
    }
    public virtual string GetName()
    {
        return myWeaponData.name;
    }
    public virtual float GetCurrAmmo()
    {
        return GetCurrentAmmo();
    }
    public virtual float GetCurrentAmmo()
    {
        return myWeaponData.currentAmmo;
    }
    public virtual float GetMaxAmmo()
    {
        return myWeaponData.maxAmmo;
    }
    public virtual void Reload()
    {
        myWeaponData.ResetAmmo();
    }
    public virtual string GetCurrentAmmoText()
    {
        return myWeaponData.name + ": " + GetCurrentAmmo() + "/" + GetMaxAmmo();
    }
    public virtual void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = GetCurrentAmmoText();
        }
    }
    public virtual void FireProjectile(GameObject projectile)
    {
        Vector3 fireLoc = Vector3.zero;
        Quaternion fireRot = Quaternion.identity;
        if (projectile == null) return;
        if (myWeaponData.muzzleObject == null)
        {
            fireLoc = transform.position + (Vector3.forward * 5f);
            fireRot = transform.rotation;
        }
        else
        {
            fireLoc = myWeaponData.muzzleObject.transform.position;
            fireRot = myWeaponData.muzzleObject.transform.rotation;
        }
        GameObject tempProj = Instantiate(projectile, fireLoc, fireRot);
        if (tempProj.GetComponent<Rigidbody>() == null)
        {
            tempProj.AddComponent<Rigidbody>();
        }
        Rigidbody tempProjRB = tempProj.GetComponent<Rigidbody>();
        tempProjRB.AddForce(myWeaponData.muzzleObject.transform.forward * myWeaponData.projectileSpeed);
        Destroy(tempProj, 10f);
    }
}
