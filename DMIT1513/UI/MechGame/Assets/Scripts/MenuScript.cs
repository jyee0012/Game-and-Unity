using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{

    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Text musicText, soundText, masterText, bgmToggleText;
    [SerializeField]
    Slider musicSlider, soundSlider, masterSlider;
    [SerializeField]
    GameObject optionsMenu, pauseMenu, startMenu;
    [SerializeField]
    AudioMixer mixer = null;
    [SerializeField]
    AudioSource[] bgmArray;
    [SerializeField]
    Dropdown bgmDropDown;
    bool bgmOn = false;
    public bool useTimeScale = true, startBgmOn = true, hasBgm = true, startCursor = false;

    // Use this for initialization
    void Start()
    {
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        SetCursor(startCursor);
        if (hasBgm)
        {
            ToggleBGM();
            if (!startBgmOn)
            {
                ToggleBGM();
            }
        }
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
            UpdateMusicSound();
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
        SetCursor(true);
        pauseMenu.SetActive(true);
        PauseEverything(true);
    }
    public void ClosePauseMenu()
    {
        PauseEverything(false);
        SetCursor(false);
        pauseMenu.SetActive(false);
        CloseOptions();
    }
    public void OpenClosePauseMenu()
    {
        if (!pauseMenu.activeInHierarchy)
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
    #region Game Control
    public void PlayGame()
    {
        PauseEverything(false);
        SceneManager.LoadScene(1);
        CloseStartMenu();
        CloseOptions();
    }
    public void CustomLoadScene(int sceneIndex)
    {
        PauseEverything(false);
        SceneManager.LoadScene(sceneIndex);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    #endregion
    #region Text
    public void UpdateMusicSound()
    {
        UpdateText(musicText, musicSlider, "Music");
        UpdateText(soundText, soundSlider, "Sound");
        UpdateText(masterText, masterSlider, "Master");
        mixer.SetFloat("MusicVolume", musicSlider.value - 80);
        mixer.SetFloat("SoundVolume", soundSlider.value - 80);
        mixer.SetFloat("MasterVolume", masterSlider.value - 80);

        Debug.Log(mixer.name);
    }
    public void UpdateText(Text text, Slider slider, string title)
    {
        text.text = title + ": " + slider.value + "%";
    }
    public bool CheckMusicSoundText()
    {
        return musicText.isActiveAndEnabled && musicSlider.isActiveAndEnabled && soundSlider.isActiveAndEnabled && soundText.isActiveAndEnabled;
    }
    #endregion
    #region Pause
    public void PauseEverything(bool pause)
    { 
        if (!pauseMenu.activeInHierarchy && pauseMenu != null) return;
        if (useTimeScale)
        {
            //Debug.Log(Time.timeScale + ":" + Time.fixedDeltaTime);
            Time.timeScale = (pause) ? 0 : 1;
            Time.fixedDeltaTime = (pause) ? 0 : 0.02f;
        }
    }
    #endregion
    #region BGM
    public void SetBGM()
    {
        int bgmIndex = bgmDropDown.value;
        foreach (AudioSource audio in bgmArray)
        {
            audio.Stop();
        }
        bgmArray[bgmIndex].Play();
        bgmArray[bgmIndex].Pause();
    }
    public void ToggleBGM()
    {
        bgmOn = !bgmOn;
        bgmToggleText.text = (bgmOn) ? "Off" : "On";
        foreach (AudioSource audio in bgmArray)
        {
            if (bgmOn)
            {
                audio.UnPause();
            }
            else
            {
                audio.Pause();
            }
        }
    }
    #endregion
    #region Cursor
    public void SetCursor(bool cursorOn = true)
    {
        if (cursorOn)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion
}
