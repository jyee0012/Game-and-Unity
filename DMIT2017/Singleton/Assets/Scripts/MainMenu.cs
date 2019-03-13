using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameController myController;

    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.FindObjectOfType<GameController>();
    }

    public void SetValue(float value)
    {
        myController.value = (int)value;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
