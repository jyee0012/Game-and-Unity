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
    #region PlayerData Functions
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
    #endregion
}

public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 2f, forceModifier = 1, timer = 0, maxJumps = 1, jumpDelay = 0.5f, groundRange = 1f, deathHeight = -10f, recordTimeLimit = 10f;
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space, recordBtn = KeyCode.R;
    [SerializeField]
    Text timerText = null, vText = null, hText = null, playerNameText = null;
    [SerializeField]
    GameObject ghostPlayer;
    [SerializeField]
    bool drawGizmo= true;

    protected float vInput, hInput, jumpCount = 0, jumpTimer = 0, posResetTimer = 10, posTimer = 0;
    protected Rigidbody rbody;
    protected bool bGrounded = true;
    protected Vector3 ground, startPos, ghostStartPos;
    
    public bool useController = false, isMultiplayer = false, recording = false;
    public int playerNum = 1;

    // Ghost Data
    public List<float> hInputGhost, vInputGhost;
    public List<Vector3> posList;

    private float recordTimer = 0, recordStart = 0, recordEnd = 0;

    #region Default Functions
    void Start()
    {
        if (GetComponent<Rigidbody>() != null) rbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        posTimer += posResetTimer;
        SetPlayerNameText();
    }
    // Update is called once per frame
    void Update()
    {
        // cannot be placed within a function, it causes delays
        if (!bGrounded && jumpTimer < Time.time)
        {
            ground = transform.position;
            ground.y -= groundRange;
            if (bGrounded = CheckGround(ground)) jumpCount = 0;
        }
        // 
        AllMovement();
        if (recording)
        {
            if (Input.GetKeyDown(recordBtn))
            {
                StopGhostRecord();
            }
        }
        else
        {
            if (Input.GetKeyDown(recordBtn))
            {
                StartGhostRecord();
            }
        }
        if (recording && recordTimeLimit < Time.time)
        {
            StopGhostRecord();
        }
        Timer();
        if (posTimer < Time.time)
        {
            posList.Add(transform.position);
            posTimer += posResetTimer;
        }
    }
    private void FixedUpdate()
    {
    }
    #endregion
    #region Movement Functions
    protected void RespawnPlayer(float fallHeight = -10)
    {
        if (transform.position.y < fallHeight) transform.position = startPos;
    }
    void AllMovement()
    {
        PlayerMovement();
        PlayerJump();
    }
    void PlayerMovement()
    {
        ControllerMovement();

        InputText();
        if (recording) hInputGhost.Add(hInput);
        //RespawnPlayer();
    }
    void PlayerJump()
    {
        ControllerMovement();
        InputText();
        if (JumpInput()) vInput = 1;
        if (recording) vInputGhost.Add(vInput);
        if ((JumpInput()) && (bGrounded || jumpCount < maxJumps))
        {
            Jump();
        }
    }
    #region Base Jump Functions
    protected void Jump()
    {
        //Debug.Log(gameObject.name);
        float force = 100 * forceModifier;
        rbody.velocity = Vector3.zero;
        rbody.AddForce(0, force, 0);
        bGrounded = false;
        jumpTimer = Time.time + jumpDelay;
        jumpCount++;
    }
    bool JumpInput()
    {
        return Input.GetKey(jumpKey) || vInput > 0.01;
    }
    protected bool CheckGround(Vector3 ground)
    {
        return Physics.Linecast(transform.position, ground, 1 << 0);
    }
    void ResetGround()
    {
        if (!bGrounded && jumpTimer < Time.time)
        {
            ground = transform.position;
            ground.y -= groundRange;
            if (bGrounded = CheckGround(ground)) jumpCount = 0;
        }
    }
    #endregion
    #region Base Movement Functions 
    protected void BasicMovement(float input)
    {
        transform.Translate(new Vector3(input * Time.deltaTime * movementSpeed, 0, 0));
        RespawnPlayer(deathHeight);
    }
    protected void BasicMovement(Vector3 vectorInput)
    {
        transform.Translate(vectorInput);
        RespawnPlayer(deathHeight);
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
            vInput = Input.GetAxis("Button0P" + playerNum);
            hInput = Input.GetAxis("Axis1P" + playerNum);
        }
        else if (isMultiplayer)
        {
            vInput = Input.GetAxis("VerticalP" + playerNum);
            hInput = Input.GetAxis("HorizontalP" + playerNum);
        }
        else
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
        }
        BasicMovement(Vector3.right * (hInput) * Time.deltaTime * movementSpeed);
    }
    #endregion
    #endregion
    void GetAllControllers()
    {
        string[] controllers = Input.GetJoystickNames();
        //Debug.Log(controllers.Length + " controllers");
        PlayerControl[] players = FindObjectsOfType<PlayerControl>();
        PlayerControl otherPlayer = null;
        if (players.Length > 1)
        {
            foreach (PlayerControl currentPlayer in players)
            {
                if (currentPlayer != this) otherPlayer = currentPlayer;
            }
        }
        for (int i = 0; i < controllers.Length; i++)
        {
            Debug.Log(i + ":" + controllers[i]);
            if (controllers[i].Length > 0)
            {
                if (otherPlayer.playerNum == i || otherPlayer == null)
                {
                    playerNum = i + 1;
                    return;
                }
            }
        }
    }
    #region Update Text
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
    void SetPlayerNameText()
    {
        if (playerNameText == null) return;
        playerNameText.text = "Player " + playerNum;
    }
    void InputText()
    {
        if (hText != null)
        {
            hText.text = "hInput: " + hInput;
        }
        if (vText != null)
        {
            vText.text = "vInput: " + vInput;
        }
    }
    #endregion
    #region Ghost/Record Functions
    void StartGhostRecord()
    {
        vInputGhost.Clear();
        hInputGhost.Clear();
        ghostStartPos = transform.position;
        recording = true;
        recordStart = Time.time;
        recordTimer = Time.time + recordTimeLimit;
    }
    void StopGhostRecord()
    {
        recording = false;
        recordEnd = Time.time;
    }
    void PlayGhostRecord()
    {
        ghostPlayer.SetActive(true);
    }
    public PlayerData GeneratePlayerData()
    {
        // this function should be triggered right before the ghost plays the recording
        PlayerData currentData = new PlayerData();
        currentData.name = "Player " + playerNum;
        currentData.time = (recordEnd - recordStart) + Time.time; // the record duration, so when the duration ends so does the ghost. This should be recordEnd - recordStart + recordPlay.
        currentData.vInputGhost = vInputGhost;
        currentData.hInputGhost = hInputGhost;
        currentData.posList.Clear();
        currentData.posList.Add(ghostStartPos);
        return currentData;
    }
    #endregion
    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, ground);
        }
    }
}
