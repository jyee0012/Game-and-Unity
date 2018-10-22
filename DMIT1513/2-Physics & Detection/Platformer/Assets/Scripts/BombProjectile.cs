using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField]
    bool bTest;

    public bool bCanExplode = false;
    public float explodeRadius = 2;
    public GameObject ignoreObj;
    // Use this for initialization
    void Start()
    {
        if (ignoreObj == null)
        {
            if (transform.parent != null) ignoreObj = transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bTest)
        {
            if (Input.GetKeyDown(KeyCode.G)) transform.localScale *= explodeRadius;
            if (Input.GetKeyDown(KeyCode.H)) transform.localScale /= explodeRadius;
        }
    }
    public void Explode()
    {
        if (!bCanExplode) return;
        transform.localScale *= explodeRadius;
        Collider[] explosionHit = Physics.OverlapSphere(transform.position, transform.localScale.x);
        foreach(Collider obj in explosionHit)
        {
            if (obj.transform.tag == "Target")
            {
                Destroy(obj);
            }
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ignoreObj) return;
        Explode();
        if (collision.transform.tag == "Target")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        Explode();
    }
}
