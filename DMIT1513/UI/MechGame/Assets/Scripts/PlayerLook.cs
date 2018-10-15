using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [SerializeField]
    bool bVerticalClamp, bSeperateClamp;
    float vInput;
    [SerializeField]
    float rotationSpeed = 100, verticalClampMax, verticalClampMin, objectClampMax, objectClampMin;
    [SerializeField]
    GameObject clampObject;

    Vector3 angles;
    bool hasObject = false;

    public bool paused = false;
    // Use this for initialization
    void Start ()
    {
        if (verticalClampMin < 0) verticalClampMin = 360 + verticalClampMin;
        if (objectClampMin < 0) objectClampMin = 360 + objectClampMin;
        if (clampObject != null) hasObject = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (paused) return;
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
        if (hasObject)
        {
            if (bSeperateClamp)
            {
                //Vector3 objAngles = clampObject.transform.rotation.eulerAngles;
                if (angles.x >= objectClampMax)
                {
                    clampObject.transform.parent = transform.parent;
                }
                else
                {
                    clampObject.transform.parent = transform;
                }
                if (angles.x <= objectClampMin && angles.x > 270)
                {
                    clampObject.transform.parent = transform.parent;
                }
                else
                {
                    clampObject.transform.parent = transform;
                }
            }
        }
    }
}
