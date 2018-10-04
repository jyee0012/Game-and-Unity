using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjScript : MonoBehaviour
{
    [SerializeField]
    bool bIsAttached = false;
    [SerializeField]
    Camera usedCamera;
    // Use this for initialization
    void Start()
    {
        if (usedCamera == null)
        {
            Camera tempCam = FindObjectOfType<Camera>();
            if (tempCam.isActiveAndEnabled) usedCamera = tempCam;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (bIsAttached)
        {
            GameObject collideObj = null;
            if (collision.gameObject.GetComponentInChildren<BoxCollider>() != null) collideObj = collision.gameObject.GetComponentInChildren<BoxCollider>().gameObject;
            if (collision.gameObject.GetComponentInChildren<SphereCollider>() != null) collideObj = collision.gameObject.GetComponentInChildren<SphereCollider>().gameObject;
            if (collision.gameObject.GetComponentInChildren<CapsuleCollider>() != null) collideObj = collision.gameObject.GetComponentInChildren<CapsuleCollider>().gameObject;
            if (collideObj != null) collideObj.transform.parent = this.transform.root;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        //if (bIsAttached) collision.transform.parent = null;
    }

    void OnMouseDrag()
    {
        Vector3 newMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10),
            newObjPos = usedCamera.ScreenToWorldPoint(newMousePos);
        transform.position = newObjPos;
    }
}
