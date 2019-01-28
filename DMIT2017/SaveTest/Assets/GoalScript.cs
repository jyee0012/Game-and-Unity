using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

    PlayerControl player;
    PlayerDataScript playerData;
    [SerializeField] int sceneLoad = 2;

    [SerializeField]
    GameObject ghostMenu, mainMenu;
    [SerializeField]
    Text highscoreText;
    // Use this for initialization
    void Start ()
    {
        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerControl>();
        if (playerData == null && GameObject.Find("PlayerData") != null) playerData = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>();
        if (ghostMenu != null) ghostMenu.SetActive(false);
        if (mainMenu != null) mainMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (playerData != null && highscoreText.text != playerData.playerData.timeText) highscoreText.text = "Record Time: " + playerData.playerData.timeText;
    }
    public void CustomLoadScene(int sceneIndex)
    {
        MainMenu(false);
        SceneManager.LoadScene(sceneIndex);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            MainMenu(true);
            if (playerData.CheckGhostData())
            {
                ghostMenu.SetActive(true);
            }
            else
            {
                OverwriteGhostData();
            }
        }
    }
    public void ConfirmGhostDataEdit(bool overwrite)
    {
        if (overwrite) OverwriteGhostData();
        ghostMenu.SetActive(false);
    }
    void MainMenu(bool active)
    {
        Time.timeScale = (active) ? 0 : 1;
        Time.fixedDeltaTime = (active) ? 0 : 0.02f;
        mainMenu.SetActive(active);
    }
    void OverwriteGhostData()
    {
        playerData.RetrieveData();
    }
}
