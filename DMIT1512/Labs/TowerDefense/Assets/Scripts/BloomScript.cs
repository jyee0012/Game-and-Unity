using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomScript : MonoBehaviour
{

    float timeActivity;
    public bool isActive { get; set; }
    // Use this for initialization
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (isActive)
            {
                timeActivity = Time.time + 6f;
                isActive = false;
            }
            if (timeActivity < Time.time && !isActive)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            isActive = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
