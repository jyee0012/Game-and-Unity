using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 2f, forceModifier = 1f;
    public bool bCanMove = true;
    public KeyCode jumpKey;
    public float multiJump = 1;
    Rigidbody rbody;
    bool bHasRigidbody = false, bGrounded = false, once = true;
    float force = 100, jumpCount = 0;
    [SerializeField]
    float vInput, hInput;

    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            bHasRigidbody = true;
            rbody = GetComponent<Rigidbody>();
        }
        if (jumpKey == KeyCode.None)
        {
            jumpKey = KeyCode.Space;
        }
    }

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        Vector3 ground = transform.position;
        ground.y -= 1f;
        // true if blocked
        if (Physics.Linecast(transform.position, ground))
        {
            bGrounded = true;
            jumpCount = 0;
        }
        if (bHasRigidbody)
        {
            if (bCanMove)
            {
                //rbody.AddForce(hInput * movementSpeed, 0, vInput * movementSpeed);
                transform.Translate(Vector3.forward * vInput * Time.deltaTime * movementSpeed);
                transform.Translate(Vector3.right * hInput * Time.deltaTime * movementSpeed);
                //Debug.Log((bGrounded || jumpCount <= multiJump) + ":" + jumpCount);
                if (Input.GetKeyDown(jumpKey) && (bGrounded || jumpCount < multiJump))
                {
                    force = 100 * forceModifier;
                    rbody.velocity = Vector3.zero;
                    rbody.AddForce(0, force, 0);
                    bGrounded = false;
                    jumpCount++;
                }
            }
        }
    }
}
