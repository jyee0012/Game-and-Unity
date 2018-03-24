using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    Vector3 startingPos;
    bool falling = false, once = true;
    public float timeStamp;
    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (falling && timeStamp < Time.time)
        {
            Reset();
        }
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(WaitAndFall());
    }
    IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        falling = true;
        timeStamp = Time.time + 5f;
        StopCoroutine(WaitAndFall());
    }
    void Reset()
    {
        StopAllCoroutines();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.position = startingPos;
        falling = false;
    }
}
