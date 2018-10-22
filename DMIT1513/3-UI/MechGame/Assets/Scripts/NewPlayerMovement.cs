﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerMovement : MonoBehaviour
{

    [SerializeField]
    GameObject camera1, camera2, winObjective;
    float vInput, hInput, rInput;
    [SerializeField]
    float movementSpeed = 2, rotationSpeed = 100, multiJump = 2, forceModifier = 1;
    [SerializeField]
    bool canJump = true, canMove = true;
    public KeyCode jumpKey, cameraSwapKey;
    float force = 100, jumpCount;
    bool bGrounded = true;
    Rigidbody rbody;
    Vector3 ground;
    [SerializeField]
    Text jumpText, winText;
    [SerializeField]
    AudioSource mechWalk;

    public bool useForceQuit = false;
    #region Start
    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        if (jumpKey == KeyCode.None) jumpKey = KeyCode.Space;
        if (cameraSwapKey == KeyCode.None) cameraSwapKey = KeyCode.C;
        if (mechWalk !=null)
        {
            mechWalk.loop = true;
            mechWalk.Play();
            mechWalk.Pause();
        }
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        if (jumpText != null) jumpText.text = "Jump: " + jumpCount + "/" + multiJump;

        ground = transform.position;
        ground.y -= 1f;
        bGrounded = CheckGround(ground);

        if (canMove) Movement();
        if (canJump) Jump();
        #region Camera Control
        if (Input.GetKeyDown(cameraSwapKey))
        {
            if (camera1.active)
            {
                SwapCamera(camera1, camera2);
            }
            else
            {
                SwapCamera(camera2, camera1);
            }
        }
        #endregion

        if (useForceQuit && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion 

    #region Check Ground
    bool CheckGround(Vector3 ground)
    {
        if (Physics.Linecast(transform.position, ground))
        {
            jumpCount = 0;
            return true;
        }
        return false;
    }
    #endregion

    #region Movement
    void Movement()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rInput = Input.GetAxis("Mouse X");
        //Debug.Log("V: " + vInput + " H: " + hInput + " R: " + rInput);

        transform.Rotate(Vector3.up, rInput * Time.deltaTime * rotationSpeed);
        transform.Translate(new Vector3(hInput * Time.deltaTime * movementSpeed, 0, vInput * Time.deltaTime * movementSpeed));

        if (Mathf.Abs(vInput) > 0.1 || Mathf.Abs(hInput) > 0.1)
        {
            //Debug.Log("Do da sound ting");
            mechWalk.UnPause();
            //mechWalk.Play();
        }
        else
        {
            mechWalk.Pause();
            //mechWalk.Stop();
        }
    }
    #endregion 

    #region Swap Camera
    void SwapCamera(GameObject camera1, GameObject camera2)
    {
        camera2.SetActive(true);
        camera1.SetActive(false);
        //GameObject canvas = GameObject.Find("Canvas");
        //if (canvas != null)
        //{
        //    canvas.GetComponent<Canvas>();
        //}
    }
    #endregion

    #region Jump
    void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && (bGrounded || jumpCount < multiJump))
        {
            force = 100 * forceModifier;
            rbody.velocity = Vector3.zero;
            rbody.AddForce(0, force, 0);
            bGrounded = false;
            jumpCount++;
        }
    }
    #endregion

    #region Draw Gizmos
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(ground, 1f);
    }
    #endregion

    #region Detect Objective
    void DetectObjective(GameObject objective)
    {
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        //{

        //}

        Collider[] objectArray = Physics.OverlapSphere(transform.position, 5f);
        for (int i = 0; i < objectArray.Length; i++)
        {
            if (objectArray[i].transform.gameObject == objective)
            {
                if (winText != null) winText.text = "Yay you win";
            }
        }
    }
    #endregion
}