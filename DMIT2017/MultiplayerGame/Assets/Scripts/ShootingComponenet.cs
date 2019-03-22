using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingComponenet : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab = null, muzzleObj = null;
    [SerializeField]
    bool canFire = true;
    [SerializeField, Range(100, 3000)]
    float projectileForce = 300;
    [SerializeField, Range(0, 3)]
    float fireDelay = 1f;
    [SerializeField, Range(1, 10)]
    float projectileLife = 3f;
    [SerializeField]
    KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField]
    PlayerController playerController = null;

    float fireTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }
    void Fire()
    {
        if (FireInput() && canFire && fireTimeStamp < Time.time)
        {
            Shoot(projectileLife);
        }
    }
    bool FireInput()
    {
        bool input = false;
        if (playerController != null)
        {
            if (playerController.useController)
            {
                input = Input.GetAxis("Button5P" + playerController.playerNum) > 0;
            }
            else
            {
                input = Input.GetKeyDown(fireKey);
            }
        }
        else
        {
            Debug.Log("Please attach player controller to " + gameObject.name);
        }
        return input;
    }
    void Shoot(float projectileLife = 10f)
    {
        if (projectilePrefab == null)
        {
            Debug.Log("Please provide projectile prefab to shoot");
            return;
        }
        if (muzzleObj == null)
        {
            Debug.Log("Please provide muzzle object to shoot from");
            return;
        }
        GameObject tempProj = Instantiate(projectilePrefab, muzzleObj.transform.position, muzzleObj.transform.rotation);
        SetupProjectile(tempProj);
        tempProj.GetComponent<Rigidbody>().AddForce(tempProj.transform.forward * projectileForce);
        Destroy(tempProj, projectileLife);
        fireTimeStamp = Time.time + fireDelay;
    }
    void SetupProjectile(GameObject projectile, bool useGrav = false, float projectileMass = 0.1f, float projectileDrag = 1f)
    {
        Rigidbody projectileRBody = null;
        if (projectile.GetComponent<Rigidbody>() == null)
        {
            projectileRBody = projectile.AddComponent<Rigidbody>();
        }
        else
        {
            projectileRBody = projectile.GetComponent<Rigidbody>();
        }

        projectileRBody.mass = projectileMass;
        projectileRBody.useGravity = useGrav;
        projectileRBody.drag = projectileDrag;
    }
}
