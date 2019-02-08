using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    GameObject player1, player2;
    [SerializeField]
    float playerDist = 0;
    Vector3 player1Pos, player2Pos, cameraStartPos, newCameraPos;
    float cameraYPos = 0, cameraXPos = 0, lerpTimer = 0, lerpTargetTime;
    // Use this for initialization
    void Start()
    {
        cameraStartPos = transform.position;
        newCameraPos = cameraStartPos;

    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        newCameraPos.x = cameraXPos;
        newCameraPos.y = cameraYPos;
        transform.position = newCameraPos;
        playerDist = Vector3.Distance(player1Pos, player2Pos);
        //lerpTimer = lerpTargetTime - Time.time;
        //if (playerDist > 70)
        //{
        //    if (GetComponent<Camera>().fieldOfView == 60) lerpTargetTime = Time.time + 1f;
        //    GetComponent<Camera>().fieldOfView = Mathf.Lerp(90, 60,1f);
        //}
    }
    void CameraMovement()
    {
        player1Pos = player1.transform.position;
        player2Pos = player2.transform.position;
        // ((big vector - small vector) /2) + small vector = point between 2 vectors
        //cameraXPos = (player1Pos.x > player2Pos.x) ? (player1Pos.x - player2Pos.x) / 2 + player2Pos.x : (player2Pos.x - player1Pos.x) / 2 + player1Pos.x;
        //cameraYPos = (player1Pos.y > player2Pos.y) ? (player1Pos.y - player2Pos.y) / 2 + player2Pos.y : (player2Pos.y - player1Pos.y) / 2 + player1Pos.y;
        cameraXPos = Mathf.Lerp(player1Pos.x, player2Pos.x, 0.5f);
        cameraYPos = Mathf.Lerp(player1Pos.y, player2Pos.y, 0.5f);
    }
}
