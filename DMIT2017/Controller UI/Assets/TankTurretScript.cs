using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretScript : MonoBehaviour {

    [SerializeField]
    bool bWeaponClamp;
    [SerializeField]
    float rotationSpeed = 100, weaponClampMax, weaponClampMin, timeBetweenShot = 0.5f, projectileForce = 100;
    [SerializeField]
    GameObject weaponMount, fireLocation, projectilePrefab;
    [SerializeField]
    KeyCode FireKey = KeyCode.Mouse0;

    bool canFire = true;
    float tInput, wInput, lastShot;
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
        Fire();
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
    void Fire()
    {
        float fireButton = Input.GetAxis("Button0");
        if ((fireButton != 0 || Input.GetKey(FireKey)) && Time.time - lastShot > timeBetweenShot && canFire)
        {
            Shoot(fireLocation.transform);
        }
    }
    bool Shoot(Transform fireLocation)
    {
        Vector3 spawnPos = fireLocation.position, fireDirection = fireLocation.forward;
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, fireLocation.rotation, null);
        //Debug.Log("Spawned");
        Rigidbody projectileRBody = projectile.GetComponent<Rigidbody>();
        projectileRBody.AddForce(fireDirection * projectileForce);
        Destroy(projectile, 3);
        lastShot = Time.time;
        return true;
    }
}
