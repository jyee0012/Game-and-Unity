using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField]
    bool canAnimate = true;

    float rotateSpeed = 100;
    bool animate = true;
    // Start is called before the first frame update
    void Start()
    {
        myWeaponData.maxAmmo = 0;
        myWeaponData.currentAmmo = myWeaponData.maxAmmo;
        myWeaponData.shootProjectile = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAnimate)
        {
            if (animate)
            {
                AnimateSwing();
            }
        }
    }
    public override void Use()
    {
        if (debugText != "")
        {
            Debug.Log(debugText);
        }
        animate = true;
        base.Use();
    }
    void AnimateSwing()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        if (transform.rotation.eulerAngles.z > 40)
        {
            rotateSpeed *= -1;
        }
        if (transform.rotation.eulerAngles.z <= 0)
        {
            rotateSpeed *= -1;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            animate = false;
        }

    }
}
