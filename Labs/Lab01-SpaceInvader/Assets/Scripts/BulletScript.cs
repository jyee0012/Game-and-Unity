using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float speed = 2f, bulletDMG = 1f, moveBy;
    public string owner;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        moveBy = Time.deltaTime * speed;
        if (owner == "PlayerTank")
        {
            transform.Translate(new Vector3(0, moveBy, 0));
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (owner == "Enemy")
        {
            transform.Translate(new Vector3(0, -moveBy, 0));
        }
    }
    #region Doing the thing
    void DoTheThing(GameObject thing)
    {
        if (thing.tag == "PlayerTank" && owner != "PlayerTank")
        {
            PlayerScript player = thing.GetComponent<PlayerScript>();
            player.getHit();
            Destroy(gameObject);
        }
        else if (thing.tag == "Enemy" && owner != "Enemy")
        {
            InvaderScript enemy = thing.GetComponent<InvaderScript>();
            enemy.TakeDamage();
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("PlayerTank").GetComponent<PlayerScript>().ScoreUp();
        }
        else if (thing.tag == "Wall")
        {
            BarrierScript wall = thing.GetComponent<BarrierScript>();
            wall.TakeDamage();
            Destroy(gameObject);
        }
    }
    #endregion
    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoTheThing(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoTheThing(collision.gameObject);
    }
    #endregion
}
