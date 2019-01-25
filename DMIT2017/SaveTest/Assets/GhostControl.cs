using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : PlayerControl {

    PlayerControl player;
    PlayerDataScript playerData;
    public bool bHasGhostData = true, bGhostDone = false;
    int currentInputIndex = 0, currentPosIndex;
    [SerializeField]
    float timeDelay = 10f;
	// Use this for initialization
	void Start () {
        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerControl>();
        if (playerData == null) playerData = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>();
        rbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        posTimer = posResetTimer + timeDelay;
        bHasGhostData = (playerData.playerData.hInputGhost.Count > 0 && playerData.playerData.vInputGhost.Count > 0);
        if (bHasGhostData)
        {
            GetGhostData(playerData.playerData);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeDelay && !bGhostDone)
        {
            if (!bGrounded && jumpDelay < Time.time)
            {
                ground = transform.position;
                ground.y -= 1f;
                if (bGrounded = CheckGround(ground)) jumpCount = 0;
            }
            GhostMovement();
            if (Time.time > posTimer)
            {
                //Debug.Log("Set my position");
                //transform.position = posList[currentPosIndex];
                posTimer += posResetTimer;
                currentPosIndex++;
            }
            bGhostDone = player.timer > playerData.playerData.time;
        }
	}
    void GhostMovement()
    {
        //GetGhostData();
        BasicMovement(hInputGhost[currentInputIndex]);
        if (vInputGhost[currentInputIndex] > 0.1 && bGrounded) Jump();
        currentInputIndex++;
    }
    void GetGhostData(SaveData player)
    {
        this.hInputGhost = player.hInputGhost;
        this.vInputGhost = player.vInputGhost;
        posList = player.posList;
    }
}
