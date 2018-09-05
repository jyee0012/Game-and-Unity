using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    GameManager.Direction playerFacing;
    public Vector2 direction;

    public bool isMoving;
    public float movementSpeed = 1.0f;
    public Vector3 speedCap;

    public float slowDownFactor = 0.5f;
    public bool isSlowing = false;

    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;

    public bool onSand;

	// Update is called once per frame
	void Update () {
        GetInput();

        Debug.Log("Magnitude: " + GetComponent<Rigidbody2D>().velocity.magnitude);

        if (isSlowing)
        {
            GetComponent<Rigidbody2D>().velocity *= slowDownFactor;
            if (GetComponent<Rigidbody2D>().velocity.magnitude <= new Vector2(0.01f, 0.01f).magnitude)
            {
                isSlowing = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
	}

    private void ApplyForce(Vector2 dir)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        direction = dir;

        if (rb.velocity.magnitude < GetComponent<PlayerStats>().speedCap.magnitude)
            rb.AddForce(direction * GetComponent<PlayerStats>().movementSpeed);
        else
            rb.velocity = GetComponent<PlayerStats>().speedCap; 
    }

    private void GetInput()
    {
        //directional movement
        if (Input.GetKey(leftKey))
        {
            if (direction != Vector2.left)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ApplyForce(Vector2.left);
            isSlowing = false;
        }

        if (Input.GetKey(rightKey))
        {
            if (direction != Vector2.right)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ApplyForce(Vector2.right);
            isSlowing = false;
        }

        if (Input.GetKey(downKey))
        {
            if (direction != Vector2.down)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ApplyForce(Vector2.down);
            isSlowing = false;
        }

        if (Input.GetKey(upKey))
        {
            if (direction != Vector2.up)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ApplyForce(Vector2.up);
            isSlowing = false;
        }

        if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey) || Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey))
        {
            isSlowing = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject temp = collision.gameObject;
        Debug.Log("STOP");

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
                    Debug.Log("Transporting...");
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag.Equals("Sand") && !onSand)
        {
            onSand = true;
            GetComponent<PlayerStats>().speedCap /= 2.0f;
            GetComponent<Rigidbody2D>().velocity /= 2.0f;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag.Equals("Sand") && onSand)
        {
            onSand = false;
            GetComponent<PlayerStats>().speedCap *= 2.0f;
            GetComponent<Rigidbody2D>().velocity *= 2.0f;
        }
    }
}
