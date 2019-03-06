using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Weapon myWeapon;

    [SerializeField]
    Text weaponName, currentAmmo, maxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        myWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        weaponName.text = myWeapon.GetName();
        currentAmmo.text = myWeapon.GetCurrentAmmo().ToString();
        maxAmmo.text = myWeapon.GetMaxAmmo().ToString();

        if (Input.GetAxis("Fire1") == 1 || Input.GetKeyDown(KeyCode.Mouse0))
        {
            myWeapon.Use();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            myWeapon.Reload();
        }
    }
}
