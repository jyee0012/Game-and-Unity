using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    float xMaxClamp, xMinClamp, yMaxClamp, yMinClamp, zMaxClamp, zMinClamp, movementSpeed = 2;

    [SerializeField]
    bool xMove, yMove, zMove, bIsAttached;

    Vector3 startPos;

    public bool paused = false;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (paused) return;
		if (xMove)
        {
            MovePlatform("x", xMinClamp, xMaxClamp);
        }
        if (yMove)
        {
            MovePlatform("y", yMinClamp, yMaxClamp);
        }
        if (zMove)
        {
            MovePlatform("z", zMinClamp, zMaxClamp);
        }

    }
    void MovePlatform(string axis, float min, float max)
    {
        switch(axis)
        {
            case "x":
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
                if (transform.position.x >= startPos.x + max && transform.position.x < startPos.x + (max *2))
                {
                    // transform.position = new Vector3(startPos.x + max, transform.position.y, transform.position.z);
                    movementSpeed *= -1;
                }
                if (transform.position.x <= startPos.x + min && transform.position.x < startPos.x + (min + min))
                {
                    //transform.position = new Vector3(startPos.x + min, transform.position.y, transform.position.z);
                    movementSpeed *= -1;
                }
                break;
            case "y":
                transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
                if (transform.position.y >= startPos.y + max && transform.position.y < startPos.y + (max * 2))
                {
                    //transform.position = new Vector3( transform.position.x, startPos.y + max, transform.position.z);
                    movementSpeed *= -1;
                }
                if (transform.position.y <= startPos.y + min && transform.position.y < startPos.y + (min + min))
                {
                    //transform.position = new Vector3(transform.position.x, startPos.y + min, transform.position.z);
                    movementSpeed *= -1;
                }
                break;
            case "z":
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
                if (transform.position.z >= startPos.z + max && transform.position.z < startPos.z + (max * 2))
                {
                    //transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + max);
                    movementSpeed *= -1;
                }
                if (transform.position.z <= startPos.z + min && transform.position.z < startPos.z + (min + min))
                {
                    //transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + min);
                    movementSpeed *= -1;
                }
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (bIsAttached) collision.transform.parent = this.transform;
    }
    void OnCollisionExit(Collision collision)
    {
        if (bIsAttached)  collision.transform.parent = null;
    }
}
