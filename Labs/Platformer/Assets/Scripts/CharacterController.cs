using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float maxSpeed = 5, jumpForce = 1000;
    public Transform groundCheck;

    protected Animator myAnimator;
    protected Rigidbody2D myRigidBody;
    protected float moveForce = 365;
    protected bool facingRight = true, grounded = false, jump = false;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // layer mask bitwise ops: https://answers.unity.com/questions/8715/how-do-i-use-layermasks.html
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        jump = (Input.GetButtonDown("Jump") && grounded);
    }

    void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
            facingRight = horizontalAxis > 0 ? true : false;

        myAnimator.SetFloat("Speed", Mathf.Abs(horizontalAxis));
        //Have we reach maxSpeed? if not, add force.
        if (horizontalAxis * myRigidBody.velocity.x < maxSpeed)
        {
            myRigidBody.AddForce(Vector2.right * horizontalAxis * moveForce);
        }
        //have we exceeded the maxSpeed? Clamp it (set it to maxSpeed).
        if (Mathf.Abs(myRigidBody.velocity.x) > maxSpeed)
        {
            myRigidBody.velocity = new Vector2(Mathf.Sign(myRigidBody.velocity.x) * maxSpeed, myRigidBody.velocity.y);
        }
        if (jump)
        {
            myAnimator.SetTrigger("Jump");
            myRigidBody.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
        if (facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
}
