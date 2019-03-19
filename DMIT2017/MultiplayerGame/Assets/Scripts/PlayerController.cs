using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rbody = null;
    [SerializeField]
    float movementSpeed = 2f, jumpForce = 320f;
    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;
    // Start is called before the first frame update
    void Start()
    {
        if (rbody == null)
        {
            if (GetComponent<Rigidbody>() == null)
            {
                rbody = gameObject.AddComponent<Rigidbody>();
            }
            else
            {
                rbody = GetComponent<Rigidbody>();
            }
        }
        rbody.useGravity = true;
        rbody.constraints = RigidbodyConstraints.FreezeRotation;
        rbody.drag = 1f;
        rbody.angularDrag = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0 , Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }
    void Jump()
    {
        rbody.AddForce(new Vector3(0, jumpForce, 0));
    }
}
