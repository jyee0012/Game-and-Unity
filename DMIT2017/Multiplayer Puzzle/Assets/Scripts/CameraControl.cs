using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    bool cameraMove = true;
    [SerializeField]
    GameObject player1 = null, player2 = null;
    [SerializeField]
    float playerDist = 0;
    [SerializeField]
    Text controlText;
    Vector3 player1Pos, player2Pos, cameraStartPos;
    float camFOV = 60;
    // Use this for initialization
    void Start()
    {
        cameraStartPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMove)
        {
            transform.position = CameraMovement();
            if (player1 != null && player2 != null)
            {
                playerDist = Vector3.Distance(player1Pos, player2Pos);
                //lerpTimer = lerpTargetTime - Time.time;
                if (playerDist > 70)
                {
                    if (playerDist > 110)
                    {
                        player1.GetComponent<PlayerControl>().RespawnPlayer();
                        player2.GetComponent<PlayerControl>().RespawnPlayer();
                    }
                    //Instead of lerping try moving the camera further negative z coord
                    if (camFOV < 90)
                    {
                        camFOV++;
                        GetComponent<Camera>().fieldOfView = camFOV;
                    }
                }
                else
                {
                    if (camFOV > 60)
                    {
                        camFOV--;
                        GetComponent<Camera>().fieldOfView = camFOV;
                    }
                }
            }
        }
    }
    public void ToggleControlText()
    {
        if (controlText == null) return;
        controlText.gameObject.SetActive(!controlText.gameObject.activeInHierarchy);
    }
    Vector3 CameraMovement()
    {
        Vector3 newCameraPos = cameraStartPos;
        if (player1 != null)
        {
            player1Pos = player1.transform.position;
            if (player2 != null)
            {
                // both players are active
                player2Pos = player2.transform.position;
                // ((big vector - small vector) /2) + small vector = point between 2 vectors
                //cameraXPos = (player1Pos.x > player2Pos.x) ? (player1Pos.x - player2Pos.x) / 2 + player2Pos.x : (player2Pos.x - player1Pos.x) / 2 + player1Pos.x;
                //cameraYPos = (player1Pos.y > player2Pos.y) ? (player1Pos.y - player2Pos.y) / 2 + player2Pos.y : (player2Pos.y - player1Pos.y) / 2 + player1Pos.y;
                newCameraPos.x = Mathf.Lerp(player1Pos.x, player2Pos.x, 0.5f);
                newCameraPos.y = Mathf.Lerp(player1Pos.y, player2Pos.y, 0.5f);
            }
            else
            {
                // only player 1 is active
                newCameraPos.x = player1Pos.x;
                newCameraPos.y = player1Pos.y;
            }
        }
        else
        {
            // only player 2 is active
            player2Pos = player2.transform.position;
            newCameraPos.x = player2Pos.x;
            newCameraPos.y = player2Pos.y;
        }
        return newCameraPos;
    }
}
