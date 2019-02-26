using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public enum WeaponType { Melee, Range }
    public string name, description;
    public float damage, attackRate, attackRange;
    public float currentAmmo, maxAmmo;
    public float weaponSpread, reloadTime, accuracy, recoil, aimSpd;
    public WeaponData()
    {

    }
    public WeaponData(WeaponData newWeapon)
    {

    }
    public void PlayAnimation()
    {

    }
    public void PlayParticle()
    {

    }
}

public class WeaponBase : MonoBehaviour
{

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

    }
    public virtual string GetName()
    {

    }
    public virtual float GetCurrAmmo()
    {

    }
    public virtual float GetMaxAmmo()
    {

    }
}
