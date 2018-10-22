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
    public bool reverse, turnOnFan = false;
    // Use this for initialization
    void Start()
    {
        ownRbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOnFan) {
            Vector3 blowPt = transform.position + (transform.forward * 0.8f) + (transform.forward * (transform.localScale.x / 2));
            RaycastHit[] hitObjs = Physics.SphereCastAll(blowPt, (transform.localScale.x / 2), transform.forward, windDist);
           if (hitObjs != null) BlowWind(hitObjs);
        }
        RotateBlades();
    }
    void BlowWind(RaycastHit[] hitArray)
    {
        foreach (RaycastHit hit in hitArray)
        {
            if (hit.transform.root == this.gameObject) continue;
            float dist = Vector3.Distance(transform.position, hit.transform.position),
                calculatedForce = forceMod * (1 - (dist / windDist));
            Rigidbody hitRBody = null;
            if (hit.transform.root.gameObject.GetComponent<Rigidbody>() != null) hitRBody = hit.transform.root.gameObject.GetComponent<Rigidbody>();
            if (hitRBody == null)
            {
                if (hit.transform.root.gameObject.GetComponentInChildren<Rigidbody>() != null) hitRBody = hit.transform.root.gameObject.GetComponentInChildren<Rigidbody>();
            }
            // 1 = 100%
            // windDist = 1%;
            // calculate dist %;
            // 1 - dist / windDist;
            if (hitRBody != null)
            {
                hitRBody.velocity = Vector3.zero;
                hitRBody.AddRelativeForce(transform.forward * calculatedForce);
                if (dist > windDist) hitRBody.velocity = Vector3.zero;
            }
        }
    }
    void RotateBlades()
    {
        if (rotatingPart == null) return;
        rotatingPart.transform.Rotate(Vector3.forward * rotateSpeed, Space.Self);
        if (reverse)
        {
            if (rotateSpeed > 0)
            {
                rotateSpeed *= -1;
            }
        }
        else if (rotateSpeed < 0)
        {
            rotateSpeed *= -1;
        }
        //rotatingPart.transform.localRotation = Quaternion.Euler((Vector3.forward * rotateSpeed * Time.deltaTime) + rotatingPart.transform.localRotation.eulerAngles);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (transform.forward * windDist), 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (transform.forward * 0.8f) + (transform.forward * (transform.localScale.x / 2)), transform.localScale.x / 2);
    }
}
