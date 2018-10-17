using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField]
    bool bTest = false, bDestroyInstant = false;
    public AudioSource boomSound;
    public ParticleSystem explosionEffect;

    public bool bCanExplode = false;
    public float explodeRadius = 2, explodeDmg = 5;
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
        //transform.localScale *= explodeRadius;
        Collider[] explosionHit = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider obj in explosionHit)
        {
            HitTarget(obj.transform);
        }
        if (boomSound != null)
        {
            boomSound.loop = false;
            boomSound.Play();
        }
        if (explosionEffect != null)
        {
            ParticleSystem explodeEffect = Instantiate(explosionEffect, transform.position, transform.rotation, transform);
            explodeEffect.Play();
        }
        bCanExplode = false;
        TrueDestroy();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ignoreObj) return;
        Explode();
        if (!bCanExplode) HitTarget(collision.transform);
        //Debug.Log(collision.gameObject.name);
        TrueDestroy();
    }
    private void OnDestroy()
    {
        Explode();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
    void TrueDestroy()
    {

        if (bDestroyInstant)
        {
            Destroy(gameObject);
            return;
        }
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (GetComponent<SphereCollider>() != null)
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        Destroy(gameObject, 3f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    void HitTarget(Transform obj)
    {
        if (obj.transform.tag != "Target") return;
        if (obj.parent.tag == "Target") obj = obj.parent;
        //Debug.Log(obj.name);
        //Debug.Log("I hit something");
        HealthScript hp = obj.GetComponent<HealthScript>();
        if (hp != null) hp.Damage(explodeDmg, false);
        else Destroy(obj.gameObject);
        //Destroy(obj);
    }
}
