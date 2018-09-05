using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    public enum Direction { LEFT, RIGHT, UP, DOWN, STOP }

    public GameObject StartScreen;

    public Text player1ScoreText;
    public Text player2ScoreText;

    private int player1Score;
    private int player2Score;

    public Text POOPS;

	// Use this for initialization
	void Start () {
        player1Score = 0;
        player2Score = 0;

        if (player1ScoreText)
            player1ScoreText.text = "" + player1Score;

        if (player2ScoreText)
            player2ScoreText.text = "" + player2Score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Score(GameObject player)
    {
        if (player.GetComponent<PlayerStats>().getID() == 1)
            player1Score += 1;
        else if (player.GetComponent<PlayerStats>().getID() == 2)
            player2Score += 1;
    }
}
