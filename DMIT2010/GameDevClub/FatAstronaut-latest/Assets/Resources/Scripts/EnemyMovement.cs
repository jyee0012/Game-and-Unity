using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public ConstantForce gravity;
    public PlayerMovement.Direction direction;
    //GameObject player;

    public float moveSpeed = 1.0f;

    private void Start()
    {
        gravity = gameObject.AddComponent<ConstantForce>();
        gravity.force = new Vector3(0, -9.81f, 0);
    }

    // Update is called once per frame
    void Update ()
    {

        //player = GameObject.FindGameObjectWithTag("Player");

        //move and face the correct direction
        if (direction == PlayerMovement.Direction.LEFT)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Wall"))
            SwitchDirection();
    }

    private void SwitchDirection()
    {
        switch (direction)
        {
            case PlayerMovement.Direction.LEFT:
                direction = PlayerMovement.Direction.RIGHT;
                break;
            case PlayerMovement.Direction.RIGHT:
                direction = PlayerMovement.Direction.LEFT;
                break;
            default:
                direction = PlayerMovement.Direction.LEFT;
                break;
        }
    }
}
