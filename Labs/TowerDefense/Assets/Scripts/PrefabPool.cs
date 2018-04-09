using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{

    public int numEnemiesInScene, bulletNum;
    public Transform enemyPrefab;
    public GameObject bulletPrefab;
    protected Transform[] enemyPrefabPool = new Transform[0];
    protected GameObject[] bulletPrefabPool = new GameObject[0];
    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeEnemies();
        InitializeBullets();
    }
    void Start()
    {
        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load("Enemy") as Transform;
        }
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load("Bullet") as GameObject;
        }
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
                enemyPrefabPool[i] = Instantiate(enemyPrefab);
                enemyPrefabPool[i].gameObject.SetActive(false);
            }
        }
    }
    public void InitializeBullets()
    {
        if (bulletPrefabPool.Length == 0)
        {
            bulletPrefabPool = new GameObject[bulletNum];
            for (int i = 0; i < bulletNum; i++)
            {
                bulletPrefabPool[i] = Instantiate(bulletPrefab);
                bulletPrefabPool[i].gameObject.SetActive(false);
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
    public GameObject Bullet
    {
        get
        {
            GameObject returnBullet = null;
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
    #endregion
}
