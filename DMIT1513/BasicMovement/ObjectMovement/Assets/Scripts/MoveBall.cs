using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {

    public float forceModifier = 1f;
    public bool bCanMove = true;
    bool bHasRigidbody = false;
    Rigidbody rbody;
    [SerializeField]
    float vInput, hInput;
	// Use this for initialization
	void Start () {
		if (this.GetComponent<Rigidbody>() != null)
        {
            bHasRigidbody = true;
            rbody = GetComponent<Rigidbody>();
        }
	}

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");

        if (bCanMove)
        {
            rbody.AddForce(hInput * forceModifier, 0, vInput * forceModifier);
        }
    }
}
