using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    List<PlayerTextController> playerTextList = new List<PlayerTextController>();
    bool[] activePlayers;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        activePlayers = new bool[playerTextList.Count];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetActivePlayers()
    {
        int activePlayerCount = 0;
        for(int i = 0; i < playerTextList.Count; i++)
        {
            activePlayers[i] = playerTextList[i].activePlayer;
            if (playerTextList[i].activePlayer)
            {
                activePlayerCount++;
            }
        }
        Debug.Log("Active Players: " + activePlayerCount);
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
