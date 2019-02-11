using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    bool cameraMove = true;
    [SerializeField]
    GameObject player1, player2;
    [SerializeField]
    float playerDist = 0;
    Vector3 player1Pos, player2Pos, cameraStartPos;
    float cameraYPos = 0, cameraXPos = 0, lerpTimer = 0, lerpTargetTime;
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
            playerDist = Vector3.Distance(player1Pos, player2Pos);
            //lerpTimer = lerpTargetTime - Time.time;
            //if (playerDist > 70)
            //{
            //    if (GetComponent<Camera>().fieldOfView == 60) lerpTargetTime = Time.time + 1f;
            //    GetComponent<Camera>().fieldOfView = Mathf.Lerp(90, 60,1f);
            //}
        }
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
