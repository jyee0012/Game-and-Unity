using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public Text bigMsg;
    public Button playButton, quitButton;
    public enum SceneState { Start, Playing, Win, GameOver, Quit}
    public SceneState currentScene = SceneState.Start;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        States();
	}
    void States()
    {
        switch(currentScene)
        {
            case SceneState.Start:
                bigMsg.text = "Detroit Vaders!";
                break;
            case SceneState.Playing:
                playButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
                bigMsg.text = "";
                break;
            case SceneState.Win:
                playButton.GetComponentInChildren<Text>().text = "Play Again?";
                quitButton.GetComponentInChildren<Text>().text = "Quit";
                break;
            case SceneState.GameOver:
                playButton.GetComponentInChildren<Text>().text = "Try Again";
                quitButton.GetComponentInChildren<Text>().text = "RageQuit";
                break;
            case SceneState.Quit:
                Application.Quit();
                break;
        }
    }
    public void LoadGameSpace(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ButtonQuit()
    {
        currentScene = SceneState.Quit;
    }
}
