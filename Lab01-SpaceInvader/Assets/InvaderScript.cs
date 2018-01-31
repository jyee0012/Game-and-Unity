using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderScript : PlayerScript
{

    Vector3 direction = new Vector3(1, 0, 0), moveDown = new Vector3(0, -3, 0);
    // Use this for initialization
    void Start()
    {
        bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
        hp = 1;
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
                Move(direction);
                if (hp < 1)
                {
                    thing = State.Dead;
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
        direction *= -1;
        Move(moveDown);
    }
}
