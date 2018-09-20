using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [SerializeField]
    bool bVerticalClamp;
    float vInput;
    [SerializeField]
    float rotationSpeed = 100, verticalClampMax, verticalClampMin;
    Vector3 angles;
	// Use this for initialization
	void Start ()
    {
        if (verticalClampMin < 0) verticalClampMin = 360 + verticalClampMin;

    }
	
	// Update is called once per frame
	void Update () {
        vInput = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.right, vInput * Time.deltaTime * -rotationSpeed);
        angles = transform.rotation.eulerAngles;

        if (bVerticalClamp)
        {
            angles = transform.localRotation.eulerAngles;
            if (angles.x >= verticalClampMax && angles.x < 90)
            {
                transform.localRotation = Quaternion.Euler(verticalClampMax , angles.y, angles.z);
            }
            if (angles.x <= verticalClampMin && angles.x > 270)
            {
                transform.localRotation = Quaternion.Euler(verticalClampMin, angles.y, angles.z);
            }
        }
	}
}
