using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjScript : MonoBehaviour
{
    [SerializeField]
    bool bIsAttached = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (bIsAttached) collision.transform.parent = this.transform;
    }
    void OnCollisionExit(Collision collision)
    {
        //if (bIsAttached) collision.transform.parent = null;
    }

    void OnMouseDrag()
    {
        Vector3 newMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10),
            newObjPos = Camera.main.ScreenToWorldPoint(newMousePos);
        transform.position = newObjPos;
    }
}
