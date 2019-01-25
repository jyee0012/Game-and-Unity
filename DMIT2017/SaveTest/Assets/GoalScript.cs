using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour {

    PlayerControl player;
    [SerializeField] int sceneLoad = 2;
    // Use this for initialization
    void Start ()
    {
        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    void CustomLoadScene(int sceneIndex)
    {
        PlayerDataScript playerData = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>();
        playerData.RetrieveData();
        SceneManager.LoadScene(sceneIndex);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            CustomLoadScene(sceneLoad);
        }
    }
}
