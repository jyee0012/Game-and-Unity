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

public class SaveScript : MonoBehaviour
{

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
    void Start()
    {
        allData = new SaveContainer();
        myData = new SaveData();
        profileBtns = new List<Button>();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateLoadProfileBtn()
    {
        Button profilebtn;
        for (int i = 0; i < allData.saveDatas.Count; i++)
        {
            Vector3 newBtnPos = new Vector3(91.5f, SaveContainer.beginningHeight - (SaveContainer.buttonSpacing * i) - 150, 0);
            profilebtn = Instantiate(btnPrefab, btnPrefab.transform.position, btnPrefab.transform.rotation, profileContainer.transform);
            profilebtn.GetComponentInChildren<Text>().text = allData.saveDatas[i].GetName();
            profilebtn.GetComponent<RectTransform>().localPosition = newBtnPos;
            //profilebtn.onClick.AddListener(() => { FillLoadedData(i); });
            switch (i)
            {
                case 0:
                    profilebtn.onClick.AddListener(delegate { FillLoadedData(0); });
                    break;
                case 1:
                    profilebtn.onClick.AddListener(() => FillLoadedData(1));
                    break;
                case 2:
                    profilebtn.onClick.AddListener(() => FillLoadedData(2));
                    break;
                case 3:
                    profilebtn.onClick.AddListener(() => FillLoadedData(3));
                    break;
                case 4:
                    profilebtn.onClick.AddListener(() => FillLoadedData(4));
                    break;
                case 5:
                    profilebtn.onClick.AddListener(() => FillLoadedData(5));
                    break;
                case 6:
                    profilebtn.onClick.AddListener(() => FillLoadedData(6));
                    break;
                case 7:
                    profilebtn.onClick.AddListener(() => FillLoadedData(7));
                    break;
                case 8:
                    profilebtn.onClick.AddListener(() => FillLoadedData(8));
                    break;
                case 9:
                    profilebtn.onClick.AddListener(() => FillLoadedData(9));
                    break;
                case 10:
                    profilebtn.onClick.AddListener(() => FillLoadedData(10));
                    break;
            }
            //Debug.Log(newBtnPos);
            profileBtns.Add(profilebtn);
        }
        for (int i = 0; i < profileBtns.Count; i++)
        {
            //profileBtns[i].onClick.AddListener(() => FillLoadedData(i));
        }
    }
    public void LoadData()
    {
        if (profileBtns.Count > 0)
        {
            foreach (Transform child in profileContainer.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject != profileContainer.gameObject)
                {
                    Destroy(child.gameObject);
                }
            }
            profileBtns.Clear();
        }
        if (File.Exists("Data.xml"))
        {
            Stream stream = File.Open("Data.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveContainer));
            allData = serializer.Deserialize(stream) as SaveContainer;
            stream.Close();
        }
        CreateLoadProfileBtn();

    }
    public void ClickButton(string input = "")
    {
        if (input == "") Debug.Log(gameObject.name);
        else Debug.Log(input);

        FillLoadedData();
    }
    public void FillLoadedData(int loadedIndex = 0)
    {
        //Debug.Log("I clicked on " + loadedIndex + " button.");
        if (allData.saveDatas.Count <= 0)
        {
            return;
        }
        myData = allData.saveDatas[loadedIndex];
        nameField.text = myData.GetName();
        scoreText.text = "Score: " + myData.GetScore();
        if (scoreSlider != null)
        {
            scoreSlider.value = myData.GetScore();
        }
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
