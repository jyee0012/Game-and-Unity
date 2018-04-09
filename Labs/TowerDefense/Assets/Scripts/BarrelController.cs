using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    PrefabPool prefabPool;
    public GameObject bulletPrefab;
    public float rotationSpeed, projectileSpeed;
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

        }
    }
    void Shoot()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
