using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    public float maxSpeed = 15, jumpForce = 1000;
    public Transform groundCheck;
    public bool hasKey { get; set; }
    protected Animator myAnimator;
    protected Rigidbody2D myRigidBody;
    protected float moveForce = 365;
    protected bool facingRight = true, grounded = false, jump = false;
    int totalCoins;
    public Text coinText;
    GameObject bullet;
    float shotDelay;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        bullet = Resources.Load("bulletPrefab") as GameObject;
        hasKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        // layer mask bitwise ops: https://answers.unity.com/questions/8715/how-do-i-use-layermasks.html
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        jump = (Input.GetButtonDown("Jump") && grounded);
        Respawn(-20);
        Movement();
        if (Input.GetKey(KeyCode.V) && shotDelay < Time.time)
        {
            Shoot();
            shotDelay = Time.time + 0.5f;
        }
        coinText.text = (totalCoins - GameObject.FindGameObjectsWithTag("Coin").Length) + "/" + totalCoins;
    }

    void FixedUpdate()
    {

    }
    #region Respawn
    void Respawn(float limit)
    {
        if (transform.position.y < limit)
        {
            transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            //Instantiate(Resources.Load("Ball"), GameObject.FindGameObjectWithTag("Spawn").transform.position, Quaternion.identity);
        }
    }
    #endregion
    #region Movement
    void Movement()
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
    #endregion
    #region Shoot
    void Shoot()
    {
        GameObject shotBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        if (facingRight)
        {
            shotBullet.GetComponent<BulletScript>().direction = new Vector3(1, 0, 0);
        }
        else
        {
            shotBullet.GetComponent<BulletScript>().direction = new Vector3(-1, 0, 0);
        }
        Destroy(shotBullet, 10f);
    }
    #endregion
}
