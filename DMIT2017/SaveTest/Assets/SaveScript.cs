using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class SaveData
{
    public string name;
    public string description;
    public int score;

    public SaveData()
    {
        name = "none";
        score = 0;
    }
    public string GetName()
    {
        return name;
    }
    public void SetName(string value)
    {
        name = value;
    }
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int value)
    {
        score = value;
    }
}
[System.Serializable]
public class SaveContainer
{
    public List<SaveData> saveDatas;
    public static float beginningHeight = 130, buttonSpacing = 35;
}

public class SaveScript : MonoBehaviour {

    SaveContainer allData;
    SaveData myData;

    [SerializeField]
    Text scoreText;
    [SerializeField]
    InputField nameField;
    [SerializeField]
    Slider scoreSlider;
    [SerializeField]
    GameObject profileContainer;
    List<Button> profileBtns;
    [SerializeField]
    Button btnPrefab;

    // Use this for initialization
    void Start () {
        allData = new SaveContainer();
        myData = new SaveData();
        LoadData();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CreateLoadProfileBtn()
    {
        Button profilebtn;
        for (int i = 0; i < allData.saveDatas.Count; i++)
        {
            profilebtn = Instantiate(btnPrefab, btnPrefab.transform.position,btnPrefab.transform.rotation, profileContainer.transform);
            profilebtn.GetComponentInChildren<Text>().text = allData.saveDatas[i].GetName();
            profilebtn.GetComponent<RectTransform>().localPosition = new Vector3(0, SaveContainer.beginningHeight - (SaveContainer.buttonSpacing * i), 0);
            profileBtns.Add(profilebtn);
        }
    }
    public void LoadData()
    {
        if (File.Exists("Data.xml"))
        {
            Stream stream = File.Open("Data.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveContainer));
            allData = serializer.Deserialize(stream) as SaveContainer;
            stream.Close();
            if (allData.saveDatas.Count <= 0)
            {
                return;
            }
            myData = allData.saveDatas[0];
            nameField.text = myData.GetName();
            scoreText.text = "Score: " + myData.GetScore();
            if (scoreSlider != null)
            {
                scoreSlider.value = myData.GetScore();
            }
        }
        CreateLoadProfileBtn();

    }
    public void SaveData()
    {
        AddNewData();

        Stream stream = File.Open("Data.xml", FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(SaveContainer));
        serializer.Serialize(stream, allData);
        stream.Close();
    }

    public void ChangeName(string value)
    {
        myData.SetName(value);
    }
    public void GetName()
    {
        ChangeName(nameField.text);
    }
    public void ChangeScore(int value)
    {
        myData.SetScore(value);
        scoreText.text = "Score: " + myData.GetScore();
    }
    public void ChangeScoreSlider()
    {
        ChangeScore(Mathf.RoundToInt(scoreSlider.value));
    }
    public void AddNewData()
    {
        if (!allData.saveDatas.Contains(myData))
        {
            allData.saveDatas.Add(myData);
        }
    }
    public void NewData()
    {
        myData = new SaveData();
        nameField.text = "";
        scoreSlider.value = 0;
        scoreText.text = "Score";
    }

}
