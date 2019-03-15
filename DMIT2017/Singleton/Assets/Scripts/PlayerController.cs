using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData = null;
    [SerializeField]
    float moveSpeed = 2f;

    [Space]
    public List<GameObject> inventoryList = null;
    [SerializeField]
    Text playerNameText = null, playerHealthText = null, playerScoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        if (playerData != null)
        {
            if (playerData.inDungeon && SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2))
            {
                gameObject.transform.position = playerData.position;
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
        if (playerData == null) return;
        if (playerHealthText != null) playerHealthText.text = "PlayerHP: " + playerData.currentHp + "/" + playerData.maxHp;
        if (playerNameText != null) playerNameText.text = playerData.playerName;
        if (playerScoreText != null) playerScoreText.text = "Score: " + playerData.score;

        if (inventoryList.Count > 0)
        {
            for(int i = 0; i < playerData.inventory.Count; i++)
            {
                inventoryList[i].GetComponent<ItemUIScript>().currentItem = playerData.inventory[i].itemDetail;
                inventoryList[i].GetComponent<ItemUIScript>().itemAmount = playerData.inventory[i].itemAmount;
            }
        }
    }
    public void ResetPlayerScore()
    {
        if (playerData == null) return;
        playerData.ResetScore();
    }
    public void FullHealPlayer()
    {
        if (playerData == null) return;
        playerData.FullHeal();
    }
    public void ClearPlayerInventory()
    {
        if (playerData == null) return;
        playerData.ClearInventory();
        foreach (GameObject item in inventoryList)
        {
            item.GetComponent<ItemUIScript>().currentItem = null;
        }
    }
}
