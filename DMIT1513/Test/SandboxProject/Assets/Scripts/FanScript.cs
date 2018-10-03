using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FanScript : MonoBehaviour
{

    [SerializeField]
    GameObject rotatingPart;
    [SerializeField]
    float windDist = 5, forceMod = 500, rotateSpeed = 2;

    Rigidbody ownRbody;
    // Use this for initialization
    void Start()
    {
        ownRbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hitObjs = Physics.SphereCastAll(transform.position, (transform.localScale.x / 2), transform.forward, windDist);
        if (hitObjs != null) BlowWind(hitObjs);
        RotateBlades();
    }
    void BlowWind(RaycastHit[] hitArray)
    {
        foreach (RaycastHit hit in hitArray)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            float dist = Vector3.Distance(transform.position, hit.transform.position),
                calculatedForce = forceMod * (1 - (dist / windDist));
            Rigidbody hitRBody = hit.transform.gameObject.GetComponent<Rigidbody>();
            // 1 = 100%
            // windDist = 1%;
            // calculate dist %;
            // 1 - dist / windDist;
            if (hitRBody != null) hitRBody.AddForce(transform.forward * calculatedForce);
        }
    }
    void RotateBlades()
    {
        if (rotatingPart == null) return;
        Quaternion newRot= rotatingPart.transform.localRotation;
        newRot.z *= Time.deltaTime * rotateSpeed;
        rotatingPart.transform.rotation = newRot;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (transform.forward * windDist), 1);
    }
}
