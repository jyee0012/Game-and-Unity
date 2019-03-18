using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class TempData
{
    public PlayerData playerData = null;
    public TempData()
    {
        playerData = null;
    }
}

public class PlayerController : MonoBehaviour
{
    public TempData tempData = new TempData();
    [SerializeField]
    float moveSpeed = 2f;

    [Space]
    public List<GameObject> inventoryList = null;
    [SerializeField]
    Text playerNameText = null, playerHealthText = null, playerScoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        if (tempData.playerData != null)
        {
            if (tempData.playerData.inDungeon && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
            {
                gameObject.transform.position = tempData.playerData.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        UpdatePlayerText();
    }
    void Movement()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed));
    }
    void UpdatePlayerText()
    {
        if (tempData.playerData == null) return;
        if (playerHealthText != null) playerHealthText.text = "PlayerHP: " + tempData.playerData.currentHp + "/" + tempData.playerData.maxHp;
        if (playerNameText != null) playerNameText.text = tempData.playerData.playerName;
        if (playerScoreText != null) playerScoreText.text = "Score: " + tempData.playerData.score;

        if (inventoryList.Count > 0)
        {
            for(int i = 0; i < tempData.playerData.inventory.Count; i++)
            {
                inventoryList[i].GetComponent<ItemUIScript>().currentItem = tempData.playerData.inventory[i].itemDetail;
                inventoryList[i].GetComponent<ItemUIScript>().itemAmount = tempData.playerData.inventory[i].itemAmount;
            }
        }
    }
    public void ResetPlayerScore()
    {
        if (tempData.playerData == null) return;
        tempData.playerData.ResetScore();
    }
    public void FullHealPlayer()
    {
        if (tempData.playerData == null) return;
        tempData.playerData.FullHeal();
    }
    public void ClearPlayerInventory()
    {
        if (tempData.playerData == null) return;
        tempData.playerData.ClearInventory();
        foreach (GameObject item in inventoryList)
        {
            item.GetComponent<ItemUIScript>().currentItem = null;
        }
    }
    public void SavePlayerData()
    {
        SaveJson("PlayerData.json");
    }
    public void LoadPlayerData()
    {
        LoadJson("PlayerData.json");
    }
    public void SaveJson(string filePath)
    {
        string json = JsonUtility.ToJson(tempData);
        File.WriteAllText(filePath, json);
    }
    public void LoadJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tempData = JsonUtility.FromJson<TempData>(json);
        }
    }
}
