using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : PlayerControl {

    public PlayerControl player;
    public PlayerData playerData;
    public bool bHasGhostData = false, bGhostDone = false;
    public int currentInputIndex = 0, currentPosIndex;
    [SerializeField]
    float timeDelay = 10f;
	// Use this for initialization
	void Start ()
    {
        rbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        posTimer = posResetTimer + timeDelay;

        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerControl>();
        if (playerData == null) playerData = player.GetComponent<PlayerData>();
   
        if (playerData != null) bHasGhostData = playerData.hasGhostData;
        if (bHasGhostData) GetGhostData(playerData);

        gameObject.SetActive(bHasGhostData);
    }
	
	// Update is called once per frame
	void Update () {
		if ((Time.time > timeDelay && !bGhostDone) && bHasGhostData)
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
        }
        if (bGhostDone)
        {
            gameObject.SetActive(false);
            currentInputIndex = currentPosIndex = 0;
            bGhostDone = false;
        }
	}
    void GhostMovement()
    {
        //GetGhostData();
        BasicMovement(hInputGhost[currentInputIndex]);
        if (vInputGhost[currentInputIndex] > 0.1 && bGrounded) Jump();
        currentInputIndex++;
        bGhostDone = currentInputIndex >= hInputGhost.Count;
    }
    void GetGhostData(PlayerData player)
    {
        this.hInputGhost = player.hInputGhost;
        this.vInputGhost = player.vInputGhost;
        posList = player.posList;
    }
}
