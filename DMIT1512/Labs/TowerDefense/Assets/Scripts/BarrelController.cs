using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    PrefabPool prefabPool;
    public Transform target;
    public float rotationSpeed, projectileSpeed = 100;
    float shootDelay;
    void Awake()
    {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 0, 3), Space.World);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 0, -3), Space.World);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }
    void GetTarget()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = prefabPool.EnemyShip;
        }

        //have the barrel follow its target
        RotateGradually2D();

        if (target != null && shootDelay < Time.time)
        {
            Shoot();
            shootDelay = Time.time + Random.Range(0.1f, 2f);
        }
    }
    void RotateGradually2D()
    {
        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Quaternion targetLook = Quaternion.Euler(0f, 0f, rot_z - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetLook, Time.deltaTime * 10f);
    }
    #region Shoot
    void Shoot()
    {
        //decide when to shoot (at random intervals)

        Transform projectile = prefabPool.Bullet;
        if (projectile != null)
        {
            projectile.position = transform.GetChild(0).transform.position;
            Vector2 projectileDirection = transform.up;
            projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * projectileSpeed);
            projectile.GetComponent<Rigidbody2D>().AddTorque(100);
        }
    }
    #endregion
    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawCube(target.position, new Vector3(0.5f, 0.5f));
    }
    #endregion
}