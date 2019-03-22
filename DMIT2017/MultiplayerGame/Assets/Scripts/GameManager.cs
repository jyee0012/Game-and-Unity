using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Controls")]
    [SerializeField]
    string text = "";
    [SerializeField]
    bool win = false, lose = false, checkForWin = true;

    [Header("Menu Manager")]
    [SerializeField]
    bool canPause = false;
    [SerializeField]
    KeyCode pauseKey = KeyCode.Escape;
    [SerializeField]
    GameObject mainMenu = null, pauseMenu = null, optionsMenu = null, endMenu = null;
    [Space]
    [SerializeField]
    Text endGameText = null;
    [SerializeField]
    Button endGameBtn = null;

    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        if (endGameText != null) endGameText.text = "";
        if (endGameBtn != null) endGameBtn.enabled = false;
        if (pauseMenu != null) pauseMenu.SetActive(pause);
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (endMenu != null) endMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkForWin)
        {
            CheckWin();
            CheckLose();
            if (win || lose) EndGame();
        }

        if (canPause && Input.GetKeyDown(pauseKey)) Pause();
    }
    void CheckWin()
    {
        int emptyCount = 0;
        foreach(SpawnerScript spawner in FindObjectsOfType<SpawnerScript>())
        {
            if (spawner.outOfSpawns) emptyCount++;
        }
        win = (emptyCount >= FindObjectsOfType<SpawnerScript>().Length) ;
    }
    void CheckLose()
    {
        lose = FindObjectsOfType<PlayerController>().Length <= 0;
    }
    void EndGame()
    {
        if (endMenu != null) endMenu.SetActive(true);
        UpdateEndGameText();
        if (endGameBtn != null) endGameBtn.enabled = true;
    }
    void UpdateEndGameText()
    {
        if (endGameText != null)
        {
            if (win) endGameText.text = "Survivors Win!";
            else if (lose)
            {
                endGameText.text = "Zombies Win!";
                //if (FindObjectOfType<SpawnerScript>().playerControlled) endGameText.text = "";
                //else endGameText.text = "";
            }
        }
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void Pause()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        if (pauseMenu != null) pauseMenu.SetActive(pause);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
