using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Rigidbody rbody = null;
    [Header("Player Settings")]
    public int playerNum = 0;
    [SerializeField]
    bool canJump = true, reverseVertical = false, playerMove = true;
    [SerializeField]
    float movementSpeed = 2f, jumpForce = 320f, rotateSpeed = 100f;

    [Header("Keyboard Settings")]
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [Header("Controller Settings")]
    public bool useController = false;

    [Header("Timer/Wave Controls")]
    [SerializeField]
    Text timerText = null;
    [SerializeField]
    Text waveText = null;
    float timer = 0, waveNum = 0, waveTimer = 0;

    float vInput = 0, hInput = 0, rInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (playerMove)
        {
            if (rbody == null) rbody = GetComponent<Rigidbody>();
            rbody.useGravity = true;
            rbody.constraints = RigidbodyConstraints.FreezeRotation;
            rbody.drag = 1f;
            rbody.angularDrag = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove)
        {
            PlayerMovement();
            PlayerRotate();
        }
        Timer();
    }
    void PlayerMovement()
    {
        if (!useController)
        {
            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
        }
        else
        {
            vInput = Input.GetAxis("Axis2P" + playerNum);
            hInput = Input.GetAxis("Axis1P" + playerNum);
        }
        if (reverseVertical) vInput *= -1;
        transform.Translate(new Vector3(hInput * movementSpeed * Time.deltaTime, 0, vInput * movementSpeed * Time.deltaTime));
        if (JumpInput() && canJump)
        {
            Jump();
        }
    }
    void PlayerRotate()
    {
        if (!useController)
        {
            rInput = Input.GetAxis("Mouse X");
        }
        else
        {
            rInput = Input.GetAxis("Axis4P" + playerNum);
        }
        transform.Rotate(new Vector3(0, rInput * rotateSpeed * Time.deltaTime, 0));
    }
    void Jump()
    {
        rbody.AddForce(new Vector3(0, jumpForce, 0));
    }
    bool JumpInput()
    {
        bool input = false;
        if (useController)
        {
            input = Input.GetAxis("Button0P" + playerNum) > 0;
        }
        else
        {
            input = Input.GetKeyDown(jumpKey);
        }
        return input;
    }
    private void OnDestroy()
    {
        if (GetComponentInChildren<Camera>() != null)
        {
            Camera playerCam = GetComponentInChildren<Camera>();
            playerCam.transform.parent = null;
            playerCam.enabled = true;

        }
    }
    void Timer()
    {
        timer += Time.deltaTime;
        //Debug.Log("Timer: " + timer);
        if ((int)timer % 90 == 0 && timer > waveTimer)
        {
            if (waveNum < 3)
            {
                waveNum++;
                waveTimer = timer + 10;
            }
        }
        UpdateTimerText();
        UpdateWaveText();
    }
    void UpdateTimerText()
    {
        string timerString = "Time: ",
            minSec = GetTimeText(timer);
        if (timerText != null) timerText.text = timerString + minSec;
    }
    void UpdateWaveText()
    {
        if (waveText == null) return;
        waveText.text = "Wave: " + waveNum;
    }
    string GetTimeText(float time)
    {
        return string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
    }
}
