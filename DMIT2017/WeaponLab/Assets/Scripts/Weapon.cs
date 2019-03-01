using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponData
{
    public enum WeaponType { Melee, Range }
    public string name, description;
    public float damage, attackRate, attackRange;
    public float currentAmmo, maxAmmo;
    public float weaponSpread, reloadTime, accuracy, recoil, aimSpd;
    public float currentClip, maxClip;
    public float projectileSpeed;

    public ParticleSystem weaponParticles;
    public GameObject weaponProjectile, muzzleObject;
    public bool canFire { get { return currentClip > 0; } }

    public WeaponData()
    {
        name = "Nameless";
        description = "Nothing";
        damage = 1f;
        attackRate = 1f;
        attackRange = 10f;
        maxAmmo = 100;
        currentAmmo = maxAmmo;
        weaponSpread = 0f;
        reloadTime = 1f;
        accuracy = 100;
        recoil = 0;
        aimSpd = 100;
        maxClip = 10;
        currentClip = maxClip;

        projectileSpeed = 10f;

        weaponParticles = null;
        weaponProjectile = null;
        muzzleObject = null;
    }
    public WeaponData(string wepName, string wepDesc, float wepDmg, float wepAtkRate, float wepAtkRange, float wepMaxAmmo, float wepSpread, float wepReloadTime, float wepAcc, float wepRecoil, float wepAimSpd, float wepClipSize)
    {
        name = wepName;
        description = wepDesc;
        damage = wepDmg;
        attackRate = wepAtkRate;
        attackRange = wepAtkRange;
        maxAmmo = wepMaxAmmo;
        weaponSpread = wepSpread;
        reloadTime = wepReloadTime;
        accuracy = wepAcc;
        recoil = wepRecoil;
        aimSpd = wepAimSpd;
        maxClip = wepClipSize;
        currentAmmo = maxAmmo;
        currentClip = maxClip;
    }
    public WeaponData(WeaponData newWeapon)
    {
        name = newWeapon.name;
        description = newWeapon.description;
        damage = newWeapon.damage;
        attackRate = newWeapon.attackRate;
        attackRange = newWeapon.attackRange;
        currentAmmo = newWeapon.currentAmmo;
        maxAmmo = newWeapon.maxAmmo;
        weaponSpread = newWeapon.weaponSpread;
        reloadTime = newWeapon.reloadTime;
        accuracy = newWeapon.accuracy;
        recoil = newWeapon.recoil;
        aimSpd = newWeapon.aimSpd;
        maxClip = newWeapon.maxClip;
        currentClip = newWeapon.currentClip;
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
        currentClip -= amount;
        currentAmmo -= amount;
    }
    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
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

    bool useAmmo = false;
    float reloadTimeStamp = 0, fireDelay = 0;
    public virtual void Use()
    {

        if (useAmmo)
        {
            if (CanFire())
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
    protected virtual bool CanFire()
    {
        return myWeaponData.canFire;
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
    public virtual float GetCurrentClip()
    {
        return myWeaponData.currentClip;
    }
    public virtual float GetMaxAmmo()
    {
        return myWeaponData.maxAmmo;
    }
    public virtual void Reload()
    {
        reloadTimeStamp = Time.time + myWeaponData.reloadTime;
        myWeaponData.ResetAmmo();
    }
    public virtual string GetCurrentAmmoText()
    {
        return myWeaponData.name + ": " + GetCurrentClip() + "/" + GetCurrentAmmo();
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
