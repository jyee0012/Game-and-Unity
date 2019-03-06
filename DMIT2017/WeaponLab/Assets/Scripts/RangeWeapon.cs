using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Use()
    {
        if (debugText != "")
        {
            Debug.Log(debugText);
        }
        if (GetCurrentAmmo() > 0)
        {
            myWeaponData.UseAmmo();
        }
        base.Use();
    }
}
