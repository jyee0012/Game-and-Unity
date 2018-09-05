using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * 2-Dimensional movement of a character with shifting gravity
 * 
 * Conditions:  if the players center of gravity is focused downward
 *                  the left and right movement will be left and right
 *                  vice versa for upwards gravity
 *              if the players center of gravity is focused left
 *                  the left and right movement now becomes up and down
 *                  vice versa for gravity focused to the right
 * 
 */
public class PlayerMoveDefyGravity : MonoBehaviour {

    public enum Direction { LEFT, RIGHT, UP, DOWN, STOP }

    public Direction gravityDirection; //determines which direction the player falls
    public Direction playerDirection; //determines whether the player is facing left or right

    public float gravityStrength;

    public float movementSpeed = 1.0f;

    public bool isMoving;
    public bool isJumping;

    public float jumpPower;

    public KeyCode leftKey;
    public KeyCode rightKey;

    public KeyCode jumpKey;
    public KeyCode downKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SetDirection();

        //allow for moving while in the air
		if (!isMoving)
        {
            //Left Movement
            if (Input.GetKey(leftKey))
            {
                //move the character in the left direction based on the gravity
                if (gravityDirection == Direction.LEFT)
                    transform.position += Vector3.up * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.RIGHT)
                    transform.position += Vector3.down * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.DOWN)
                    transform.position += Vector3.left * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.UP)
                    transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }

            //Right Movement
            if (Input.GetKey(rightKey))
            {
                //move the character in the right direction based on the gravtiy
                if (gravityDirection == Direction.LEFT)
                    transform.position += Vector3.down * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.RIGHT)
                    transform.position += Vector3.up * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.DOWN)
                    transform.position += Vector3.right * movementSpeed * Time.deltaTime;
                if (gravityDirection == Direction.UP)
                    transform.position += Vector3.left * movementSpeed * Time.deltaTime;

            }

            if (Input.GetKeyDown(jumpKey) && !isJumping)
            {
                //apply a force in the direction considered opposite of the gravtiy
                isJumping = true;
                Debug.Log("Is Jumping");

                if (gravityDirection == Direction.DOWN)
                {
                    Debug.Log("Applying Force");
                    gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100 * jumpPower);
                }
                else if (gravityDirection == Direction.UP)
                {
                    Debug.Log("Applying Force");
                    gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 100 * jumpPower);
                }
                else if (gravityDirection == Direction.LEFT)
                {
                    Debug.Log("Applying Force");
                    gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 100 * jumpPower);
                }
                else if (gravityDirection == Direction.RIGHT)
                {
                    Debug.Log("Applying Force");
                    gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 100 * jumpPower);
                }
            }
        }
	}

    //sets the direction the gravity is pulling
    private void SetDirection()
    {
        //Sets the gravity direction but also needs to set player rotation
        if (gravityDirection == Direction.LEFT)
        {
            Physics.gravity = new Vector3(-gravityStrength, 0, 0);
            Physics2D.gravity = new Vector3(-gravityStrength, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        if (gravityDirection == Direction.RIGHT)
        {
            Physics.gravity = new Vector3(gravityStrength, 0, 0);
            Physics2D.gravity = new Vector3(gravityStrength, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        if (gravityDirection == Direction.UP)
        {
            Physics.gravity = new Vector3(0, gravityStrength, 0);
            Physics2D.gravity = new Vector3(0, gravityStrength, 0);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (gravityDirection == Direction.DOWN)
        {
            Physics.gravity = new Vector3(0, -gravityStrength, 0);
            Physics2D.gravity = new Vector3(0, -gravityStrength, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //check if the player is landing on something so it can set jumping to false
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collides with the cube");

        isJumping = false;

        CornerCube obj = other.gameObject.GetComponent<CornerCube>();

        // /*
        if (obj.GetComponent<CornerCube>())
        {
            if (obj.direction1 != gravityDirection)
            {
                //set player to that gravity call SetDirection
                gravityDirection = obj.direction1;
                SetDirection();
                //Debug.Log("");
            }
            else if (obj.direction2 != gravityDirection)
            {
                //set player to that gravity call SetDirection
                gravityDirection = obj.direction2;
                SetDirection();
            }
        }
        //*/
    }
}
