using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerData playerData = null;
    [SerializeField]
    float moveSpeed = 2f;

    [Space]
    public bool inDungeon = false;
    [SerializeField]
    Text playerNameText = null, playerHealthText = null, playerScoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
