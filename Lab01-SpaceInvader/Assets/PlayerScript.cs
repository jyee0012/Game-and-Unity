using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    #region Variable
    protected float speed = 2f, dmg = 1f, hp = 3f, shootDelay;
    public enum State { Alive, Dead, BeenHit }
    public State thing = State.Alive;
    protected GameObject bulletPrefab;
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        States();
    }
    #endregion

    #region States
    void States()
    {
        switch (thing)
        {
            case State.Alive:
                PlayerControls();
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
    #region Player Controls
    void PlayerControls()
    {
        PlayerMovement();
        PlayerShoot();
    }
    #region Movement
    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(new Vector3(1, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5f;
        }
        else
        {
            speed = 2f;
        }
    }
    protected void Move(Vector3 moveBy)
    {
        transform.Translate(moveBy * Time.deltaTime * speed);
    }
    #endregion
    #region Shoot
    protected void Shoot(bool enemy)
    {
        Vector3 bulletSpawnPos = transform.position + new Vector3(0, -gameObject.GetComponent<BoxCollider2D>().edgeRadius, +3);
        GameObject shotBullet = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        shotBullet.GetComponent<BulletScript>().owner = gameObject.tag;
        if (enemy)
        {
            shotBullet.GetComponent<BulletScript>().moveBy *= -1;
        }
        Destroy(shotBullet, 5f);
    }
    #endregion
    #region Player Shoot
    void PlayerShoot()
    {
        if (Input.GetKey(KeyCode.Space) && shootDelay < Time.time)
        {
            Shoot(false);
            float timeDelay = Random.Range(0.2f, 0.6f);
            shootDelay = Time.time + timeDelay;
        }
    }
    #endregion
    #endregion
    #region Health Stuff
    public void TakeDamage()
    {
        hp--;
    }
    public void TakeDamage(int takedmg)
    {
        hp -= takedmg;
    }
    public void Heal()
    {
        hp++;
    }
    public void Heal(int healHealth)
    {
        hp += healHealth;
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    #endregion
}
