using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 10.0f;
    float timeStamp;
    void Start()
    {
        timeStamp = Time.time + 2.0f;
    }
    void Update()
    {
        //transform.Translate(transform.forward * bulletSpeed * Time.deltaTime);
        if (Time.time >= timeStamp)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Zombie") 
        {
            other.transform.GetComponentInParent<ZombieAIOpt>().Damage();
        }
        else if (other.transform.tag == "Survivor")
        {
            other.transform.GetComponentInParent<SurvivorAI>().Damage();
        }
        else if (other.transform.tag == "Soldier")
        {
            other.transform.GetComponentInParent<SoldierAI>().Damage();
        }
        Destroy(gameObject);
    }
}
