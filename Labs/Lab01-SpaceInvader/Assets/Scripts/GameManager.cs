using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public Text bigMsg;
    public Button playButton, quitButton;
    public GameObject Spawner;
    public enum SceneState { Start, Playing, Win, GameOver, Quit}
    public SceneState currentScene = SceneState.Start;
	// Use this for initialization
	void Start () {
        playButton = GameObject.FindGameObjectsWithTag("Buttons")[1].GetComponent<Button>();
        quitButton = GameObject.FindGameObjectsWithTag("Buttons")[0].GetComponent<Button>();
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
                ShowButtons(true);
                break;
            case SceneState.Playing:
                ShowButtons(false);
                bigMsg.text = "";
                break;
            case SceneState.Win:
                currentScene = SceneState.Playing;
                break;
            case SceneState.GameOver:
                ShowButtons(true);
                playButton.GetComponentInChildren<Text>().text = "Try Again";
                quitButton.GetComponentInChildren<Text>().text = "RageQuit";
                bigMsg.text = "";
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
    void ShowButtons(bool show)
    {
        if (playButton != null && quitButton != null)
        {
            if (show)
            {
                playButton.GetComponent<Button>().enabled = true;
                quitButton.GetComponent<Button>().enabled = true;
                playButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
            }
            else
            {
                playButton.GetComponent<Button>().enabled = false;
                quitButton.GetComponent<Button>().enabled = false;
                playButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
            }
        }
    }
}
