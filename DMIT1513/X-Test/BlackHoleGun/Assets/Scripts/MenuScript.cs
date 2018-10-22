using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Text musicText, soundText;
    [SerializeField]
    Slider musicSlider, soundSlider;
    [SerializeField]
    GameObject optionsMenu, pauseMenu, startMenu;
    // Use this for initialization
    void Start()
    {
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClosePauseMenu();
        }
        if (CheckMusicSoundText())
        {
            UpdateMusicSoundText();
        }
    }

    #region Open/Close Menu
    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
    }
    public void CloseStartMenu()
    {
        startMenu.SetActive(false);
    }
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        CloseOptions();
    }
    public void OpenClosePauseMenu()
    {
        if (!pauseMenu.active)
        {
            OpenPauseMenu();
        }
        else
        {
            ClosePauseMenu();
        }
    }
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
    #endregion

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        CloseStartMenu();
        CloseOptions();
    }
    public void CustomLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void UpdateMusicSoundText()
    {
        musicText.text = "Music: " + musicSlider.value + "%";
        soundText.text = "Sound: " + soundSlider.value + "%";
    }
    public bool CheckMusicSoundText()
    {
        return musicText.isActiveAndEnabled && musicSlider.isActiveAndEnabled && soundSlider.isActiveAndEnabled && soundText.isActiveAndEnabled;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
