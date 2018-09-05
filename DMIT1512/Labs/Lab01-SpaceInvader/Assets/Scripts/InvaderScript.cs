using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderScript : PlayerScript
{
    Vector3 direction = new Vector3(1, 0, 0), moveDown = new Vector3(0, -3, 0);
    float movementDelay;
    public float level = 1, scoreHold = 10;
    int bounceCounter = 0;
    public bool unique = false;
    // Use this for initialization
    void Start()
    {
        if (!unique)
        {
            bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
        }
        else
        {
            bulletPrefab = Resources.Load("newBulletPrefab") as GameObject;
            direction *= -1;
            scoreHold = 100;
        }
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
            #region Alive
            case State.Alive:
                if (!unique)
                {
                    speed = 2f * (0.5f * level);
                }
                else
                {
                    speed = 2f;
                }
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
                    if (!unique)
                    {
                        ChangeDirection();
                    }
                    else
                    {
                        NewChangeDirection();
                    }
                }
                if (bounceCounter >= 3)
                {
                    transform.Translate(new Vector3(-0.5f, 0, 0));
                }
                break;
            #endregion
            #region Dead
            case State.Dead:
                Death();
                break;
            #endregion
            #region Been Hit
            case State.BeenHit:
                break;
            #endregion
            #region Pause
            case State.Pause:
                break;
                #endregion
        }
    }
    #endregion
    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!unique)
            {
                ChangeEveryInvader();
            }
            else
            {
                NewChangeDirection();
            }
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
    #region Change Invader
    void ChangeEveryInvader()
    {
        foreach (GameObject invader in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            invader.GetComponent<InvaderScript>().ChangeDirection();
        }
    }
    public void ChangeDirection()
    {
        if (!unique)
        {
            movementDelay = Time.time + 10f;
            direction *= -1;
            Move(moveDown);
        }
    }
    void NewChangeDirection()
    {
        movementDelay = Time.time + 10f;
        direction *= -1;
        bounceCounter++;
    }
    #endregion
}
