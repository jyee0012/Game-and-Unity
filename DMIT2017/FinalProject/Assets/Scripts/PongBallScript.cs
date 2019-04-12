using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongBallScript : MonoBehaviour
{
    public Rigidbody rbody = null;
    [SerializeField]
    float moveSpeed = 2f, moveForce = 100f, rotateDelay = 0.5f, moveDelay = 1;
    [SerializeField]
    Vector2 ricochetRange = Vector2.zero;
    [SerializeField]
    bool moving = false, useForce = false, canRotate = false, reset = false, additiveForce = false;

    Vector3 startPos = Vector3.zero;
    float rotateTimeStamp = 0, moveTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (rbody == null) rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            ResetPos(reset);
            reset = !reset;
        }
        if (moving && moveTimeStamp < Time.time)
        {
            if (useForce)
            {
                rbody.AddForce(transform.forward * moveForce);
            }
            else
            {
                ConstantMovement(transform.forward);
            }
            moveTimeStamp = Time.time + moveDelay;
        }
    }
    void ConstantMovement(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Paddle")
        {
            Debug.Log("Before: " + transform.rotation.eulerAngles);
            transform.rotation = collision.transform.rotation;
            RandomRotate();
            Debug.Log("After: " + transform.rotation.eulerAngles);
            if (additiveForce)
            {
                rbody.AddForce(transform.forward * moveForce);
            }
        }
    }
    void RandomRotate()
    {
        float randRangeX = Random.Range(ricochetRange.x, ricochetRange.y);
        float randRangeY = Random.Range(ricochetRange.x, ricochetRange.y);
        float randRangeZ = Random.Range(ricochetRange.x, ricochetRange.y);
        transform.Rotate(new Vector3(randRangeX, randRangeY, randRangeZ));
        rotateTimeStamp = Time.time + rotateDelay;
    }
    void ResetPos(bool reset = false)
    {
        if (reset)
        {
            transform.position = startPos;
        }
    }
}
