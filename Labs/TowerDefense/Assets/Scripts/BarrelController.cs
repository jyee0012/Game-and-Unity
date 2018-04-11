﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    PrefabPool prefabPool;
    public Transform bulletPrefab;
    public float rotationSpeed, projectileSpeed = 100;
    void Awake()
    {
        prefabPool = GameObject.Find("PrefabPool").GetComponent<PrefabPool>();
    }
    // Use this for initialization
    void Start()
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = prefabPool.Bullet;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
    void Shoot()
    {
        Transform projectile = prefabPool.Bullet;
        if (projectile != null)
        {
            projectile.position = transform.GetChild(0).transform.position;
            Vector2 projectileDirection = transform.up;
            projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * projectileSpeed);
            projectile.GetComponent<Rigidbody2D>().AddTorque(100);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
