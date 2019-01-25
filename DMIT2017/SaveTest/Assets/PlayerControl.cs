using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<float> hInputGhost, vInputGhost;
    public List<Vector3> posList;
    public string playerName;
    #region Start
    void Start()
    {
        if (GetComponent<Rigidbody>() != null) rbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        posTimer += posResetTimer;
        playerNameText.text = playerName;
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        if (!bGrounded && jumpDelay < Time.time)
        {
            ground = transform.position;
            ground.y -= 1f;
            if (bGrounded = CheckGround(ground)) jumpCount = 0;
        }
        PlayerMovement();
        PlayerJump();
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
    void PlayerMovement()
    {
        hInput = Input.GetAxis("Horizontal");
        hText.text = "hInput: " + hInput;
        hInputGhost.Add(hInput);
        BasicMovement(hInput);
    }
    protected void BasicMovement(float input)
    {
        transform.Translate(new Vector3(input * Time.deltaTime * movementSpeed, 0, 0));
        RespawnPlayer();
    }
    void PlayerJump()
    {
        vInput = Input.GetAxis("Vertical");
        vText.text = "vInput: " + vInput;
        if (JumpInput()) vInput = 1; 
        vInputGhost.Add(vInput);
        if ((JumpInput()) && (bGrounded || jumpCount < maxJumps))
        {
            Jump();
        }
    }
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
        return Input.GetKeyDown(jumpKey) || vInput > 0.1;
    }
    protected bool CheckGround(Vector3 ground)
    {
        return Physics.Linecast(transform.position, ground, 1 << 0);
    }
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
