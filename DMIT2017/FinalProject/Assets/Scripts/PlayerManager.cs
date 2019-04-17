using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;
    [SerializeField]
    List<PlayerTextController> playerTextList = new List<PlayerTextController>();
    [SerializeField]
    List<GameObject> playerPaddleList = new List<GameObject>();
    public bool[] activePlayers;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            activePlayers = Instance.activePlayers;
            Destroy(Instance);
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayersThenPlayGame(int sceneIndex)
    {
        SetActivePlayers();
        LoadScene(sceneIndex);
    }
    void SetActivePlayers()
    {
        activePlayers = new bool[playerTextList.Count];
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
