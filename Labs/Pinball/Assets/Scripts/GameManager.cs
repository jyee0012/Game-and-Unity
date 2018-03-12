using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Text ptText;
    GameObject ball;
    BallScript ballScript;
	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballScript = ball.GetComponent<BallScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (ptText != null)
        {
            ptText.text = "" + ballScript.points * 100;
        }
	}
    #region Button Functions
    public void LoadGameSpace(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
    #endregion
}
