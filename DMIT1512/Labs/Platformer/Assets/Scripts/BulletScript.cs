using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public Vector3 direction { get; set; }
    float speed = 15f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Move(direction);
	}
    void Move(Vector3 direct)
    {
        transform.Translate(direct * Time.deltaTime * speed);
    }
    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
