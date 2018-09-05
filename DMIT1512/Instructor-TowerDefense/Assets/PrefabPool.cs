using UnityEngine;

public class PrefabPool : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        InitializeEnemies();
        InitializeProjectiles();
        InitializeTurrets();
    }

    public int numEnemiesInScene;
    public Transform enemyPrefab;
    protected Transform[] enemyPrefabPool = new Transform[0];
    public void InitializeEnemies()
    {
        if(enemyPrefabPool.Length == 0)
        {
            enemyPrefabPool = new Transform[numEnemiesInScene];
            for(int c = 0; c < numEnemiesInScene; c++)
            {
                enemyPrefabPool[c] = Instantiate(enemyPrefab);
                enemyPrefabPool[c].gameObject.SetActive(false);
            }
        }
    }
    public Transform Enemy
    {
        get
        {
            Transform returnTransform = null;
            int c = 0;
            //loop through the array until an available Enemy is found
            while(c < enemyPrefabPool.Length && returnTransform == null)
            {
                if(!enemyPrefabPool[c].gameObject.activeInHierarchy)
                {
                    returnTransform = enemyPrefabPool[c];
                    enemyPrefabPool[c].gameObject.SetActive(true);
                }
                c++;
            }
            return returnTransform;
        }
    }

    public int numPlayerProjectilesInScene;
    public Transform projectilePrefab;
    protected Transform[] projectilePool = new Transform[0];
    public void InitializeProjectiles()
    {
        if (projectilePool.Length == 0)
        {
            projectilePool = new Transform[numPlayerProjectilesInScene];
            for (int c = 0; c < numPlayerProjectilesInScene; c++)
            {
                projectilePool[c] = Instantiate(projectilePrefab);
                projectilePool[c].gameObject.SetActive(false);
            }
        }
    }
    public Transform Projectile
    {
        get
        {
            Transform returnTransform = null;
            int c = 0;
            while (c < projectilePool.Length && returnTransform == null)
            {
                if (!projectilePool[c].gameObject.activeInHierarchy)
                {
                    returnTransform = projectilePool[c];
                    projectilePool[c].gameObject.SetActive(true);
                }
                c++;
            }
            return returnTransform;
        }
    }

    public int numTurretsInScene;
    public Transform turretPrefab;
    protected Transform[] turretPool = new Transform[0];
    public void InitializeTurrets()
    {
        if (turretPool.Length == 0)
        {
            turretPool = new Transform[numTurretsInScene];
            for (int c = 0; c < numTurretsInScene; c++)
            {
                turretPool[c] = Instantiate(turretPrefab);
                turretPool[c].gameObject.SetActive(false);
            }
        }
    }
    public Transform Turret
    {
        get
        {
            Transform returnTransform = null;
            int c = 0;
            while (c < turretPool.Length && returnTransform == null)
            {
                if (!turretPool[c].gameObject.activeInHierarchy)
                {
                    returnTransform = turretPool[c];
                    turretPool[c].gameObject.SetActive(true);
                }
                c++;
            }
            return returnTransform;
        }
    }

}
