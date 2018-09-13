﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {

    #region Variables
    public float movementSpeed = 2f, 
        verticalClampMin, verticalClampMax, horizontalClampMin, horizontalClampMax,
        maxCraneVertical, minCraneVertical;
    public bool bConstantRotation = true, 
        bHorizontalRotation = true, bVerticalRotation = true, 
        bHorizontalClamp = false, bVerticalClamp = false,
        bEnableCraneControls = false;
    public GameObject CraneHand = null;
    public KeyCode upKey, downKey, lowerCraneKey, raiseCraneKey;
    #endregion
    
    #region Start
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
        if (bEnableCraneControls)
        {
            if (lowerCraneKey == KeyCode.None)
            {
                lowerCraneKey = KeyCode.L;
            }
            if (raiseCraneKey == KeyCode.None)
            {
                raiseCraneKey = KeyCode.O;
            }
        }
	}
    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        ControlledRotation();
        if (CraneHand != null && bVerticalRotation) 
        {
            Quaternion newRotation = CraneHand.transform.rotation;
            newRotation.z = 360 - transform.rotation.z;
            CraneHand.transform.rotation = newRotation;
            CraneControls(bEnableCraneControls);
        }
	}
    #endregion

    #region Controlled Rotation
    void ControlledRotation()
    {
        if (!bConstantRotation && !bHorizontalRotation && !bVerticalRotation)
        {
            return;
        }

        if (bConstantRotation)
        {
            transform.Rotate(Vector3.up * movementSpeed);
        }
        else if (bHorizontalRotation)
        {
            Vector3 angles = transform.rotation.eulerAngles;
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(Vector3.up * -movementSpeed);
                if (bHorizontalClamp)
                {
                    angles = transform.rotation.eulerAngles;
                    if (angles.y <= horizontalClampMin && angles.y > 90)
                    {
                        transform.rotation = Quaternion.Euler(angles.x, horizontalClampMin, angles.z);
                    }
                }
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(Vector3.up * movementSpeed);
                if (bHorizontalClamp)
                {
                    angles = transform.rotation.eulerAngles;
                    if (angles.y >= horizontalClampMax && angles.y < 90)
                    {
                        transform.rotation = Quaternion.Euler(angles.x, horizontalClampMax, angles.z);
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

    #region Crane Controls
    void CraneControls(bool canControl)
    {
        if (!canControl)
        {
            return;
        }
        else
        {
            if (Input.GetKey(lowerCraneKey))
            {
                CraneHand.transform.Translate(Vector3.up * Time.deltaTime * -movementSpeed);
                if (CraneHand.transform.position.y <= minCraneVertical && CraneHand.transform.position.y >= (minCraneVertical + minCraneVertical))
                {
                    Vector3 newLocation = CraneHand.transform.position;
                    newLocation.y = minCraneVertical;
                    CraneHand.transform.position = newLocation;
                }
                
            }
            if (Input.GetKey(raiseCraneKey))
            {
                CraneHand.transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
                if (CraneHand.transform.position.y >= maxCraneVertical && CraneHand.transform.position.y <= (maxCraneVertical *-1))
                {
                    Vector3 newLocation = CraneHand.transform.position;
                    newLocation.y = maxCraneVertical;
                    CraneHand.transform.position = newLocation;
                }
            }
        }
    }
    #endregion
}