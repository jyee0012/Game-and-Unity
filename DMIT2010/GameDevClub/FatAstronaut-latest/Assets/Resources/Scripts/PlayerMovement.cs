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
public class PlayerMovement : MonoBehaviour
{

    public enum Direction { LEFT = 90, RIGHT = -90, UP = 180, DOWN = 0, STOP }

    public Direction gravityDirection; //the destination direction of gravity
    public Direction playerDirection; //determines whether the player is facing left or right

    private Quaternion destinationRotation; //the angle the player should be facing when moving from wall to wall

    //gravity acting on the object
    public float gravityStrength;
    public ConstantForce gravity;

    public float movementSpeed = 1.0f;
    public float drillingSpeed = 0.5f;

    //Drilling variables
    public float drillTime = 2.0f; //2 seconds
    public float drillwaitTime = 2.0f;

    private float currentDrillTime; //the predicted time of how lone to drill for
    private float currentWaitDrillTime; //tells the user how long to wait before can start drilling again

    //Jetpack Variables
    public Vector3 jetpackCapSpeed;
    public float jetpackPower = 10.0f; //the power of the jetpack against gravity

    //Jumping Variables
    public float jumpPower;

    //When the player is Damaged
    public float damageTimer;
    private float currentDamageTimer;

    //player is dead
    public bool isDying;

    //Validations for actions
    public bool isJumping;
    public bool isFlying;
    public bool isFalling;
    public bool isDrilling;
    public bool isTurning;
    public bool isDamaged; //grants the player invincible status for a certain time

    public bool jumpingTimerStarted;

    public Vector3 playerSaveState; //saves the player position and settings before drilling
    public GameObject currentGameSpace;

    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode downKey;

    //particle systems
    public GameObject digging_PS;

    private void Start()
    {
        //set the player to the spawn GameSpace
        SpawnPlayer();
        gravity = gameObject.AddComponent<ConstantForce>();
        gravity.force = new Vector3(0, -gravityStrength, 0);

        SetDirection();
    }
    public void SpawnPlayer()
    {
        foreach (GameObject space in GameObject.FindGameObjectsWithTag("Gamespace"))
        {
            if (space.GetComponent<GameSpaceScript>() != null && space.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Spawn)
            {
                transform.position = space.transform.position;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //SetDirection();
        ActivateJetPack();



        //turning the character
        if (isTurning)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, destinationRotation, 1.2f * movementSpeed * Time.deltaTime);

            if (!isDying)
            {
                Quaternion cameraRotation = destinationRotation;

                cameraRotation.x = 0;
                cameraRotation.y = 0;

                //turn the camera at the same speed as the player
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraRotation, 1.2f * movementSpeed * Time.deltaTime);
            }

            if (transform.rotation == destinationRotation)
            {
                isTurning = false;
            }
        }

        //allow for moving while in the air (ie. jumping/flying)
        if (!isDamaged)
        {
            if (!isFalling && !isDrilling)
            {
                movePlayer();

                playerJump();

                playerDrilling();
            }
            else if (isDrilling)
            {
                //check if the user has lifted his/her finger (before drilling timer is up)
                if (Input.GetKeyUp(downKey) && Time.time < currentDrillTime)
                {
                    Debug.Log("LIFTING FINGA: \n\t" + gravityDirection + "\nSaveState Position: " + playerSaveState.x + ", " + playerSaveState.y);

                    StopDrilling();
                }
                else
                {
                    //slowly move the player through the ground in the direction of gravity
                    if (gravityDirection == Direction.RIGHT)
                        transform.position += Vector3.right * drillingSpeed * Time.deltaTime;
                    else if (gravityDirection == Direction.LEFT)
                        transform.position += Vector3.left * drillingSpeed * Time.deltaTime;
                    else if (gravityDirection == Direction.UP)
                        transform.position += Vector3.up * drillingSpeed * Time.deltaTime;
                    else if (gravityDirection == Direction.DOWN)
                        transform.position += Vector3.down * drillingSpeed * Time.deltaTime;
                }

                //when the user has been drilling the right amount of time
                if (Time.time >= currentDrillTime - 0.2)
                {
                    //part of the animation where the astronaut pops out (apply a small force)
                    if (Time.time < currentDrillTime)
                    {
                        //apply a small force to the player in the direction of movement to show the player (popping out)
                        Debug.Log("Gravity Direction: " + gravityDirection);

                        //reverse the direction of gravity
                        switch (gravityDirection)
                        {
                            case Direction.LEFT:
                                gravityDirection = Direction.RIGHT;
                                break;
                            case Direction.RIGHT:
                                gravityDirection = Direction.LEFT;
                                break;
                            case Direction.DOWN:
                                gravityDirection = Direction.UP;
                                break;
                            case Direction.UP:
                                gravityDirection = Direction.DOWN;
                                break;
                            default:
                                gravityDirection = Direction.DOWN;
                                break;
                        }

                        //stronger when drilling up so player doesnt fall back down
                        //if (gravityDirection == Direction.DOWN)
                        //    ApplyForce(50);
                        //else
                        //    ApplyForce(30);

                        if (gravityDirection != Direction.DOWN)
                            isFalling = true;

                        digging_PS.SetActive(false);

                        //set the gravity to downward when player pops out
                        gravityDirection = Direction.DOWN;
                        SetDirection();
                    }
                    else
                    {
                        GetComponent<Rigidbody>().detectCollisions = true;
                        gravity.enabled = true;
                        //GetComponent<Rigidbody>().useGravity = true;

                        currentWaitDrillTime = Time.time + drillwaitTime;

                        isDrilling = false;
                    }
                }
            }
        }
        else
        {
            //check if the timer is over or not
            if (Time.time >= currentDamageTimer)
            {
                isDamaged = false;
            }
        }
    }

    #region Left/Right Movement
    //moving the player left and right
    public void movePlayer()
    {
        //Left Movement
        if (Input.GetKey(leftKey))
        {
            switch (gravityDirection)
            {
                case Direction.LEFT:
                    transform.rotation = Quaternion.Euler(-90, 0, -90);
                    break;
                case Direction.RIGHT:
                    transform.rotation = Quaternion.Euler(90, 0, 90);
                    break;
                case Direction.UP:
                    transform.rotation = Quaternion.Euler(0, 90, 180);
                    break;
                case Direction.DOWN:
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                default:
                    break;
            }

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
            //Debug.Log("ROTASTIONS: " + transform.rotation.x + "\n" + transform.rotation.z);
            //transform.Rotate(Vector3.up, 90f);

            //transform.rotation = new Quaternion(0, 0.7f, transform.rotation.z, 0.5f);
            Debug.Log("Rotation: " + transform.rotation.eulerAngles.y);

            switch(gravityDirection)
            {
                case Direction.LEFT:
                    transform.rotation = Quaternion.Euler(90, 0, -90);
                    break;
                case Direction.RIGHT:
                    transform.rotation = Quaternion.Euler(-90, 0, 90);
                    break;
                case Direction.UP:
                    transform.rotation = Quaternion.Euler(0, -90, 180);
                    break;
                case Direction.DOWN:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                default:
                    break;
            }
            //transform.rotation = Quaternion.Euler(destinationRotation.x, rotateVar, destinationRotation.z);

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
    }

    #endregion

    #region Player Jumping

    private void playerJump()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;

            if (gravityDirection != Direction.DOWN)
            {
                isFalling = true;
                ApplyForce(jumpPower / 2.5f);
                gravityDirection = Direction.DOWN;
                SetDirection();
            }
            else
            {
                ApplyForce(jumpPower);
            }

        }
    }

    #endregion

    #region Utility Functions

    private void ApplyForce(float force)
    {
        //Apply a force in the opposite direction of gravity
        if (gravityDirection == Direction.DOWN)
        {
            Debug.Log("Applying Force");
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * force * 10);
        }
        else if (gravityDirection == Direction.UP)
        {
            Debug.Log("Applying Force");
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * force * 10);
        }
        else if (gravityDirection == Direction.LEFT)
        {
            Debug.Log("Applying Force");
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * force * 10);
        }
        else if (gravityDirection == Direction.RIGHT)
        {
            Debug.Log("Applying Force");
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * force * 10);
        }
    }

    //tells the player to rotate to the passed in angle
    private void SetDestinationRotation(float angle)
    {
        //start rotating the player and set the rotation the player should be
        isTurning = true;
        destinationRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        playerDirection = gravityDirection;
    }

    //sets the direction the gravity is pulling
    private void SetDirection()
    {
        //Sets the gravity direction but also needs to set player rotation
        if (gravityDirection == Direction.LEFT)
        {
            gravity.force = new Vector3(-gravityStrength, 0, 0);

            if (playerDirection != gravityDirection)
                SetDestinationRotation(-90f);
        }
        else if (gravityDirection == Direction.RIGHT)
        {
            gravity.force = new Vector3(gravityStrength, 0, 0);

            //check if player is the same as gravity direction
            if (playerDirection != gravityDirection)
                SetDestinationRotation(90f);
        }
        else if (gravityDirection == Direction.UP)
        {
            gravity.force = new Vector3(0, gravityStrength, 0);

            if (playerDirection != gravityDirection)
                SetDestinationRotation(180f);
        }
        else // DOWN
        {
            gravity.force = new Vector3(0, -gravityStrength, 0);

            if (playerDirection != gravityDirection)
                SetDestinationRotation(0f);
        }
    }

    #endregion

    #region JetPack

    private void ActivateJetPack()
    {
        //jet pack button and make sure the jetpack is available
        if (Input.GetKey(KeyCode.Q) && GetComponent<PlayerStats>().CanUseJetPack())
        {
            isFalling = false;
            isJumping = false;

            //player cannot fly faster than the cap
            if (GetComponent<Rigidbody>().velocity.y < jetpackCapSpeed.y)
            {
                ApplyForce(jetpackPower);

                if (gravityDirection != Direction.DOWN)
                {
                    //ApplyForce(jumpPower / 3);
                    gravityDirection = Direction.DOWN;
                    SetDirection();
                }

                GetComponent<PlayerStats>().BurnFuel();
                isFlying = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && isFlying)
        {
            isFalling = true;
            isFlying = false;
        }
    }

    #endregion

    #region Drilling Action

    private void playerDrilling()
    {
        //drilling into the ground
        if (Input.GetKey(downKey) && Time.time >= currentWaitDrillTime && !isJumping && !isFalling)
        {
            isDrilling = true;
            isJumping = false;
            isFalling = false;

            //lock the player so not moving while drilling
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            //save the player's position and state before drilling
            playerSaveState = gameObject.transform.position;

            //start the drilling particle system
            digging_PS.SetActive(true);


            //turn collisions and gravity off
            gravity.enabled = false;
            //GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().detectCollisions = false;

            //set the time to drill
            currentDrillTime = Time.time + drillTime;
        }
    }

    private void StopDrilling()
    {
        //the player has stopped drilling
        isDrilling = false;
        isJumping = false;
        isFalling = false;

        digging_PS.SetActive(false);

        //snap the player back to the surface
        transform.position = playerSaveState;
        //gravityDirection = playerSaveState.GetComponent<PlayerMoveDefyGravity>().gravityDirection;

        //set it so the player cant drill for a period of time
        currentWaitDrillTime = Time.time + drillwaitTime;

        gravity.enabled = true;
        //GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().detectCollisions = true;
    }

    #endregion

    #region Collisions

    //pushes the player backwards (in the opposite direction he was hit)
    private void playerKnockBack(Direction direction)
    {
        //apply a force up
        ApplyForce(15);
        currentDamageTimer = Time.time + damageTimer; //player becomes invincible to damage for a short period of time

        if (direction == Direction.RIGHT)
            GetComponent<Rigidbody>().AddForce(Vector3.left * 50);
        else if (direction == Direction.LEFT)
            GetComponent<Rigidbody>().AddForce(Vector3.right * 50);

    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
        isFalling = false;

        if (collision.gameObject.tag.Equals("Enemy") && Time.time >= currentDamageTimer && !isDying)
        {
            isDamaged = true;

            GetComponent<PlayerStats>().TakeDamage(collision.gameObject.GetComponent<EnemyStats>().damage);
            //knock back the player
            Direction hitdirection;

            if (collision.gameObject.transform.position.x > transform.position.x)
                hitdirection = Direction.RIGHT;
            else
                hitdirection = Direction.LEFT;

            playerKnockBack(hitdirection);

            if (GetComponent<PlayerStats>().livesLeft == 0)
            {
                isDying = true;
                isTurning = true;
                if (collision.gameObject.transform.position.x > transform.position.x) //the enemy is on the right side
                {
                    //turn to the left
                    destinationRotation = Quaternion.AngleAxis(90, Vector3.forward);
                }
                else
                {
                    //turn to the right
                    destinationRotation = Quaternion.AngleAxis(-90, Vector3.forward);
                }
            }
        }

        if (isJumping)
            isFalling = true;

        if (isDying && !isTurning)
        {
            Debug.Log("Spawining GraveStone : " + isDying);
            //spawn the gravestone
            GetComponent<PlayerStats>().Death();
            isDying = false;
        }
    }

    //check if the player is landing on something so it can set jumping to false
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (!isDrilling)
        {
            Debug.Log("collides with the cube");

            // /*
            if (obj != null && obj.GetComponent<CornerCube>())
            {
                if (obj.GetComponent<CornerCube>().magnetDirection != gravityDirection)
                {
                    //set player to that gravity call SetDirection
                    gravityDirection = obj.GetComponent<CornerCube>().magnetDirection;

                    SetDirection();
                    //Debug.Log("");
                }
            }
            //*/

            if (obj != null && obj.GetComponent<GameSpaceScript>() != null && obj.GetComponent<GameSpaceScript>().thisSpace == GameSpaceScript.SpaceType.Block)
            {
                GetComponent<PlayerStats>().Death();
            }
        }

        //snaps the camera to the current gamespace position
        if (obj != null && obj.tag.Equals("Gamespace"))
        {
            if (obj != currentGameSpace)
            {
                currentGameSpace = obj;
                Camera.main.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, Camera.main.transform.position.z);
                //reset the camera rotation to normal
                Camera.main.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }

    #endregion
}
