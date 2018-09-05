using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {

	public void OnClick()
    {
        //clear current Scene

        //load new Scene
        Debug.Log("Starting new game...");
        SceneManager.LoadScene("Main_Scene");
    }
}
