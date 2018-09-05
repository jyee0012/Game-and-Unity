using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{

    public float animationSpeed = 1.0f;

    public bool isMoving;
    public bool atNextSquare;

    public GameManager.Direction direction;

    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode UP;
    public KeyCode DOWN;

    /* [0] = Left
     * [1] = Up
     * [2] = Down
     * [3] = Right
     */
    private Vector3 nextSquare;
    private GameManager.Direction playerInput;

    public void Start()
    {
        if (direction == GameManager.Direction.LEFT)
            nextSquare = transform.position + Vector3.left;
        if (direction == GameManager.Direction.RIGHT)
            nextSquare = transform.position + Vector3.right;
        if (direction == GameManager.Direction.UP)
            nextSquare = transform.position + Vector3.up;
        if (direction == GameManager.Direction.DOWN)
            nextSquare = transform.position + Vector3.down;
    }

    public void Update()
    {
        //tells the program which direction the player is going to move
        //tells the program that the player is going to start moving
        #region Basic Key Input

        if (!isMoving)
        {
            //check which key was pressed and if the player can move in that direction
            if (Input.GetKeyDown(LEFT))
            {
                isMoving = true;
                atNextSquare = false;
                direction = GameManager.Direction.LEFT;
                nextSquare = transform.position + Vector3.left;
            }
            else if (Input.GetKeyDown(RIGHT))
            {
                isMoving = true;
                atNextSquare = false;
                direction = GameManager.Direction.RIGHT;
                nextSquare = transform.position + Vector3.right;
            }
            else if (Input.GetKeyDown(UP))
            {
                isMoving = true;
                atNextSquare = false;
                direction = GameManager.Direction.UP;
                nextSquare = transform.position + Vector3.up;
            }
            else if (Input.GetKeyDown(DOWN))
            {
                isMoving = true;
                atNextSquare = false;
                direction = GameManager.Direction.DOWN;
                nextSquare = transform.position + Vector3.down;
            }

            playerInput = direction;
        } else
        {
            if (Input.GetKeyDown(LEFT))
                playerInput = GameManager.Direction.LEFT;
            else if (Input.GetKeyDown(RIGHT))
                playerInput = GameManager.Direction.RIGHT;
            else if (Input.GetKeyDown(UP))
                playerInput = GameManager.Direction.UP;
            else if (Input.GetKeyDown(DOWN))
                playerInput = GameManager.Direction.DOWN;
        }

        //stop the object at any time
        // /* Debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInput = GameManager.Direction.STOP;
        }
        // */

        //Debug.Log("Distance: " + Vector3.Distance(transform.position, nextSquare));

        #endregion

        if (isMoving)
        {
            //Debug.Log("Player Position: " + transform.position.x + "\nNext Square Position: " + nextSquare.x);

            //if the player has not reached the position of the next square on the game board
            if (!atNextSquare)
            {
                if (direction == GameManager.Direction.LEFT)
                    transform.position += Vector3.left * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.RIGHT)
                    transform.position += Vector3.right * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.DOWN)
                    transform.position += Vector3.down * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.UP)
                    transform.position += Vector3.up * animationSpeed * Time.deltaTime;

                //if the player has reached the next square
                if ((transform.position.x >= nextSquare.x && direction == GameManager.Direction.RIGHT) ||
                    (transform.position.x <= nextSquare.x && direction == GameManager.Direction.LEFT) ||
                    (transform.position.y <= nextSquare.y && direction == GameManager.Direction.DOWN) ||
                    (transform.position.y >= nextSquare.y && direction == GameManager.Direction.UP))
                {
                    transform.position = nextSquare;
                    atNextSquare = true;
                }
            }
            else
            {
                //snap player to set integer location
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);

                //Debug.Log("Has reached next Square!");
                //check if the the player input something
                if (playerInput != direction)
                {
                    //set the next square to the direction prompted by the user
                    if (playerInput == GameManager.Direction.LEFT)
                    {
                        direction = GameManager.Direction.LEFT;
                        nextSquare = transform.position + Vector3.left;
                    } else if (playerInput == GameManager.Direction.RIGHT)
                    {
                        direction = GameManager.Direction.RIGHT;
                        nextSquare = transform.position + Vector3.right;
                    } else if (playerInput == GameManager.Direction.UP)
                    {
                        direction = GameManager.Direction.UP;
                        nextSquare = transform.position + Vector3.up;
                    } else if (playerInput == GameManager.Direction.DOWN)
                    {
                        direction = GameManager.Direction.DOWN;
                        nextSquare = transform.position + Vector3.down;
                    } else if (playerInput == GameManager.Direction.STOP)
                    {
                        isMoving = false;
                    }

                    //set the player to snap to an integer location

                } else {
                    //get the next square
                    if (direction == GameManager.Direction.LEFT)
                        nextSquare = transform.position + Vector3.left;
                    if (direction == GameManager.Direction.RIGHT)
                        nextSquare = transform.position + Vector3.right;
                    if (direction == GameManager.Direction.UP)
                        nextSquare = transform.position + Vector3.up;
                    if (direction == GameManager.Direction.DOWN)
                        nextSquare = transform.position + Vector3.down;
                }
                
                atNextSquare = false;
            }
        }
    }

    // /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject temp = collision.gameObject;

        //Debug.Log("Entering Collision");
        //is it a transport hut?
        if (temp.GetComponent<TransportHut>() != null)
        {
            //save the hut id that it connects to
            int connectingID = temp.GetComponent<TransportHut>().connectingHut_ID;

            //get list of huts in the game scene
            GameObject[] huts = GameObject.FindGameObjectsWithTag("Transport Hut");
            //Debug.Log("Huts in game: " + huts.Length);

            //find the hut thats connected and transport the player to it
            foreach (GameObject th in huts)
            {
                if (th.GetComponent<TransportHut>().hutID == connectingID)
                {
                    transform.position = th.GetComponent<TransportHut>().getHutEntrance(gameObject);
                    isMoving = false;
                    //Debug.Log("Transporting...");
                }
            }
        } if (temp.tag.Equals("Wall"))
        {
            //Debug.Log("Running into wall");
            if ((temp.transform.position.x <= (transform.position + Vector3.left).x && direction == GameManager.Direction.LEFT) ||  //left
                (temp.transform.position.x >= (transform.position + Vector3.right).x && direction == GameManager.Direction.RIGHT) || //right
                (temp.transform.position.y >= (transform.position + Vector3.right).y && direction == GameManager.Direction.UP) || //up
                (temp.transform.position.y <= (transform.position + Vector3.right).y && direction == GameManager.Direction.DOWN))  //down
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
                isMoving = false;
            }            
        }
    }
    //*/
}