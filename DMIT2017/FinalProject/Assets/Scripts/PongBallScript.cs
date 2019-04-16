using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongBallScript : MonoBehaviour
{
    [SerializeField, TextArea]
    string ballDesc = "";
    public Rigidbody rbody = null;
    public float moveSpeed = 2f, moveForce = 100f, rotateDelay = 0.5f, moveDelay = 1;
    [SerializeField]
    Vector2 ricochetRange = Vector2.zero;
    [SerializeField]
    bool moving = false, useForce = false, canRotate = false, additiveForce = false;


    [Header("Gravity Ball Settings")]
    [SerializeField]
    bool gravityBall = false;
    [SerializeField]
    bool gravityPull = true;
    [SerializeField]
    float gravityRange = 3, gravityDelay = 1;
    
    [Header("Debug")]
    [SerializeField]
    bool reset = false;

    // Private Variables
    Vector3 startPos = Vector3.zero;
    float rotateTimeStamp = 0, moveTimeStamp = 0, gravityTimeStamp = 0;

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
        if (gravityBall && gravityTimeStamp < Time.time)
        {
            GravityWell(gravityRange);
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
            //Debug.Log("Before: " + transform.rotation.eulerAngles);
            transform.rotation = collision.transform.rotation;
            RandomRotate();
            //Debug.Log("After: " + transform.rotation.eulerAngles);
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
    void GravityWell(float gravityRange = 3)
    {
        List<PongBallScript> pongBalls = new List<PongBallScript>();
        foreach (Collider obj in Physics.OverlapSphere(transform.position, gravityRange))
        {
            if (obj.GetComponent<PongBallScript>() != null && obj != gameObject) pongBalls.Add(obj.GetComponent<PongBallScript>());
        }
        foreach (PongBallScript ball in pongBalls)
        {
            ball.transform.LookAt(transform.position);
            if (gravityPull)
            {
                ball.rbody.AddForce(ball.transform.forward * ball.moveForce);
            }
        }
        gravityTimeStamp = Time.time + gravityDelay;
    }
    private void OnDrawGizmos()
    {
        if (gravityBall)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.position, gravityRange);
        }
    }
}
