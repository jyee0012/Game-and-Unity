using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerMovement : MonoBehaviour {

    [SerializeField]
    GameObject camera1, camera2, winObjective;
    float vInput, hInput, rInput;
    [SerializeField]
    float movementSpeed = 2, rotationSpeed = 100, multiJump = 2, forceModifier = 1;
    public KeyCode jumpKey, cameraSwapKey;
    float force = 100, jumpCount;
    bool bGrounded = true;
    Rigidbody rbody;
    Vector3 ground;
    [SerializeField]
    Text jumpText, winText;
    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody>();
        jumpKey = KeyCode.Space;
        cameraSwapKey = KeyCode.C;
    }
	
	// Update is called once per frame
	void Update ()
    {
        jumpText.text = "Jump: " + jumpCount + "/" + multiJump;
        ground = transform.position;
        ground.y -= 1f;
        if (Physics.Linecast(transform.position, ground))
        {
            bGrounded = true;
            jumpCount = 0;
        }
        Movement();
        Jump();
        DetectObjective(winObjective);
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
    }
    #region Movement
    void Movement()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rInput = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up, rInput * Time.deltaTime * rotationSpeed);
        transform.Translate(new Vector3(hInput * Time.deltaTime * movementSpeed, 0, vInput * Time.deltaTime * movementSpeed));
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
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(ground, 1f);
    }
    void DetectObjective(GameObject objective)
    {
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        //{

        //}

        Collider[] objectArray = Physics.OverlapSphere(transform.position, 5f);
        for(int i = 0; i < objectArray.Length; i++)
        {
            if (objectArray[i].transform.gameObject == objective)
            {
                winText.text = "Yay you win";
            }
        }
    }
}
