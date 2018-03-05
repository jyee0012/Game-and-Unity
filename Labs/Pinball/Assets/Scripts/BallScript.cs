using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    int ballAmount = 5, points;
    public Text ballText, pointText;
    // Use this for initialization
    void Start()
    {
        Respawn(30);
    }

    // Update is called once per frame
    void Update()
    {
        Respawn(-20);
    }
    void Respawn(float limit)
    {
        if(transform.position.y < limit)
        {
            transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ballAmount--;
            //Instantiate(Resources.Load("Ball"), GameObject.FindGameObjectWithTag("Spawn").transform.position, Quaternion.identity);
        }
    }
    void MoarBalls()
    {
        ballAmount++;
    }
    void MoarBalls(int num)
    {
        ballAmount += num;
    }
}
