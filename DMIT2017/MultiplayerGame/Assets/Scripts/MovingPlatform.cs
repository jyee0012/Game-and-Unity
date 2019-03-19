using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    string Notes = "-Will only interact with objects with a rigidbody in the parent /n-Will require the box collider to be modified to fit model /n-Will ignore model's collider";
    [Space]

    [SerializeField]
    GameObject prefabModel = null;
    [SerializeField]
    [Tooltip("Draws Lines for direction platform will head \nblue:forward/red:right/green:up")]
    bool drawGizmo = false;
    [SerializeField]
    bool drawDestination = false;
    [SerializeField]
    [Tooltip("How far to move it from the starting position \nX:Right - Y:Up - Z:Forward")]
    Vector3 destination = Vector3.zero;
    [SerializeField]
    float movementSpeed = 2f;


    Vector3 startPos = Vector3.zero, finalPos = Vector3.zero, directionalVec = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        finalPos = startPos + destination;
        if (destination.x == 0) directionalVec.x = 0;
        if (destination.y == 0) directionalVec.y = 0;
        if (destination.z == 0) directionalVec.z = 0;
        if (prefabModel != null) ReplaceModel();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        Vector3 moveVec = new Vector3(directionalVec.x * movementSpeed * Time.deltaTime, directionalVec.y * movementSpeed * Time.deltaTime, directionalVec.z * movementSpeed * Time.deltaTime);
        transform.Translate(moveVec);

        CheckForBoundary();
    }
    void CheckForBoundary()
    {
        if (destination.x > 0)
        {
            if (transform.position.x > finalPos.x || transform.position.x < startPos.x) directionalVec.x *= -1;
        }
        else
        {
            if (transform.position.x < finalPos.x || transform.position.x > startPos.x) directionalVec.x *= -1;
        }
        if (destination.y > 0)
        {
            if (transform.position.y > finalPos.y || transform.position.y < startPos.y) directionalVec.y *= -1;
        }
        else
        {
            if (transform.position.y < finalPos.y || transform.position.y > startPos.y) directionalVec.y *= -1;
        }
        if (destination.z > 0)
        {
            if (transform.position.z > finalPos.z || transform.position.z < startPos.z) directionalVec.z *= -1;
        }
        else
        {
            if (transform.position.z < finalPos.z || transform.position.z > startPos.z) directionalVec.z *= -1;
        }
    }
    void ReplaceModel()
    {
        if (GetComponentsInChildren<MeshRenderer>() != null)
        {
            foreach(MeshRenderer mRender in GetComponentsInChildren<MeshRenderer>())
            {
                mRender.enabled = false;
            }
        }
        if (prefabModel != null)
        {
            GameObject model = Instantiate(prefabModel, transform.position, transform.rotation, transform);
            foreach (BoxCollider collider in model.GetComponentsInChildren<BoxCollider>())
            {
                collider.enabled = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        AttachObject(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        AttachObject(collision.gameObject, false);
    }
    void AttachObject(GameObject attachObj, bool attach = true)
    {
        if (attachObj.GetComponent<Rigidbody>() == null) return;
        if (attach)
        {
            if (attachObj.transform.parent != transform)
            {
                attachObj.transform.parent = transform;
            }
        }
        else
        {
            if (attachObj.transform.parent == transform)
            {
                attachObj.transform.parent = null;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * 10);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 10);
        }
        if (drawDestination)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(finalPos, GetComponent<BoxCollider>().size);
        }
    }
}
