using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public float initialForwardAngle = 90; // initial angle of your "gun barrel"

    public float rotationSpeed = 60;
    public float projectileForce = 1000;

    protected Transform target;
    protected PrefabPool prefabPool;
    private void Awake()
    {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
    }
    void Update ()
    {
        //if(target == null)
        //{
        //    //this should be moved to prefabPool
        //    GameObject[] enemyShips = GameObject.FindGameObjectsWithTag("Enemy");
        //    int randomShip = Random.Range(0, enemyShips.Length);
        //    target = enemyShips[randomShip].transform;
        //}

        ////have the barrel follow its target
        ////RotateGradually2D();

        //Shoot();

    }
    
    protected void Shoot()
    {
        //decide when to shoot (at random intervals)

        Transform projectile = prefabPool.Projectile;
        if (projectile != null)
        {
            projectile.position = transform.GetChild(0).transform.position;
            Vector2 projectileDirection = transform.up;
            projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * projectileForce);
            projectile.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
        }
    }

}
