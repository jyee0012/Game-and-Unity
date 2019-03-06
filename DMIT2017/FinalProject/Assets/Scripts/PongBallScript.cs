using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBallScript : MonoBehaviour
{
    public Rigidbody rbody = null;
    [SerializeField]
    float moveSpeed = 2f, moveForce = 100f, rotateDelay = 0.5f;
    [SerializeField]
    Vector2 ricochetRange = Vector2.zero;
    [SerializeField]
    bool moving = false, useForce = false;

    float rotateTimeStamp = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (rbody == null) rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (useForce) {
                rbody.AddForce(transform.forward * moveForce);
            }
            else
            {
                ConstantMovement(transform.forward);
            }
        }
    }
    void ConstantMovement(Vector3 direction) {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (rotateTimeStamp < Time.time) {
            RandomRotate();
        }
    }
    void RandomRotate()
    {
        float randRange = Random.Range(ricochetRange.x, ricochetRange.y);
        transform.Rotate(new Vector3(0, 180 + randRange, 0));
        rotateTimeStamp = Time.time + rotateDelay;
    }
}
