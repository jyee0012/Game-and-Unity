using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretScript : MonoBehaviour {

    [SerializeField]
    bool bWeaponClamp;
    [SerializeField]
    float rotationSpeed = 100, weaponClampMax, weaponClampMin;
    [SerializeField]
    GameObject weaponMount;

    float tInput, wInput;
    Vector3 angles;

    // Use this for initialization
    void Start ()
    {
        if (weaponClampMin < 0) weaponClampMin = 360 + weaponClampMin;
    }
	
	// Update is called once per frame
	void Update () {
        TurretMovement();
        WeaponMountClamp();
	}
    void TurretMovement()
    {
        tInput = Input.GetAxis("Axis6");
        transform.Rotate(Vector3.up, tInput * Time.deltaTime * -rotationSpeed);
        if (weaponMount != null)
        {
            wInput = Input.GetAxis("Axis7");
            weaponMount.transform.Rotate(Vector3.right, wInput * Time.deltaTime * -rotationSpeed);
        }
    }
    void WeaponMountClamp()
    {
        if (!bWeaponClamp) return;
        angles = weaponMount.transform.rotation.eulerAngles;
        if (angles.x >= weaponClampMax)
        {
            weaponMount.transform.localRotation = Quaternion.Euler(weaponClampMax, angles.y, angles.z);
        }
        if (angles.x <= weaponClampMin && angles.x > 270)
        {
            weaponMount.transform.localRotation = Quaternion.Euler(weaponClampMin, angles.y, angles.z);
        }
    }
}
