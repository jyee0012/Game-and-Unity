using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataScript : MonoBehaviour {

    public SaveData playerData = new SaveData();
    PlayerControl player;
    
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) player = GameObject.Find("Player").GetComponent<PlayerControl>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) && playerData.time > 0) ImportData();
    }
    public void RetrieveData()
    {
        playerData.hInputGhost = player.hInputGhost;
        playerData.vInputGhost = player.vInputGhost;
        playerData.posList = player.posList;
        playerData.time = player.timer;
    }
    public void ImportData()
    {
        SaveScript saveScript = GameObject.Find("Canvas").GetComponent<SaveScript>();
        saveScript.ImportDataAt(playerData, playerData.saveIndex);
    }
}
