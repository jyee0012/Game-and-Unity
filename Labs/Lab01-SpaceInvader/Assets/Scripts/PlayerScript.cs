using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{

    #region Variable
    protected float speed = 2f, dmg = 1f, hp = 3f, shootDelay, hitDelay;
    public enum State { Alive, Dead, BeenHit, End }
    public State thing = State.Alive;
    protected GameObject bulletPrefab;
    bool gotHit = false, hit = false;
    public Text gameText;
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
        gameText.gameObject.SetActive(true);
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
                if (hitDelay <= Time.time)
                {
                    gameObject.GetComponent<SpriteRenderer>().gameObject.SetActive(hit);
                    hit = !hit;
                }
                else
                {
                    thing = State.Alive;
                    gameObject.GetComponent<SpriteRenderer>().gameObject.SetActive(true);
                    hit = false;
                }
                break;
            case State.End:
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
    protected void Shoot()
    {
        Vector3 bulletSpawnPos = transform.position + new Vector3(0, -gameObject.GetComponent<BoxCollider2D>().edgeRadius, +3);
        GameObject shotBullet = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);
        shotBullet.GetComponent<BulletScript>().owner = gameObject.tag;
        Destroy(shotBullet, 5f);
    }
    #endregion
    #region Player Shoot
    void PlayerShoot()
    {
        if (Input.GetKey(KeyCode.Space) && shootDelay < Time.time)
        {
            Shoot();
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
    #region Win/Lose
    public void YouWin()
    {
        gameText.text = "Congratulations, You Win!";
        gameText.gameObject.SetActive(true);
        thing = State.End;
    }
    public void GameOver()
    {
        gameText.text = "Game Over!";
        gameText.gameObject.SetActive(true);
        thing = State.End;
    }
    #endregion
    public void getHit()
    {
        if (thing == State.Alive)
        {
            TakeDamage();
            thing = State.BeenHit;
            hitDelay = Time.time + 5f;
            transform.position = new Vector3(0, -4, -5);
        }
    }
}
