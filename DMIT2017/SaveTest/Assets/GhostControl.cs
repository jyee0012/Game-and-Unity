using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : PlayerControl {

    PlayerControl player;

    int currentIndex = 0;
    [SerializeField]
    float timeDelay = 10f;
	// Use this for initialization
	void Start () {
        if (player == null) player = GameObject.Find("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeDelay)
        {
            ground = transform.position;
            ground.y -= 1f;
            if (bGrounded = CheckGround(ground)) jumpCount = 0;
            GhostMovement();
        }
	}
    void GhostMovement()
    {
        BasicMovement(player.hInputGhost[currentIndex]);
        if (player.vInputGhost[currentIndex] > 0.1 && bGrounded) Jump();
    }
    void GetGhostData()
    {
        this.hInputGhost = player.hInputGhost;
        this.vInputGhost = player.vInputGhost;
    }
}
