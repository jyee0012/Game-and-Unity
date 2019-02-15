using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour {

    [SerializeField]
    int playerCount = 0, goalScene = 0, totalPlayers = 0;
    [SerializeField]
    bool useGoal = true;
	// Use this for initialization
	void Start () {
        if (totalPlayers == 0) totalPlayers = FindAllPlayers().Count/2;
    }

    // Update is called once per frame
    void Update()
    {
        if (useGoal)
        {
            if (playerCount >= totalPlayers) LoadGoalScene();
        }
    }
    public void LoadCustomScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadGoalScene()
    {
        LoadCustomScene(goalScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    List<GameObject> FindAllPlayers(string nameTag = "Player")
    {
        List<GameObject> allPlayers = new List<GameObject>();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag(nameTag))
        {
            if (!allPlayers.Contains(player) && player.activeInHierarchy)
            {
                allPlayers.Add(player);
            }
        }
        return allPlayers;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCount++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCount--;
        }
    }
}
