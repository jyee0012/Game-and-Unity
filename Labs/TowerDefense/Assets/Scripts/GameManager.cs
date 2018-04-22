using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text sceneText;
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
    }

    // Update is called once per frame
    void Update()
    {

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
