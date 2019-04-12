using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    [SerializeField]
    Vector2 distanceMinMax = Vector2.zero;
    [SerializeField]
    Vector3 paddleMoveDirection = Vector3.right;
    [SerializeField]
    int playerNum = 0;
    [SerializeField]
    bool useController = false, clampMovement = false, playerControlled = true;
    [SerializeField]
    float moveSpeed = 2f;

    GameObject targetBall = null;
    Vector3 startPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
	if (playerControlled)
	{
        	Movement(GetPlayerInput());
	}
	else
	{
		if (targetBall == null) targetBall = GetClosestBall();
		else
		{
			// get paddle move direction and follow the ball in that direction
		}
	}
    }
    GameObject GetClosestBall()
    {
	GameObject closestBall = null;
	foreach(GameObject ball in FindObjectsByType<PongBallScript>())
	{
		if (closestBall == null) closestBall = ball;
		if (Vector3.Distance(ball.transform.position, transform.position) < Vector3.Distance(closestBall.transform.position, transform.position))
		{
			closestBall = ball;
		}
	}
	return closestBall;
    }
    Vector3 GetPlayerInput()
    {
        #region Controller Controls
        /*
        axis1.text = Input.GetAxis("Axis1").ToString(); //left analog horizontal
        axis2.text = Input.GetAxis("Axis2").ToString(); //left analog vertical
        axis3.text = Input.GetAxis("Axis3").ToString(); //offset of triggers
        axis4.text = Input.GetAxis("Axis4").ToString(); //right analog horizontal
        axis5.text = Input.GetAxis("Axis5").ToString(); //right analog vertical
        axis6.text = Input.GetAxis("Axis6").ToString(); //d-pad horizontal
        axis7.text = Input.GetAxis("Axis7").ToString(); //d-pad vertical
        axis8.text = Input.GetAxis("Axis8").ToString();
        axis9.text = Input.GetAxis("Axis9").ToString(); //left trigger
        axis10.text = Input.GetAxis("Axis10").ToString(); //right trigger
        button0.text = Input.GetAxis("Button0").ToString(); //a button
        button1.text = Input.GetAxis("Button1").ToString(); //b button
        button2.text = Input.GetAxis("Button2").ToString(); //x button
        button3.text = Input.GetAxis("Button3").ToString(); //y button
        button4.text = Input.GetAxis("Button4").ToString(); //left bumper
        button5.text = Input.GetAxis("Button5").ToString(); //right bumper
        button6.text = Input.GetAxis("Button6").ToString(); //back button
        button7.text = Input.GetAxis("Button7").ToString(); //start button
        */
        #endregion
        float xInput = 0;
        if (useController)
        {
            xInput = Input.GetAxis("Axis1P" + playerNum);
        }
        else
        {
            xInput = Input.GetAxis("Horizontal");
        }
        Vector3 playerDirection = new Vector3(xInput, 0,0);
        return playerDirection;
    }
    void Movement(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime * moveSpeed);
        if (clampMovement)
        {
            // include checks based on axis
            // right now this only checks left-right, have it check forward/back and up/down
            Vector3 clampedPos = transform.position;
            if (transform.position.x > startPos.x + distanceMinMax.y)
            {
                clampedPos.x = startPos.x + distanceMinMax.y;
            }
            if (transform.position.x < startPos.x + distanceMinMax.x)
            {
                clampedPos.x = startPos.x + distanceMinMax.x;
            }
            transform.position = clampedPos;
        }
    }
}
