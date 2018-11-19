using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField]
    bool bTest = false, bDestroyInstant = false, useTeamTag = false, useRoot = false;
    public AudioSource boomSound;
    public ParticleSystem explosionEffect;

    public bool bCanExplode = false;
    public float explodeRadius = 2, explodeDmg = 5, destroyDelay = 3;
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
    public void Explode(Transform explodeTarget = null)
    {
        if (!bCanExplode) return;
        //transform.localScale *= explodeRadius;
        if (explodeTarget == null)
        {
            bool hitSomething = false;
            Collider[] explosionHit = Physics.OverlapSphere(transform.position, explodeRadius);
            foreach (Collider obj in explosionHit)
            {
                string objTag = "";
                Debug.Log(obj.name);
                Debug.Log("I am " + bombOwner + " and I hit " + obj.transform.gameObject.name + " and they have a tag of " + obj.transform.tag);
                if (obj.tag == "Moveable" || obj.tag == "EnemyUnit")
                {
                    objTag = obj.tag;
                    HitTarget(obj.transform, objTag);
                }
                if (objTag != "") hitSomething = true;
            }
            if (!hitSomething) return;
        }
        else
            HitTarget(explodeTarget);

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
        CollideTarget(collision.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        CollideTarget(other.transform);
    }
    void CollideTarget(Transform collisionTarget)
    {
        //Debug.Log(collisionTarget.parent.gameObject.name + " ignores " + ignoreObj.name);
        //collisionTarget.GetComponentInParent<PlayableUnits>().gameObject != null;
        //if (collisionTarget.root.gameObject == ignoreObj) return;
        //if (useTeamTag && collisionTarget.root.gameObject.tag == ignoreObj.tag) return;
        Transform playUnit = FindPlayableUnit(collisionTarget).transform;
        //Debug.Log("I am " + bombOwner + " and I hit " + collisionTarget.parent.gameObject.name);
        
        if (!bCanExplode) HitTarget(collisionTarget.parent);
        else Explode(collisionTarget.parent);
        //Debug.Log(collision.gameObject.name);
        //TrueDestroy();
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
        Destroy(gameObject, destroyDelay);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    void HitTarget(Transform obj, string targetTag = "Target")
    {
        //Debug.Log(bombOwner.name + " hit -> " + obj.name);
        obj = FindPlayableUnit(obj).transform;
        if (obj == null) return;
        //if (obj.transform.tag != targetTag) return;
        if (useRoot)
        {
            if (obj.root.tag != targetTag) return;
            if (obj.root.tag == targetTag) obj = obj.root;
        }
        //Debug.Log(obj.name);
        //Debug.Log("I hit something");

        PlayableUnits unit = obj.GetComponent<PlayableUnits>();
        if (unit != null && bombOwner != null) unit.target = bombOwner;

        HealthScript hp = obj.GetComponent<HealthScript>();
        if (hp != null) hp.Damage(explodeDmg, false);
        else Destroy(obj.gameObject);
        //Destroy(obj);
    }
    GameObject FindPlayableUnit(Transform transform)
    {
        GameObject playableUnit = null;
        PlayableUnits tempUnit = transform.GetComponentInParent<PlayableUnits>();
        if (tempUnit != null)

            playableUnit = tempUnit.gameObject;
        else
        {
            tempUnit = transform.GetComponentInChildren<PlayableUnits>();
            if (tempUnit != null)
                playableUnit = tempUnit.gameObject;
        }
        return playableUnit;
    }
    GameObject FindPlayableUnit(GameObject obj)
    {
        return FindPlayableUnit(obj.transform);
    }
}
