using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{

    public int numEnemiesInScene;
    public Transform enemyPrefab;
    protected Transform[] enemyPrefabPool = new Transform[0];
    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeEnemies();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Initialize Enemies
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
    #endregion
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
}
