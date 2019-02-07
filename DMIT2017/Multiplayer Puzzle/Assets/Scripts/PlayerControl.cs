using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData
{
    public string name, description;
    public int score;
    public float time;
    public List<float> vInputGhost, hInputGhost;
    public List<Vector3> posList;
    public PlayerData()
    {
        name = "noone";
        score = 0;
        vInputGhost = new List<float>();
        hInputGhost = new List<float>();
        posList = new List<Vector3>();
    }
    public void ClearGhostData()
    {
        vInputGhost = new List<float>();
        hInputGhost = new List<float>();
        posList = new List<Vector3>();
    }
    public bool hasGhostData
    {
        get
        {
            return (hInputGhost.Count > 0 && vInputGhost.Count > 0);
        }
    }
    public string GetName()
    {
        return name;
    }
    public void SetName(string value)
    {
        name = value;
    }
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int value)
    {
        score = value;
    }
}

public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 2f, forceModifier = 1, timer = 0, maxJumps = 1;
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;
    [SerializeField]
    Text timerText, vText, hText, playerNameText;

    protected float vInput, hInput, jumpCount = 0, jumpDelay = 0, posResetTimer = 10, posTimer = 0;
    protected Rigidbody rbody;
    protected bool bGrounded = true;
    protected Vector3 ground, startPos;

    public bool useController;
    public int playerNum = 1;

    // Ghost Data
    public List<float> hInputGhost, vInputGhost;
    public List<Vector3> posList;
    public string playerName;
    #region Start
    void Start()
    {
        if (GetComponent<Rigidbody>() != null) rbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        posTimer += posResetTimer;
        if (playerName == "") playerName = "N/A";
        //playerNameText.text = playerName;
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        AllMovement();
        Timer();
        if (posTimer < Time.time)
        {
            posList.Add(transform.position);
            posTimer += posResetTimer;
        }
    }
    #endregion
    #region Fixed Update
    private void FixedUpdate()
    {

    }
    #endregion

    protected void RespawnPlayer(float fallHeight = -10)
    {
        if (transform.position.y < fallHeight) transform.position = startPos;
    }
    void AllMovement()
    {
        ResetGround();
        PlayerMovement();
    }
    void PlayerMovement()
    {
        ControllerMovement();
        hText.text = "hInput: " + hInput;
        hInputGhost.Add(hInput);
        RespawnPlayer();
    }
    void PlayerJump()
    {
        vText.text = "vInput: " + vInput;
        if (JumpInput()) vInput = 1;
        vInputGhost.Add(vInput);
        if ((JumpInput()) && (bGrounded || jumpCount < maxJumps))
        {
            Jump();
        }
    }
    void CameraMovement()
    {
        // ((big vector - small vector) /2) + small vector = point between 2 vectors
    }
    #region Base Jump Functions
    protected void Jump()
    {
        //Debug.Log(gameObject.name);
        float force = 100 * forceModifier;
        rbody.velocity = Vector3.zero;
        rbody.AddForce(0, force, 0);
        bGrounded = false;
        jumpDelay = Time.time + 0.5f;
        jumpCount++;
    }
    bool JumpInput()
    {
        return Input.GetKey(jumpKey) || vInput > 0.1;
    }
    protected bool CheckGround(Vector3 ground)
    {
        return Physics.Linecast(transform.position, ground, 1 << 0);
    }
    void ResetGround()
    {
        if (!bGrounded && jumpDelay < Time.time)
        {
            ground = transform.position;
            ground.y -= 1f;
            if (bGrounded = CheckGround(ground)) jumpCount = 0;
        }
    }
    #endregion
    // to fix a jump delay issue the player jump is now within controller movement so there is no delay when getting vInput
    #region Base Movement Functions 
    protected void BasicMovement(float input)
    {
        transform.Translate(new Vector3(input * Time.deltaTime * movementSpeed, 0, 0));
        RespawnPlayer();
    }
    void ControllerMovement()
    {
        #region Controls
        /*
        axis1.text = Input.GetAxis("Axis1").ToString(); //left analog horizontal
        axis2.text = Input.GetAxis("Axis2").ToString(); //left analog vertical
        axis3.text = Input.GetAxis("Axis3").ToString(); //offset of triggers
        axis4.text = Input.GetAxis("Axis4").ToString(); //right analog horizontal
        axis5.text = Input.GetAxis("Axis5").ToString(); //right analog vertical
        axis6.text = Input.GetAxis("Axis6").ToString(); //d-pad horizontal
        axis7.text = Input.GetAxis("Axis7").ToString(); //d-pad vertical
        axis8.text = Input.GetAxis("Axis8").ToString();
        axis9.text = Input.GetAxis("Axis9").ToString(); //left trigger
        axis10.text = Input.GetAxis("Axis10").ToString(); //right trigger
        button0.text = Input.GetAxis("Button0").ToString(); //a button
        button1.text = Input.GetAxis("Button1").ToString(); //b button
        button2.text = Input.GetAxis("Button2").ToString(); //x button
        button3.text = Input.GetAxis("Button3").ToString(); //y button
        button4.text = Input.GetAxis("Button4").ToString(); //left bumper
        button5.text = Input.GetAxis("Button5").ToString(); //right bumper
        button6.text = Input.GetAxis("Button6").ToString(); //back button
        button7.text = Input.GetAxis("Button7").ToString(); //start button
        */
        #endregion
        if (useController)
        {
            vInput = Input.GetAxis("Axis2P" + playerNum);
            hInput = Input.GetAxis("Axis1P" + playerNum);

            transform.Translate(-Vector3.forward * (hInput) * Time.deltaTime * movementSpeed);
        }
        else
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * (hInput) * Time.deltaTime * movementSpeed);
            //transform.Rotate(Vector3.up * (hInput));
        }
        PlayerJump();
    }
    #endregion
    void Timer()
    {
        timer += Time.deltaTime;
        //Debug.Log("Timer: " + timer);
        UpdateTimerText();
    }
    void UpdateTimerText()
    {
        string timerString = "Time: ",
            minSec = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
        if (timerText != null) timerText.text = timerString + minSec;
    }
}
