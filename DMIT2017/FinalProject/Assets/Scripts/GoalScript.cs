using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField]
    PaddleScript playerPaddle = null;
    [SerializeField]
    AudioSource destroySound = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            UpdatePlayerLives();
            if (other.GetComponent<PongBallScript>() != null)
            {
                UpdatePlayerScore(other.GetComponent<PongBallScript>());
            }
            if (destroySound != null) destroySound.PlayOneShot(destroySound.clip);
        }
    }
    void UpdatePlayerLives()
    {
        if (playerPaddle == null) return;
        if (playerPaddle.useLives)
        {
            playerPaddle.LoseLives();
        }
    }
    void UpdatePlayerScore(PongBallScript ball = null)
    {
        if (ball == null || !playerPaddle.useScore) return;
        if (ball.lastPlayerHit != null && ball.lastPlayerHit.GetComponent<PaddleScript>() != null)
        {
            ball.lastPlayerHit.GetComponent<PaddleScript>().GainScore();
        }
    }
}
