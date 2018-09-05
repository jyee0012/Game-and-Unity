using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
   
    public GameManager.Direction direction;
    public bool isMoving;
    public bool onSand;

    //Controls
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;

    public Vector3 nextSquare;
    public bool atNextSquare;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {

        /*
         * Getting the input
         */
        if (atNextSquare)
            GetKeyInput();


        if (isMoving)
        {
            //determine the amount to move in the specified direction
            if (direction == GameManager.Direction.LEFT)
                transform.position += Vector3.left * GetComponent<PlayerStats>().GetMovementSpeed() * Time.deltaTime;
            if (direction == GameManager.Direction.RIGHT)
                transform.position += Vector3.right * GetComponent<PlayerStats>().GetMovementSpeed() * Time.deltaTime;
            if (direction == GameManager.Direction.UP)
                transform.position += Vector3.up * GetComponent<PlayerStats>().GetMovementSpeed() * Time.deltaTime;
            if (direction == GameManager.Direction.DOWN)
                transform.position += Vector3.down * GetComponent<PlayerStats>().GetMovementSpeed() * Time.deltaTime;

            //check if the player is in the position of the next square and stop moving
            if ((transform.position.x >= nextSquare.x && direction == GameManager.Direction.RIGHT) ||
                (transform.position.x <= nextSquare.x && direction == GameManager.Direction.LEFT) ||
                (transform.position.y >= nextSquare.y && direction == GameManager.Direction.UP) ||
                (transform.position.y <= nextSquare.y && direction == GameManager.Direction.DOWN))
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
                atNextSquare = true;
                isMoving = false;
            }
        }
    }

    private void GetKeyInput()
    {
        //get any player input
        if (Input.GetKey(Left))
        {
            direction = GameManager.Direction.LEFT;
            //Round the next square to full integers as the player will be snapping to the units
            nextSquare = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z) + Vector3.left;
            atNextSquare = false;
        }

        if (Input.GetKey(Right))
        {
            direction = GameManager.Direction.RIGHT;
            nextSquare = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z) + Vector3.right;
            atNextSquare = false;
        }

        if (Input.GetKey(Up))
        {
            direction = GameManager.Direction.UP;
            nextSquare = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z) + Vector3.up;
            atNextSquare = false;
        }

        if (Input.GetKey(Down))
        {
            direction = GameManager.Direction.DOWN;
            nextSquare = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z) + Vector3.down;
            atNextSquare = false;
        }

        //if square is available, player is moving
        if (checkAvailableSquare() && !atNextSquare)
            isMoving = true;
        else
        {
            isMoving = false;
            atNextSquare = true;
        }
    }

    //return true if there is no obstacle 
    private bool checkAvailableSquare()
    {
        //Debug.Log("Next Square Position: " + 
        //    "\nPosition X: " + nextSquare.x + 
        //    "\nPosition Y: " + nextSquare.y);

        //check all wall objects
        GameObject[] wallObjects = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wo in wallObjects)
        {
            if (wo.transform.position == nextSquare)
                return false;
        }

        return true;
    }

    // /*
    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject temp = collision.gameObject;

        Debug.Log("Entering Collision");
        //is it a transport hut?
        if (temp.GetComponent<TransportHut>() != null)
        {
            //save the hut id that it connects to
            int connectingID = temp.GetComponent<TransportHut>().connectingHut_ID;

            //get list of huts in the game scene
            GameObject[] huts = GameObject.FindGameObjectsWithTag("Transport Hut");
            Debug.Log("Huts in game: " + huts.Length);

            //find the hut thats connected and transport the player to it
            foreach (GameObject th in huts)
            {
                if (th.GetComponent<TransportHut>().hutID == connectingID)
                {
                    transform.position = th.GetComponent<TransportHut>().getHutEntrance(gameObject);
                    isMoving = false;
                    direction = GameManager.Direction.STOP;
                    atNextSquare = true;
                    Debug.Log("Transporting...");
                }
                    
            }
        }
        else if (temp.tag.Equals("Wall")) //player is running into an object tagged as a wall
        {
            isMoving = false;
            atNextSquare = true;
            direction = GameManager.Direction.STOP;
        } else if (temp.tag.Equals("Maskini"))
        {
            //call the Score(player) function in the GameManager
            GameManager.gm.Score(gameObject);
            temp.SetActive(false);
        }
    }
    // */

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag.Equals("Sand") && !onSand)
        {
            onSand = true;
            GetComponent<PlayerStats>().setMovementSpeed(GetComponent<PlayerStats>().movementSpeed / 2);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag.Equals("Sand") && onSand)
        {
            onSand = false;
            GetComponent<PlayerStats>().setMovementSpeed(GetComponent<PlayerStats>().movementSpeed * 2);
        }
    }
}
