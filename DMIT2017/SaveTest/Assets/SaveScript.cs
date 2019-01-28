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
        colorIndex, //[0] Blue ,[1] Red ,[2] Yellow, [3] Purple
        saveIndex;
    public float time;
    public List<float> vInputGhost, hInputGhost;
    public List<Vector3> posList;
    public string timeText
    {
        get
        {
            if (time > 0)
                return string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
            else
                return "N/A";
        }
    }
    public bool hasGhostData
    {
        get
        {
            return (hInputGhost.Count > 0 && vInputGhost.Count > 0);
        }
    }

    public SaveData()
    {
        name = "noone";
        score = 0;
        shapeIndex = 0;
        colorIndex = 0;
        time = 0;
        vInputGhost = new List<float>();
        hInputGhost = new List<float>();
        posList = new List<Vector3>();
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
    public void ClearGhostData()
    {
        vInputGhost = new List<float>();
        hInputGhost = new List<float>();
        posList = new List<Vector3>();
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
    Button btnPrefab, createNewProfileBtn, deleteGhostBtn;
    [SerializeField]
    Dropdown shapeDropdown, colorDropdown;
    [SerializeField]
    GameObject playerDataPrefab;
    [SerializeField]
    List<Text> highscoreList;

    List<Button> profileBtns;
    List<SaveData> recordHolderList;

    // Use this for initialization
    void Start()
    {
        allData = new SaveContainer();
        myData = new SaveData();
        profileBtns = new List<Button>();
        recordHolderList = new List<SaveData>();
        LoadData();
        DisplayConfirmDelete(false);
        if (createNewProfileBtn != null) createNewProfileBtn.gameObject.SetActive(allData.saveDatas.Count < 10);
        NewData();
        deleteGhostBtn.gameObject.SetActive(myData.hasGhostData);
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
        float horizontalDisplacement = 91.5f; //91.5f, 71.8f
        for (int i = 0; i < allData.saveDatas.Count; i++)
        {
            Vector3 newBtnPos = new Vector3(horizontalDisplacement, SaveContainer.beginningHeight - (SaveContainer.buttonSpacing * i) - 150, 0);
            profilebtn = Instantiate(btnPrefab, btnPrefab.transform.position, btnPrefab.transform.rotation, profileContainer.transform);
            profilebtn.GetComponentInChildren<Text>().text = allData.saveDatas[i].GetName();
            allData.saveDatas[i].saveIndex = i;
            profilebtn.GetComponent<RectTransform>().localPosition = newBtnPos;
            if (allData.saveDatas[i].time > 0)
            {
                recordHolderList.Add(allData.saveDatas[i]);
            }
            //profilebtn.onClick.AddListener(() => { FillLoadedData(i); });
            #region Stupid Switch
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
            #endregion
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
        recordHolderList.Clear();
        RemoveProfileBtns();
        CreateLoadProfileBtn();
        FillLeaderBoard();
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
        scoreText.text = "Record Time: " + myData.timeText;
        shapeDropdown.value = myData.shapeIndex;
        colorDropdown.value = myData.colorIndex;
        deleteGhostBtn.gameObject.SetActive(myData.hasGhostData);
        if (scoreSlider != null)
        {
            scoreSlider.value = myData.GetScore();
        }
        //Debug.Log(myData.name);
    }
    public void SaveData()
    {
        AddNewData();
        SaveXml();
        LoadData();
        if (createNewProfileBtn != null) createNewProfileBtn.gameObject.SetActive(allData.saveDatas.Count < 10);
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
        scoreText.text = "Record Time: " + myData.timeText;
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
    public void ImportDataAt(SaveData importedData, int saveIndex)
    {
        allData.saveDatas[saveIndex] = importedData;
        SaveData();
    }
    public void DeleteGhost()
    {
        myData.ClearGhostData();
        SaveXml();
        FillLoadedData(myData.saveIndex);
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
        scoreText.text = "Record Time: ";
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

        if (myData == null || myData == new SaveData()) return;
        CreatePlayerData();
        SceneManager.LoadScene(1);
        //CloseStartMenu();
        //CloseOptions();
    }
    public void CreatePlayerData()
    {
        if (GameObject.Find("PlayerData") != null)
        {
            Destroy(GameObject.Find("PlayerData"));
        }

        if (playerDataPrefab == null) playerDataPrefab = Resources.Load<GameObject>("Prefab/PlayerData") as GameObject;
        GameObject playerData = Instantiate(playerDataPrefab, Vector3.zero, Quaternion.identity, null);
        playerData.name = "PlayerData";
        playerData.GetComponent<PlayerDataScript>().playerData = myData;
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
    void FillLeaderBoard()
    {
        recordHolderList.Sort(delegate(SaveData save1, SaveData save2) { return save1.time.CompareTo(save2.time); });
        for (int i = 0; i < highscoreList.Count; i++)
        {
            if (recordHolderList.Count > i)
            {
                highscoreList[i].text = i + 1 + ") " + recordHolderList[i].GetName() + " - Time: " + recordHolderList[i].timeText;
            }
            else
            {
                highscoreList[i].text = i + 1 + ") " + "N/A";
            }
        }
    }
    string TimeFormat(float time)
    {
        return string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
    }
}
