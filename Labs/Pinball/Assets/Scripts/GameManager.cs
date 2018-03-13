using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text ptText;
    GameObject ball;
    BallScript ballScript;
    public GameObject target1, target2, target3, block;
    // Use this for initialization
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballScript = ball.GetComponent<BallScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ptText != null)
        {
            ptText.text = "" + ballScript.points * 100;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Ball").Length; i++)
            {
                Destroy(GameObject.FindGameObjectWithTag("Ball"));
            }
        }
        if (target1 != null && target2 != null && target3 != null && block != null )
        {
            RemoveBlock();
        }
    }
    #region Button Functions
    public void LoadGameSpace(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
    #endregion
    void RemoveBlock()
    {
        if (target1.GetComponent<TargetScript>().hit && target2.GetComponent<TargetScript>().hit && target3.GetComponent<TargetScript>().hit)
        {
            Destroy(block);
        }
    }
}
