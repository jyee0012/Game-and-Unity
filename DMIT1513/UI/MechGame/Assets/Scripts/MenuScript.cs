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
    public bool useTimeScale = true, startBgmOn = true;
    // Use this for initialization
    void Start()
    {
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        ToggleBGM();
        if (!startBgmOn)
        {
            ToggleBGM();
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
        pauseMenu.SetActive(true);
        PauseEverything(true);
    }
    public void ClosePauseMenu()
    {
        PauseEverything(false);
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
    public void UpdateMusicSound()
    {
        UpdateText(musicText, musicSlider, "Music");
        UpdateText(soundText, soundSlider, "Sound");
        UpdateText(masterText, masterSlider, "Master");
        mixer.SetFloat("Music", musicSlider.value - 80);
        mixer.SetFloat("Sound", soundSlider.value - 80);
        mixer.SetFloat("Master", masterSlider.value - 80);

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
    public void CloseGame()
    {
        Application.Quit();
    }
    public void PauseEverything(bool pause)
    { 
        if (!pauseMenu.activeInHierarchy && pauseMenu != null) return;
        if (useTimeScale)
        {
            //Debug.Log(Time.timeScale + ":" + Time.fixedDeltaTime);
            Time.timeScale = (pause) ? 0.0001f : 1;
            Time.fixedDeltaTime = (pause) ? 0.0001f : 0.02f;
        }
    }
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
}
