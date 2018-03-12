using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    public int ballAmount = 5;
    public float points = 0;
    public Text pointText;
    float timeStamp;
    bool thing = false;
    private void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        DontDestroyOnLoad(transform.gameObject);
    }
    // Use this for initialization
    void Start()
    {
        Respawn(30);
    }

    // Update is called once per frame
    void Update()
    {
        if (ballAmount > 0)
        {
            Respawn(-20);
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                SceneManager.LoadScene(2);
            }
        }
        if (thing)
        {
            if (timeStamp < Time.time)
            {
                ballAmount++;
                Respawn(100);
                thing = false;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            pointText.text = "Pts: " + points * 100;
        }
    }
    #region Respawn
    void Respawn(float limit)
    {
        if(transform.position.y < limit)
        {
            transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().rotation = 0;
            if (ballAmount < 2)
            {
                Match();
            }
            ballAmount--;
            //Instantiate(Resources.Load("Ball"), GameObject.FindGameObjectWithTag("Spawn").transform.position, Quaternion.identity);
        }
    }
    #endregion
    #region Match
    void Match()
    {
        if(points % 10 == 2)
        {
            MoarBalls();
        }
    }
    #endregion
    #region Balls & Points
    void MoarBalls()
    {
        ballAmount++;
    }
    void MoarBalls(int num)
    {
        ballAmount += num;
    }
    void GetPoints(int pts)
    {
        points += pts;
    }
    #endregion
    #region Collide
    void Collide(string tag)
    {
        if (!thing)
        {
            switch (tag)
            {
                case "Outlane":
                    thing = true;
                    ballAmount += 1;
                    timeStamp = Time.time + 5f;
                    break;
                case "Kickout":
                    break;
                default:
                    break;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collide(collision.gameObject.tag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collide(collision.tag);
    }
    #endregion
}
