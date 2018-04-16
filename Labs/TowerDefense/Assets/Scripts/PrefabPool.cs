using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{

    public int numEnemiesInScene, bulletNum, turretNum;
    public GameObject enemyPrefab, bulletPrefab, turretPrefab;
    protected Transform[] enemyPrefabPool = new Transform[0],  bulletPrefabPool = new Transform[0], turretPrefabPool = new Transform[0];
    public Material enemyMat, bulletMat, turretMat;
    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load("Enemy") as GameObject;
        }
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load("Bullet") as GameObject;
        }
        if (turretPrefab == null)
        {
            turretPrefab = Resources.Load("Turret") as GameObject;
        }
        InitializeEnemies();
        InitializeBullets();
        InitializeTurrets();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Initialize Things
    public void InitializeEnemies()
    {
        if (enemyPrefabPool.Length == 0)
        {
            enemyPrefabPool = new Transform[numEnemiesInScene];
            for (int i = 0; i < numEnemiesInScene; i++)
            {
                enemyPrefabPool[i] = Instantiate(enemyPrefab.transform, gameObject.transform);
                if(enemyMat != null)
                {
                    enemyPrefabPool[i].GetComponent<MeshRenderer>().material = enemyMat;
                }
                enemyPrefabPool[i].gameObject.SetActive(false);
            }
        }
    }
    public void InitializeBullets()
    {
        if (bulletPrefabPool.Length == 0)
        {
            bulletPrefabPool = new Transform[bulletNum];
            for (int i = 0; i < bulletNum; i++)
            {
                bulletPrefabPool[i] = Instantiate(bulletPrefab.transform, gameObject.transform);
                if (bulletMat != null)
                {
                    bulletPrefabPool[i].GetComponent<MeshRenderer>().material = bulletMat;
                }
                bulletPrefabPool[i].gameObject.SetActive(false);
            }
        }
    }
    public void InitializeTurrets()
    {
        if (turretPrefabPool.Length == 0)
        {
            turretPrefabPool = new Transform[turretNum];
            for (int i = 0; i < turretNum; i++)
            {
                turretPrefabPool[i] = Instantiate(turretPrefab.transform, gameObject.transform);
                if (turretMat != null)
                {
                    turretPrefabPool[i].GetComponent<MeshRenderer>().material = turretMat;
                }
                turretPrefabPool[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Return Objects
    public Transform Enemy
    {
        get
        {
            Transform returnTransform = null;
            int i = 0;
            while (i < enemyPrefabPool.Length && returnTransform == null)
            {
                if (!enemyPrefabPool[i].gameObject.activeInHierarchy)
                {
                    returnTransform = enemyPrefabPool[i];
                    enemyPrefabPool[i].gameObject.SetActive(true);
                }
                i++;
            }
            return returnTransform;
        }
    }
    public Transform Bullet
    {
        get
        {
            Transform returnBullet = null;
            int i = 0;
            while (i < bulletPrefabPool.Length && returnBullet == null)
            {
                if (!bulletPrefabPool[i].gameObject.activeInHierarchy)
                {
                    returnBullet = bulletPrefabPool[i];
                    bulletPrefabPool[i].gameObject.SetActive(true);
                }
                i++;
            }
            return returnBullet;
        }
    }
    public Transform Turret
    {
        get
        {
            Transform returnTurret = null;
            int i = 0;
            while (i< turretPrefabPool.Length && returnTurret == null)
            {
                if (!turretPrefabPool[i].gameObject.activeInHierarchy)
                {
                    returnTurret = turretPrefabPool[i];
                    turretPrefabPool[i].gameObject.SetActive(true);
                }
                i++;
            }
            return returnTurret;
        }
    }
    public Transform EnemyShip
    {
        get
        {
            Transform enemyShip = null;
            enemyShip = GameObject.FindGameObjectsWithTag("Enemy")[0].transform;
            return enemyShip;
        }
    }
    #endregion
}
