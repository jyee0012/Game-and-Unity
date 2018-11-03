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
    public GameObject ignoreObj, spawnOther, bombOwner = null;
    Vector3 ownerPos;
    // Use this for initialization
    void Start()
    {
        if (ignoreObj == null)
        {
            if (transform.root != null) ignoreObj = transform.root.gameObject;
        }
        if (bombOwner != null) ownerPos = bombOwner.transform.position;
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
    public Vector3 GetOwnerPos()
    {
        return ownerPos;
    }
    public void Explode()
    {
        if (!bCanExplode) return;
        //transform.localScale *= explodeRadius;
        bool hitSomething = false;
        Collider[] explosionHit = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach(Collider obj in explosionHit)
        {
            string objTag = "";
            if (obj.tag == "Moveable" || obj.tag == "EnemyUnit") objTag = obj.tag;
            HitTarget(obj.transform, objTag);
            if (objTag != "") hitSomething = true;
        }
        if (!hitSomething) return;

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
        if (collision.transform.root.gameObject == ignoreObj) return;
        Explode();
        if (!bCanExplode) HitTarget(collision.transform);
        //Debug.Log(collision.gameObject.name);
        TrueDestroy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == ignoreObj) return;
        Explode();
        if (!bCanExplode) HitTarget(other.transform);
        //Debug.Log(collision.gameObject.name);
        TrueDestroy();
    }
    private void OnDestroy()
    {
        //Explode();
        if (spawnOther == null) return;
        GameObject thing = Instantiate(spawnOther, transform.position, Quaternion.identity);
        Destroy(thing, 3);
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
    void HitTarget(Transform obj, string targetTag = "Target")
    {
        if (obj.transform.tag != targetTag || obj.root.tag != targetTag) return;
        if (obj.root.tag == targetTag) obj = obj.root;
        //Debug.Log(obj.name);
        //Debug.Log("I hit something");

        PlayableUnits unit = obj.GetComponent<PlayableUnits>();
        if (unit != null && bombOwner != null) unit.target = bombOwner;

        HealthScript hp = obj.GetComponent<HealthScript>();
        if (hp != null) hp.Damage(explodeDmg, false);
        else Destroy(obj.gameObject);
        //Destroy(obj);
    }
}
