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
    Text musicText, soundText, masterText, bgmToggleText, controlText, controlToggleText;
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
    bool bgmOn = false, showControls = true;
    public bool useTimeScale = true, startBgmOn = true, hasBgm = true, startCursor = false, useDefaultSound = false, useGlobalVolume = false, useMixerSettings = true, alwaysOnCursor = false;
    [SerializeField]
    float masterVolume = 80, musicVolume = 80, soundVolume = 80;

    // Use this for initialization
    void Start()
    {
        if (optionsMenu != null) optionsMenu.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        SetCursor(startCursor);
        if (useMixerSettings) GetMixerVolume();
        if (useDefaultSound) SetVolumeSettings(masterVolume, musicVolume, soundVolume, useGlobalVolume);
        if (hasBgm)
        {
            bgmArray[0].Play();
            ToggleBGM();
            if (!startBgmOn)
            {
                ToggleBGM();
            }
        }
        if (controlText != null)
        {
            ToggleControlText(showControls);
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

        //Debug.Log(mixer.name);
    }
    public void UpdateText(Text text, Slider slider, string title)
    {
        if (text == null || slider == null) return;

        text.text = title + ": " + slider.value + "%";
    }
    public bool CheckMusicSoundText()
    {
        if (musicSlider == null || musicText == null || soundSlider == null || soundText == null) return false;
        return (musicText.gameObject.activeInHierarchy || musicSlider.gameObject.activeInHierarchy || 
            soundText.gameObject.activeInHierarchy || soundSlider.gameObject.activeInHierarchy) 
            || 
            (musicText.isActiveAndEnabled && musicSlider.isActiveAndEnabled && 
            soundSlider.isActiveAndEnabled && soundText.isActiveAndEnabled);
    }
    void ToggleControlText(bool show = true)
    {
        controlText.gameObject.SetActive(show);
        if(controlToggleText != null) controlToggleText.text = (show) ? "Hide Controls":"Show Controls";
    }
    public void ToggleShowControls()
    {
        showControls = !showControls;
        ToggleControlText(showControls);
    }
    #endregion
    #region Pause
    public void PauseEverything(bool pause)
    { 
        //if (!pauseMenu.activeInHierarchy && pauseMenu != null) return;
        if (useTimeScale)
        {
            //Debug.Log(Time.timeScale + ":" + Time.fixedDeltaTime);
            Time.timeScale = (pause) ? 0 : 1;
            Time.fixedDeltaTime = (pause) ? 0 : 0.02f;
        }
    }
    #endregion
    #region BGM
    public void GetMixerVolume()
    {
        float masterValue, musicValue, soundValue;
        mixer.GetFloat("MasterVolume", out masterValue);
        mixer.GetFloat("MusicVolume", out musicValue);
        mixer.GetFloat("SoundVolume", out soundValue);
        masterSlider.value = masterValue;
        musicSlider.value = musicValue;
        soundSlider.value = soundValue;
    }
    public void SetVolumeSettings(float master = 80, float music = 80, float sound = 80, bool useGlobal = true)
    {
        float useVolume = 0;
        if (useGlobal)
        {
            useVolume = master;
            masterSlider.value = useVolume;
        }
        else
        {
            masterSlider.value = master;
            musicSlider.value = music;
            soundSlider.value = sound;
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

        if (bgmOn) bgmArray[bgmIndex].Play();
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

        if (cursorOn || alwaysOnCursor)
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
