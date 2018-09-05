using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text sceneText, subText, sizeText;
    public Slider sizeSlider;
    string conditionText;
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("PrefabPool").GetComponent<PrefabPool>().win)
        {
            sceneText.text = "Congratulations, you win!";
            conditionText = "You made it past - Wave ";
        }
        else
        {
            sceneText.text = "Game Over";
            conditionText = "You made it to - Wave ";
        }
        subText.text = conditionText + (GameObject.Find("PrefabPool").GetComponent<PrefabPool>().waveNum - 1);
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
