using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjScript : MonoBehaviour
{
    [SerializeField]
    bool bIsAttached = false;
    [SerializeField]
    Camera usedCamera;

    GameObject objectRoot;
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
            if (collision.transform.tag == "Floor") return;
            if (objectRoot == null)
            {
                CheckAndSetObjectGroup(collision);
            }

            // if other object doesn't have root then use our root
            SetObjectRoot(collision.gameObject, objectRoot);
        }
    }
    void CheckAndSetObjectGroup(Collision collision)
    {
        if (this.gameObject.transform.root.tag == "ObjectGroup")
        {
            objectRoot = gameObject.transform.root.gameObject;
        }
        else if (collision.gameObject.transform.root.tag == "ObjectGroup")
        {
            objectRoot = collision.gameObject.transform.root.gameObject;
            this.gameObject.transform.parent = objectRoot.transform;
        }
        else
        {
            objectRoot = new GameObject();
            objectRoot.tag = "ObjectGroup";
            this.gameObject.transform.parent = objectRoot.transform;
        }
    }
    void SetObjectRoot(GameObject target, GameObject root)
    {
        if (root.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rbody = root.AddComponent<Rigidbody>();
            rbody.drag = 1;
            rbody.angularDrag = 1;
            //rbody.useGravity = false;
        }
        //if (root.GetComponent<BaseObjScript>() == null)
        //{
        //    BaseObjScript baseObj = root.AddComponent<BaseObjScript>();
        //    baseObj.bIsAttached = true;
        //}
        if (target.tag != "ObjectGroup")
        {
            if (target.GetComponent<Rigidbody>() != null)
            {
                Destroy(target.GetComponent<Rigidbody>());
            }
            else if (target.GetComponentInChildren<Rigidbody>() != null)
            {
                Destroy(target.GetComponentInChildren<Rigidbody>());
            }
        }
        if (root.transform.tag == "ObjectGroup") target.transform.parent = root.transform;
    }
    void SetObjectRootLocation()
    {
        if (objectRoot == null) return;
        Vector3 furthest, closest, difference;
        for (int i = 0; i < objectRoot.transform.childCount; i++)
        {
            objectRoot.transform.GetChild(i);
            // find the middle of all children
            // find the transforms and save the smaller of the pair
            // find the distance between the two and divide by 2
            // add half distance to the smaller transform to get inbetween
            // repeat for all children
            //if (objectRoot.transform.GetChild(i).transform.position > furthest)
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
        if (this.gameObject.GetComponent<BlackHoleScript>() != null)
        {
            BlackHoleScript bhScript = this.gameObject.GetComponent<BlackHoleScript>();
            bhScript.activeBH = true;
        }
        if (this.gameObject.GetComponent<FanScript>() != null)
        {
            FanScript fanScript = this.gameObject.GetComponent<FanScript>();
            fanScript.turnOnFan = true;
        }
    }
}
