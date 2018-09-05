using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{

    GameObject pinball, lifeBall;
    int ballCounter;
    GameObject[] ballList;
    // Use this for initialization
    void Start()
    {
        pinball = GameObject.FindGameObjectWithTag("Ball");
        lifeBall = Resources.Load("BallLife") as GameObject;
        ballCounter = pinball.GetComponent<BallScript>().ballAmount - 1;
        ballList = new GameObject[20];
        SpawnBalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballCounter != pinball.GetComponent<BallScript>().ballAmount -1)
        {
            ballCounter = pinball.GetComponent<BallScript>().ballAmount - 1;
            SpawnBalls();
        }
    }
    void SpawnBalls()
    {
        ClearBalls();
        for (int i = 0; i < ballCounter; i++)
        {
            ballList[i] = Instantiate(lifeBall, new Vector3(transform.position.x, transform.position.y + (i * 2), 0), Quaternion.identity, this.gameObject.transform);
        }
    }
    void ClearBalls()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }

}
