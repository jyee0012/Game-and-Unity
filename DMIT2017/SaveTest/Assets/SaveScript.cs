using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

#region SaveDatas
[System.Serializable]
public class SaveData
{
    public string name, description;
    public int score, 
        shapeIndex, //[0] Cube ,[1] Sphere ,[2] Cylinder, [3] Capsule
        colorIndex; //[0] Blue ,[1] Red ,[2] Yellow, [3] Purple

    public SaveData()
    {
        name = "noone";
        score = 0;
        shapeIndex = 0;
        colorIndex = 0;
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
    public void SetShape(int value)
    {
        shapeIndex = value;
    }
    public void SetColor(int value)
    {
        colorIndex = value;
    }
}
[System.Serializable]
public class SaveContainer
{
    public List<SaveData> saveDatas;
    public static float beginningHeight = 130, buttonSpacing = 35;
}
#endregion

public class SaveScript : MonoBehaviour
{

    SaveContainer allData;
    SaveData myData;

    [SerializeField]
    Text scoreText, confirmText;
    [SerializeField]
    InputField nameField;
    [SerializeField]
    Slider scoreSlider;
    [SerializeField]
    GameObject profileContainer, confirmDeletePanel, startMenu, pauseMenu, optionsMenu;
    [SerializeField]
    Button btnPrefab, createNewProfileBtn;
    [SerializeField]
    Dropdown shapeDropdown, colorDropdown;

    List<Button> profileBtns;


    // Use this for initialization
    void Start()
    {
        allData = new SaveContainer();
        myData = new SaveData();
        profileBtns = new List<Button>();
        LoadData();
        DisplayConfirmDelete(false);
        if (createNewProfileBtn != null) createNewProfileBtn.gameObject.SetActive(allData.saveDatas.Count < 10);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Data Control
    #region Save & Load
    public void CreateLoadProfileBtn()
    {
        Button profilebtn;
        float horizontalDisplacement = 0f; //91.5f, 71.8f
        for (int i = 0; i < allData.saveDatas.Count; i++)
        {
            
            Vector3 newBtnPos = new Vector3(horizontalDisplacement, SaveContainer.beginningHeight - (SaveContainer.buttonSpacing * i) - 150, 0);
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
        if (horizontalDisplacement != 71.8f) horizontalDisplacement = 71.8f;
    }
    public void LoadData()
    {
        if (File.Exists("Data.xml"))
        {
            Stream stream = File.Open("Data.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveContainer));
            allData = serializer.Deserialize(stream) as SaveContainer;
            stream.Close();
        }
        RemoveProfileBtns();
        CreateLoadProfileBtn();
    }
    public void RemoveProfileBtns()
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
        shapeDropdown.value = myData.shapeIndex;
        colorDropdown.value = myData.colorIndex;
        if (scoreSlider != null)
        {
            scoreSlider.value = myData.GetScore();
        }
    }
    public void SaveData()
    {
        AddNewData();
        SaveXml();
        LoadData();
    }
    public void SaveXml()
    {
        Stream stream = File.Open("Data.xml", FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(SaveContainer));
        serializer.Serialize(stream, allData);
        stream.Close();
    }
    #endregion
    #region Change Data
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
    public void ChangeShape()
    {
        if (shapeDropdown == null) return;
        myData.SetShape(shapeDropdown.value);
    }
    public void ChangeColor()
    {
        if (colorDropdown == null) return;
        myData.SetColor(colorDropdown.value);
    }
    #endregion
    #region New Data
    public void AddNewData()
    {
        if (allData.saveDatas.Count >= 10) return;
        if (!allData.saveDatas.Contains(myData))
        {
            allData.saveDatas.Add(myData);
        }
    }
    public void NewData()
    {
        if (createNewProfileBtn != null) createNewProfileBtn.gameObject.SetActive(allData.saveDatas.Count < 10);
        myData = new SaveData();
        nameField.text = "";
        scoreSlider.value = 0;
        scoreText.text = "Score";
    }
    #endregion
    #region Delete Data
    public void DeleteData()
    {
        if (myData != null)
        {
            allData.saveDatas.Remove(myData);
            SaveXml();
            LoadData();
            NewData();
        }
        DisplayConfirmDelete(false);
    }
    public void DisplayConfirmDelete(bool showConfirm)
    {
        if (showConfirm)
        {
            confirmText.text = "Are you sure you wish to delete " + myData.GetName() + "?";
        }
        confirmDeletePanel.SetActive(showConfirm);
    }
    #endregion
    #endregion
    #region Open/Close Menu
    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
    }
    public void CloseStartMenu()
    {
        startMenu.SetActive(false);
    }
    public void OpenPauseMenu()
    {
        SetCursor(true);
        pauseMenu.SetActive(true);
        PauseEverything(true);
    }
    public void ClosePauseMenu()
    {
        PauseEverything(false);
        SetCursor(false);
        pauseMenu.SetActive(false);
        CloseOptions();
    }
    public void OpenClosePauseMenu()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            OpenPauseMenu();
        }
        else
        {
            ClosePauseMenu();
        }
    }
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
    #endregion
    #region Game Control
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        CloseStartMenu();
        CloseOptions();
    }
    public void CustomLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    #endregion
    #region Pause
    public void PauseEverything(bool pause)
    {
        //if (!pauseMenu.activeInHierarchy && pauseMenu != null) return;
        //Debug.Log(Time.timeScale + ":" + Time.fixedDeltaTime);
        Time.timeScale = (pause) ? 0 : 1;
        Time.fixedDeltaTime = (pause) ? 0 : 0.02f;
    }
    #endregion
    #region Cursor
    public void SetCursor(bool cursorOn = true)
    {

        if (cursorOn)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    #endregion
}
