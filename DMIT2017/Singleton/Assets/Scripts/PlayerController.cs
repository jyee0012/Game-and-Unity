using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerData playerData = null;

    [SerializeField]
    Text playerNameText = null, playerHealthText = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdatePlayerText()
    {
        if (playerData == null) return;
        if (playerHealthText != null) playerHealthText.text = "PlayerHP: " + playerData.playerInfo.currentHp + "/" + playerData.playerInfo.maxHp;
        if (playerNameText != null) playerNameText.text = playerData.playerInfo.name;
    }
}
