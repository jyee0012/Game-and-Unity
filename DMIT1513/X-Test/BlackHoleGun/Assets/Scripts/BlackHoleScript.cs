using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour {

    [SerializeField]
    float exploRadius = 5, forceMod = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        BlackHole(exploRadius, forceMod);
	}

    void BlackHole(float explosionRadius, float pullForce)
    {
        Collider[] explosion = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider obj in explosion)
        {
            Vector3 objPos = obj.transform.position;
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
    
}
