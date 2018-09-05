using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public bool key = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (key)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().hasKey = true;
        }
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
