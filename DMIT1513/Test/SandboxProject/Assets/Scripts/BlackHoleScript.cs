using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour {

    [SerializeField]
    float exploRadius = 5, forceMod = 500, delayDestroy = 3;

    public bool activeBH = false;
    bool once = true;
    float timeStamp;
	// Use this for initialization
	void Start () {
        if (delayDestroy < 0) delayDestroy = 3;
	}
	
	// Update is called once per frame
	void Update () {
        if (activeBH) BlackHole(exploRadius, forceMod);
        if (!once && timeStamp < Time.time) Destroy(this.gameObject, 0.1f); 
	}

    void BlackHole(float explosionRadius, float pullForce)
    {
        if (once)
        {
            timeStamp = Time.time + delayDestroy;
            once = false;
        }
        Collider[] explosion = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider obj in explosion)
        {
            Vector3 objPos = obj.transform.position;
            if (obj.gameObject == this.gameObject) continue;
            if (obj.GetComponent<Rigidbody>() != null)
            {
                Rigidbody objRbody = obj.GetComponent<Rigidbody>();
                //obj.transform.LookAt(transform);
                Vector3 pullVector = (obj.transform.position - transform.position);
                //Debug.Log(pullVector);
                pullVector *= -pullForce;
                objRbody.AddForce(pullVector);
            }
        }
    }
    void OnDestroy()
    {
        if (transform.parent == null) transform.DetachChildren();
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.parent = transform.parent;
            }
        }
    }
    
}
