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
    public ParticleSystem weaponParticles;
    public GameObject weaponProjectile;

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
        weaponParticles = null;
        weaponProjectile = null;
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
}

public class WeaponBase : MonoBehaviour
{
    public WeaponData myWeaponData = new WeaponData();

    [SerializeField]
    Text ammoText = null;

    bool useAmmo = false;
    float reloadTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Use()
    {

        if (useAmmo)
        {
            if (CanFire())
            {
                myWeaponData.UseAmmo();
            }
            else
            {
                Reload();
            }
        }
    }
    protected virtual bool CanFire()
    {
        return myWeaponData.currentClip > 0;
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
}
