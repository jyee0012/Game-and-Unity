using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    float damage = 0, score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RandomizeValues()
    {
        if (damage <= 0) damage = Random.Range(0, 20);
        if (score <= 0) score = Random.Range(1, 100);
    }
    private void OnTriggerEnter(Collider other)
    {
        HitPlayer(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        HitPlayer(collision.collider);
    }
    void HitPlayer(Collider other)
    {
        if (other.tag == "Player")
        {
            RandomizeValues();
            if (other.GetComponent<PlayerController>() != null)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                player.tempData.playerData.TakeDamage(damage);
                player.tempData.playerData.GainScore(score);
            }
            Destroy(gameObject);
        }
    }
}
