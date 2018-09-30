using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }
    public void UpdateMusicSoundText()
    {
        musicText.text = "Music: " + musicSlider.value + "%";
        soundText.text = "Sound: " + soundSlider.value + "%";
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
