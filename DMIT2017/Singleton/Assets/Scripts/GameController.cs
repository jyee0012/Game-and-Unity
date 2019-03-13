using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gcInstance;

    public int value;

    private void Awake()
    {
        if (gcInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gcInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
