using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheeseWin : MonoBehaviour {

    private void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            if (obj.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Goal)
                transform.position = obj.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag.Equals("Player"))
        {
            //the player wins!! YAY!
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LevelUp();

            SceneManager.LoadScene("Win_Scene");
        }
    }
}
