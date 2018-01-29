using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float speed = 2f, bulletDMG = 1f;
    string owner;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0, Time.deltaTime*speed, 0));
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerTank":
                PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
                player.TakeDamage();
                Destroy(gameObject);
                break;
            case "Enemy":
                InvaderScript enemy = collision.gameObject.GetComponent<InvaderScript>();
                enemy.TakeDamage();
                Destroy(gameObject);
                break;
        }
    }
}
