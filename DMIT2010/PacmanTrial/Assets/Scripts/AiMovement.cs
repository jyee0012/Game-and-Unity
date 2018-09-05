using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovement : MonoBehaviour
{

    #region Variables
    public float animationSpeed = 1.0f;

    public bool isMoving;
    public bool atNextSquare;

    public GameManager.Direction direction;
    #region Key Inputs Variables
    KeyCode LEFT;
    KeyCode RIGHT;
    KeyCode UP;
    KeyCode DOWN;
    #endregion
    /* [0] = Left
     * [1] = Up
     * [2] = Down
     * [3] = Right
     */
    private Vector3 nextSquare;
    private GameManager.Direction playerInput;
    #endregion
    #region AI Variables
    GameObject closestWall;
    enum someDirection { Left, Right, Up, Down }
    someDirection directMe;
    GameManager.Direction lastDirection;
    bool isAI = true, cannotReach = false;
    public int randMove = -1;
    float timeStamp;
    Vector3 playerLocation;
    // Is suppose to store the direction in int state of which directions it cannot go
    public List<int> cannotGo = new List<int>(); // not really being used
    #endregion
    #region Start
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
    #endregion
    #region Update
    public void Update()
    {
        #region List-of-To-Do's
        // 0 = left = x++
        // 1 = right = x--
        // 2 = up = y++
        // 3 = down = y--
        // Level 1 AI: moves randomly
        // Level 2 AI: (always) moves away from player when near (cannot escape corners) **GOAL** using destinations
        // Level 3 AI: moves away from player when near (can escape corners)
        // for the random directional movement throw a check if the randMove is in that specific direction
        // if it is then choose a different randMove
        #region Level 2 AI:
        // choose a direction of up/down + left/right
        // or choose a direction to not go in up/down/left/right
        // find player, check which direction he is in and head in the opposite direction
        #endregion
        #region Level 3 AI:
        // if corner find the closest cordinate to player and don't go in that direction
        // i.e. If player is +3y & +5x head away using the y even if he has to go closer.
        #endregion
        #endregion
        //level 1 AI
        #region Finds the closest Wall
        GameObject[] wallList = GameObject.FindGameObjectsWithTag("Wall");
        closestWall = wallList[0];
        cannotGo.Clear();
        foreach (GameObject wall in wallList)
        {
            // if distance from here to closest wall is greater than distance from here to wall in list then
            if (Vector3.Distance(transform.position, closestWall.transform.position) > Vector3.Distance(transform.position, wall.transform.position))
            {
                closestWall = wall;
            }
            #region if wall is less than 1 away add to cannotGo List
            if (Vector2.Distance(transform.position, wall.transform.position) < 1.1)
            {
                if (wall.transform.position.y - transform.position.y > 0)
                {
                    // Add Up(2) to cannot go
                    CannotGo(2);
                }
                else
                {
                    // Add Down(3) to cannot go
                    CannotGo(3);
                }

                if (wall.transform.position.x - transform.position.x > 0)
                {
                    // Add Right(1) to cannot go
                    CannotGo(1);
                }
                else
                {
                    // Add Left(0) to cannot go
                    CannotGo(0);
                }
            }
            #endregion
        }

        //Debug.Log(Vector2.Distance(transform.position,closestWall.transform.position));
        #endregion
        #region Find Player
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj != this.gameObject)
            {
                // add check which player is closer
                #region How far is player
                playerLocation = obj.transform.position; //- transform.position;
                if (Vector2.Distance(transform.position, obj.transform.position) < 5)
                {
                    bool broken = true;
                    if (broken)
                    {
                        #region Where is Player
                        if (playerLocation.x < playerLocation.y) // is he closer on x or y
                        {
                            if (playerLocation.x < 0) // player is to the left
                            {
                                if (randMove == 0)
                                {
                                    randMove = 1;
                                }
                            }
                            else
                            {
                                if (randMove == 1)
                                {
                                    randMove = 0;
                                }
                            }
                        }
                        else
                        {
                            if (playerLocation.y < 0) // player is to the bottom
                            {
                                if (randMove == 3)
                                {
                                    randMove = 2;
                                }
                            }
                            else
                            {
                                if (randMove == 2)
                                {
                                    randMove = 3;
                                }
                            }
                        }
                        #endregion
                        #region Debug
                        //Debug.Log(Vector3.Distance(transform.position, obj.transform.position));
                        //Debug.Log("X:" + playerLocation.x);
                        //Debug.Log("Y:" + playerLocation.y);
                        //Debug.Log("Z:" + playerLocation.z);
                        #endregion
                    }
                }
                #endregion
            }
        }
        #endregion
        #region While loop to keep choosing a random move direction
        bool whileloop = false;
        if (whileloop)
        {
            do
            {
                randMove = Random.Range(0, 4);
            }
            while (cannotGo.Contains(randMove));
        }
        else
        {
            randMove = Random.Range(0, 4);
            if (cannotGo.Contains(randMove))
            {
                for (int i = 0; i < 10; i++)
                {
                    randMove = Random.Range(0, 4);
                }
            }
        }
        #endregion
        
        //tells the program which direction the player is going to move
        #region Basic Key Input

        #region if i'm not moving
        if (!isMoving)
        {
            #region AI
            if (!isAI)
            {
                KeyInput();
            }
            else
            {
                #region Random
                if (randMove == 0)
                {
                    SingleKeyInput("Left");
                }
                else if (randMove == 1)
                {
                    SingleKeyInput("Right");
                }
                else if (randMove == 2)
                {
                    SingleKeyInput("Up");
                }
                else if (randMove == 3)
                {
                    SingleKeyInput("Down");
                }

                isMoving = true;
                atNextSquare = false;
                #endregion
            }

            //Debug.Log(randMove);
            playerInput = direction;
            randMove = -1;
            timeStamp = Time.time + 2;
            #endregion
        }
        else
        {
            #region Next Movement
            if (randMove == 0)
                playerInput = GameManager.Direction.LEFT;
            else if (randMove == 1)
                playerInput = GameManager.Direction.RIGHT;
            else if (randMove == 2)
                playerInput = GameManager.Direction.UP;
            else if (randMove == 3)
                playerInput = GameManager.Direction.DOWN;
            #endregion
        }
        #endregion
        //stop the object at any time
        #region Debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerInput = GameManager.Direction.STOP;
        }
        #endregion

        //Debug.Log("Distance: " + Vector3.Distance(transform.position, nextSquare));

        #endregion
        //tells the program that the player is going to start moving
        #region If I am moving
        if (isMoving)
        {
            //Debug.Log("Player Position: " + transform.position.x + "\nNext Square Position: " + nextSquare.x);

            //if the player has not reached the position of the next square on the game board
            if (!atNextSquare)
            {
                #region if not at next square
                #region Direction Stuff
                if (direction == GameManager.Direction.LEFT)
                    transform.position += Vector3.left * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.RIGHT)
                    transform.position += Vector3.right * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.DOWN)
                    transform.position += Vector3.down * animationSpeed * Time.deltaTime;
                else if (direction == GameManager.Direction.UP)
                    transform.position += Vector3.up * animationSpeed * Time.deltaTime;
                #endregion
                //if the player has reached the next square
                if ((transform.position.x >= nextSquare.x && direction == GameManager.Direction.RIGHT) ||
                    (transform.position.x <= nextSquare.x && direction == GameManager.Direction.LEFT) ||
                    (transform.position.y <= nextSquare.y && direction == GameManager.Direction.DOWN) ||
                    (transform.position.y >= nextSquare.y && direction == GameManager.Direction.UP))
                {
                    transform.position = nextSquare;
                    atNextSquare = true;
                }
                else if (Time.time > timeStamp)
                {
                    atNextSquare = true;
                    //cannotReach = true;
                }
                //if (cannotReach)
                //{
                //    CannotGo(direction);
                //}
                //else
                //{
                //    cannotGo.Clear();
                //}
                #endregion
            }
            else
            {
                #region if at next square
                //snap player to set integer location
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);

                isMoving = false;

                //Debug.Log("Has reached next Square!");
                //check if the the player input something
                if (playerInput != direction)
                {
                    #region if player input is not a direction
                    //set the next square to the direction prompted by the user
                    if (playerInput == GameManager.Direction.LEFT)
                    {
                        SingleKeyInput("Left");
                    }
                    else if (playerInput == GameManager.Direction.RIGHT)
                    {
                        SingleKeyInput("Right");
                    }
                    else if (playerInput == GameManager.Direction.UP)
                    {
                        SingleKeyInput("Up");
                    }
                    else if (playerInput == GameManager.Direction.DOWN)
                    {
                        SingleKeyInput("Down");
                    }
                    else if (playerInput == GameManager.Direction.STOP)
                    {
                        isMoving = false;
                    }

                    //set the player to snap to an integer location
                    #endregion
                }
                else
                {
                    #region if player input is a direction
                    //get the next square
                    if (direction == GameManager.Direction.LEFT)
                        nextSquare = transform.position + Vector3.left;
                    if (direction == GameManager.Direction.RIGHT)
                        nextSquare = transform.position + Vector3.right;
                    if (direction == GameManager.Direction.UP)
                        nextSquare = transform.position + Vector3.up;
                    if (direction == GameManager.Direction.DOWN)
                        nextSquare = transform.position + Vector3.down;
                    #endregion
                }

                atNextSquare = false;
                #endregion
            }
        }
        #endregion

    }
    #endregion
    // Keep Collision for A.I.
    #region Collision
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
        }
        if (temp.tag.Equals("Wall"))
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
    #endregion
    #region Single Key Input
    void SingleKeyInput(string inputDirection)
    {
        if (inputDirection == "Left")
        {
            directMe = someDirection.Left;
        }
        else if (inputDirection == "Right")
        {
            directMe = someDirection.Right;
        }
        else if (inputDirection == "Up")
        {
            directMe = someDirection.Up;
        }
        else if (inputDirection == "Down")
        {
            directMe = someDirection.Down;
        }
        switch (directMe)
        {
            case someDirection.Left:
                direction = GameManager.Direction.LEFT;
                nextSquare = transform.position + Vector3.left;
                break;
            case someDirection.Right:
                direction = GameManager.Direction.RIGHT;
                nextSquare = transform.position + Vector3.right;
                break;
            case someDirection.Up:
                direction = GameManager.Direction.UP;
                nextSquare = transform.position + Vector3.up;
                break;
            case someDirection.Down:
                direction = GameManager.Direction.DOWN;
                nextSquare = transform.position + Vector3.down;
                break;
            default:
                break;
        }
    }
    #endregion
    #region Key Input
    void KeyInput()
    {
        //check which key was pressed and if the player can move in that direction
        if (Input.GetKeyDown(LEFT))
        {
            SingleKeyInput("Left");
            isMoving = true;
            atNextSquare = false;
        }
        else if (Input.GetKeyDown(RIGHT))
        {
            SingleKeyInput("Right");
            isMoving = true;
            atNextSquare = false;
        }
        else if (Input.GetKeyDown(UP))
        {
            SingleKeyInput("Up");
            isMoving = true;
            atNextSquare = false;
        }
        else if (Input.GetKeyDown(DOWN))
        {
            SingleKeyInput("Down");
            isMoving = true;
            atNextSquare = false;
        }
    }
    #endregion
    #region CannotGo
    void CannotGo(GameManager.Direction input)
    {
        cannotGo.Clear();
        switch (input)
        {
            case GameManager.Direction.RIGHT:
                cannotGo.Add(1);
                break;
            case GameManager.Direction.LEFT:
                cannotGo.Add(0);
                break;
            case GameManager.Direction.DOWN:
                cannotGo.Add(3);
                break;
            case GameManager.Direction.UP:
                cannotGo.Add(2);
                break;
        }
    }
    void CannotGo(int num)
    {
        if (!cannotGo.Contains(num))
        {
            cannotGo.Add(num);
        }
    }
    #endregion
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(10, 10, 1));
        Gizmos.DrawWireSphere(closestWall.transform.position, 0.5f);
        Gizmos.DrawWireSphere(playerLocation, 0.5f);
    }
}
