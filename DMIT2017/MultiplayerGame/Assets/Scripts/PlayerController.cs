using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rbody = null;
    [Header("Player Settings")]
    [SerializeField]
    int playerNum = 0;
    [SerializeField]
    bool canJump = true, reverseVertical = false;
    [SerializeField]
    float movementSpeed = 2f, jumpForce = 320f;

    [Space]
    [Header("Keyboard Settings")]
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;

    [Space]
    [Header("Controller Settings")]
    [SerializeField]
    bool useController = false;

    float vInput = 0, hInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (rbody == null)
        {
            if (GetComponent<Rigidbody>() == null)
            {
                rbody = gameObject.AddComponent<Rigidbody>();
            }
            else
            {
                rbody = GetComponent<Rigidbody>();
            }
        }
        rbody.useGravity = true;
        rbody.constraints = RigidbodyConstraints.FreezeRotation;
        rbody.drag = 1f;
        rbody.angularDrag = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
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
}
