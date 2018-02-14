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
    int hitNum = 0, score = 0;
    public Text gameText, scoreText, subGameText;
    public GameObject life1, life2, life3, life4, life5;
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        bulletPrefab = Resources.Load("bulletPrefab") as GameObject;
        gameText.gameObject.SetActive(false);
        gameText.text = "";
        subGameText.gameObject.SetActive(false);
        subGameText.text = "";
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
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
                ShowHealth();
                if (hp < 1)
                {
                    thing = State.Dead;
                }
                else if (hp > 5)
                {
                    hp = 5;
                }
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
                {
                    YouWin();
                }
                break;
            case State.Dead:
                GameOver();
                Death();
                break;
            case State.BeenHit:
                if (hitDelay <= Time.time)
                {
                    if (hitNum % 2 == 0)
                    {
                        gameObject.GetComponent<SpriteRenderer>().gameObject.SetActive(hit);
                        hit = !hit;
                    }
                    hitNum++;
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
        GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().currentScene = GameManager.SceneState.Win;
        NextLevel();
    }
    public void GameOver()
    {
        gameText.text = "Game Over!";
        subGameText.text = "Your Score: " + score;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().currentScene = GameManager.SceneState.GameOver;
        gameText.gameObject.SetActive(true);
        subGameText.gameObject.SetActive(true);
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
    public void NextLevel()
    {
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<InvaderSpawner>().level++;
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<InvaderSpawner>().SpawnInvaders(3, 7);
        hp += 3;
        foreach (GameObject barrier in GameObject.FindGameObjectsWithTag("Wall"))
        {
            barrier.GetComponent<BarrierScript>().ResetBarrier();
        }
    }
    public void ScoreUp()
    {
        score += 10;
    }
    public void ScoreUp(int num)
    {
        score += num;
    }
    void ShowHealth()
    {
        life1.gameObject.SetActive(true);
        life2.gameObject.SetActive(true);
        life3.gameObject.SetActive(true);
        life4.gameObject.SetActive(true);
        life5.gameObject.SetActive(true);
        switch ((int)hp)
        {
            case 5:
                break;
            case 4:
                life5.gameObject.SetActive(false);
                break;
            case 3:
                life5.gameObject.SetActive(false);
                life4.gameObject.SetActive(false);
                break;
            case 2:
                life5.gameObject.SetActive(false);
                life4.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                break;
            case 1:
                life5.gameObject.SetActive(false);
                life4.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                life2.gameObject.SetActive(false);
                break;
            case 0:
                life5.gameObject.SetActive(false);
                life4.gameObject.SetActive(false);
                life3.gameObject.SetActive(false);
                life2.gameObject.SetActive(false);
                life1.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
