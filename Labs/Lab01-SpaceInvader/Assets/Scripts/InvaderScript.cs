using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderScript : PlayerScript
{

    Vector3 direction = new Vector3(1, 0, 0), moveDown = new Vector3(0, -3, 0);
    float movementDelay;
    public float level = 1;
    // Use this for initialization
    void Start()
    {
        bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
        hp = 1;
        shootDelay = Time.time + Random.Range(2f, 14f);
    }

    // Update is called once per frame
    void Update()
    {
        InvaderState();
    }
    #region State
    void InvaderState()
    {
        switch (thing)
        {
            case State.Alive:
                speed = 2f * (0.5f*level);
                Move(direction);
                if (shootDelay <= Time.time)
                {
                    Shoot();
                    shootDelay = Time.time + Random.Range(5f, 14f);
                }
                if (hp < 1 || (transform.position.x > 9.5 || transform.position.x < -9.5))
                {
                    thing = State.Dead;
                }
                if (transform.position.y <= -6)
                {
                    thing = State.Dead;
                    GameObject.FindGameObjectWithTag("PlayerTank").GetComponent<PlayerScript>().GameOver();
                }
                if (movementDelay <= Time.time)
                {
                    ChangeDirection();
                }
                break;
            case State.Dead:
                Death();
                break;
            case State.BeenHit:
                break;
        }
    }
    #endregion
    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            ChangeEveryInvader();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(collision.gameObject);
        }
    }
    #endregion
    void ChangeEveryInvader()
    {
        foreach(GameObject invader in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            invader.GetComponent<InvaderScript>().ChangeDirection();
        }
    }
    public void ChangeDirection()
    {
        movementDelay = Time.time + 10f;
        direction *= -1;
        Move(moveDown);
    }
}
