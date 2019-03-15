using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonScript : MonoBehaviour
{
    [SerializeField]
    int dungeonBuildIndex = 0, mapBuildIndex = 0;
    [SerializeField]
    bool entrance = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.GetComponent<PlayerController>() != null)
            {
                PlayerController player = other.transform.GetComponent<PlayerController>();
                if (entrance)
                {
                    if (!player.playerData.inDungeon)
                    {
                        player.playerData.EnterDungeon(player.transform.position);
                        SceneManager.LoadScene(dungeonBuildIndex);
                    }
                }
                else
                {
                    SceneManager.LoadScene(mapBuildIndex);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.GetComponent<PlayerController>() != null)
            {
                PlayerController player = other.transform.GetComponent<PlayerController>();
                if (entrance)
                {
                    if (player.playerData.inDungeon)
                    {
                        player.playerData.ExitDungeon();
                    }
                }
            }
        }
    }
}
