using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float activeTimer;
    bool once = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy && once)
        {
            activeTimer = Time.time + 5f;
            once = false;
        }
        if (gameObject.activeInHierarchy && activeTimer < Time.time)
        {
            gameObject.SetActive(false);
            once = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
