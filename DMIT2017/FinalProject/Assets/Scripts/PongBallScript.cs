using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongBallScript : MonoBehaviour
{
    [SerializeField, TextArea]
    string ballDesc = "";
    public GameObject lastHit = null, lastPlayerHit = null;
    public Rigidbody rbody = null;
    public float moveSpeed = 2f, moveForce = 100f, rotateDelay = 0.5f, moveDelay = 1, respawnDelay = 5, destructionRange = 100;
    [SerializeField]
    Vector2 ricochetRange = Vector2.zero;
    [SerializeField]
    bool moving = false, useForce = false, canRotate = false, additiveForce = false;
    [SerializeField]
    AudioSource hitSound = null, playerHitSound = null;


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
    bool respawning = false;
    Vector3 startPos = Vector3.zero;
    float rotateTimeStamp = 0, moveTimeStamp = 0, gravityTimeStamp = 0, respawnTimeStamp = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (rbody == null) rbody = GetComponent<Rigidbody>();
        RandomRotate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > destructionRange) Destroy(gameObject);
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
        if (respawning && respawnTimeStamp < Time.time)
        {
            EnableDisableSelf(true);
            ResetPos();
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
            lastHit = collision.gameObject;
            if (hitSound != null) hitSound.PlayOneShot(hitSound.clip);
            if (collision.gameObject.GetComponent<PaddleScript>() != null)
            {
                lastPlayerHit = collision.gameObject;
                if (playerHitSound != null) playerHitSound.PlayOneShot(playerHitSound.clip);
            }
            if (additiveForce)
            {
                rbody.AddForce(transform.forward * moveForce);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Goal")
        {
            Destroy(gameObject);
        }
    }
    void EnableDisableSelf(bool activeSelf = true)
    {
        MeshRenderer meshRender = GetComponent<MeshRenderer>();
        Collider collide = GetComponent<Collider>();
        rbody.velocity = Vector3.zero;

        meshRender.enabled = activeSelf;
        collide.enabled = activeSelf;
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
