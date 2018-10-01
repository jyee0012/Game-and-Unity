using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectiveController : MonoBehaviour {

    [SerializeField]
    GameObject[] objectives;
    Vector3[] objstartPos;

    float objectiveCount;
    [SerializeField]
    Text objText;
    [SerializeField]
    bool useTags = false;
	// Use this for initialization
	void Start () {
        if (useTags)
        {
            objectives = GameObject.FindGameObjectsWithTag("Objective");
        }
        objstartPos = new Vector3[objectives.Length];
        for (int i = 0; i < objectives.Length;i++)
        {
            objstartPos[i] = objectives[i].transform.position;
        }
        objectiveCount = objectives.Length;

    }
	
	// Update is called once per frame
	void Update () {
        CheckAllObjectives();
        UpdateObjText();
    }
    void CheckAllObjectives()
    {
        objectiveCount = objectives.Length;
        foreach (GameObject obj in objectives)
        {
            if (!CheckMove(obj.transform.position))
            {
                objectiveCount--;
            }

        }
        if (objectiveCount <= 0)
        {
            LoadNextScene();
        }
    }

    bool CheckMove(Vector3 startPos)
    {
        if (startPos != transform.position)
        {
            return true;
        }
        return false;
    }
    void LoadNextScene()
    {
        //GetComponent<LevelChanger>().RandomLevel();
        Debug.Log("test");
    }
    void UpdateObjText()
    {
        objText.text = "Cubes: " + objectiveCount + "/" + objectives.Length;
    }
}
