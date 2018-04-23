using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text sceneText, subText, sizeText;
    public Slider sizeSlider;
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("PrefabPool").GetComponent<PrefabPool>().win)
        {
            sceneText.text = "Congratulations, you win!";
        }
        else
        {
            sceneText.text = "Game Over";
        }
        subText.text = "You made it to - Wave " + GameObject.Find("PrefabPool").GetComponent<PrefabPool>().waveNum;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = sizeSlider.value;
        sizeText.text = "Screen Size: " + sizeSlider.value;
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
    public static void SLoadGameSpace(int index)
    {
        SceneManager.LoadScene(index);
    }
    public static void SButtonQuit()
    {
        Application.Quit();
    }
    #endregion
}
