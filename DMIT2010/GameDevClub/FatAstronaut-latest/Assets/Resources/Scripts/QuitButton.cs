using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

class QuitButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Quitting the Game...");
        Application.Quit();
    }
}