using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {

    public float movementSpeed = 2f, verticalClampMin, verticalClampMax, horizontalClampMin, horizontalClampMax;
    public bool bConstantRotation = true, bHorizontalRotation = true, bVerticalRotation = true, bHorizontalClamp = false, bVerticalClamp = false;
    public GameObject CraneHand = null;
    public KeyCode upKey, downKey;
	// Use this for initialization
	void Start () {
		if (verticalClampMin < 0)
        {
            verticalClampMin = 360 + verticalClampMin;
        }
        if (horizontalClampMin < 0)
        {
            horizontalClampMin = 360 + horizontalClampMin;
        }
        if (upKey == KeyCode.None)
        {
            upKey = KeyCode.UpArrow;
        }
        if (downKey == KeyCode.None)
        {
            downKey = KeyCode.DownArrow;
        }
	}
	
	// Update is called once per frame
	void Update () {
        ControlledRotation();
        if (CraneHand != null && bVerticalRotation) 
        {
            Quaternion newRotation = new Quaternion(CraneHand.transform.rotation.x, CraneHand.transform.rotation.y, transform.rotation.z * -1, CraneHand.transform.rotation.w);
            CraneHand.transform.rotation = newRotation;
        }
	}
    #region Controlled Rotation
    void ControlledRotation()
    {
        if (bConstantRotation)
        {
            transform.Rotate(Vector3.up * movementSpeed);
        }
        else if (bHorizontalRotation)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(Vector3.up * -movementSpeed);
                if (bHorizontalClamp)
                {
                    if (transform.rotation.y >= horizontalClampMax)
                    {
                        Quaternion clampedRotation = transform.rotation;
                        clampedRotation.y = horizontalClampMax;
                        transform.rotation = clampedRotation;
                    }
                }
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(Vector3.up * movementSpeed);
                if (bHorizontalClamp)
                {
                    if (transform.rotation.y <= horizontalClampMin)
                    {
                        Quaternion clampedRotation = transform.rotation;
                        clampedRotation.y = horizontalClampMin;
                        transform.rotation = clampedRotation;
                    }
                }
            }
        }
        else if (bVerticalRotation)
        {
            Vector3 angles = transform.rotation.eulerAngles;
            //Debug.Log(angles);

            if (Input.GetKey(upKey))
            {
                transform.Rotate(Vector3.forward * movementSpeed);
                if (bVerticalClamp)
                {
                    angles = transform.rotation.eulerAngles;
                    if (angles.z >= verticalClampMax && angles.z < 90)
                    {
                        transform.rotation = Quaternion.Euler(angles.x, angles.y, verticalClampMax);
                    }
                }
            }
            if (Input.GetKey(downKey))
            {
                transform.Rotate(Vector3.forward * -movementSpeed);
                if (bVerticalClamp)
                {
                    angles = transform.rotation.eulerAngles;
                    if (angles.z <= verticalClampMin && angles.z > 270)
                    {
                        transform.rotation = Quaternion.Euler(angles.x, angles.y, verticalClampMin);
                    }
                }
            }
        }
    }
    #endregion
}
