using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ObjectRotation : MonoBehaviour {

    #region Variables
    [SerializeField]
    float movementSpeed = 2f, 
        verticalClampMin, verticalClampMax, horizontalClampMin, horizontalClampMax,
        maxCraneVertical, minCraneVertical;
    [SerializeField]
    bool bConstantRotation = true, 
        bHorizontalRotation = true, bVerticalRotation = true, 
        bHorizontalClamp = false, bVerticalClamp = false,
        bEnableCraneControls = false;
    [SerializeField]
    GameObject CraneHand = null;
    [SerializeField]
    KeyCode leftKey, rightKey, upKey, downKey, lowerCraneKey, raiseCraneKey;

    [SerializeField]
    AudioSource mechMove;
    public bool paused = false;
    #endregion

    #region Start
    // Use this for initialization
    void Start () {

		if (verticalClampMin < 0) verticalClampMin = 360 + verticalClampMin;
        
        if (horizontalClampMin < 0) horizontalClampMin = 360 + horizontalClampMin;

        if (upKey == KeyCode.None) upKey = KeyCode.UpArrow;
        
        if (downKey == KeyCode.None) downKey = KeyCode.DownArrow;

        if (leftKey == KeyCode.None) leftKey = KeyCode.Q;

        if (rightKey == KeyCode.None) rightKey = KeyCode.E;

        if (bEnableCraneControls)
        {
            if (lowerCraneKey == KeyCode.None) lowerCraneKey = KeyCode.L;
            
            if (raiseCraneKey == KeyCode.None) raiseCraneKey = KeyCode.O;
        }
        if (mechMove != null)
        {
            PlayMechMove();
            PauseMechMove();
        }
	}
    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        if (paused) return;
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
            Vector3 angles = transform.localRotation.eulerAngles;
            if (Input.GetKey(leftKey))
            {
                transform.Rotate(Vector3.up * -movementSpeed);
                if (bHorizontalClamp)
                {
                    angles = transform.localRotation.eulerAngles;
                    if (angles.y <= horizontalClampMin && angles.y > 90)
                    {
                        transform.localRotation = Quaternion.Euler(angles.x, horizontalClampMin, angles.z);
                    }
                }
                ResumeMechMove();
            }
            if (Input.GetKey(rightKey))
            {
                transform.Rotate(Vector3.up * movementSpeed);
                if (bHorizontalClamp)
                {
                    angles = transform.localRotation.eulerAngles;
                    if (angles.y >= horizontalClampMax && angles.y < 270)
                    {
                        transform.localRotation = Quaternion.Euler(angles.x, horizontalClampMax, angles.z);
                    }
                }

                ResumeMechMove();
            }
            if (!(Input.GetKey(rightKey) && Input.GetKey(leftKey)))
            {
                PauseMechMove();
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
                    angles = transform.localRotation.eulerAngles;
                    if (angles.z >= verticalClampMax && angles.z < 90)
                    {
                        transform.localRotation = Quaternion.Euler(angles.x, angles.y, verticalClampMax);
                    }
                }
            }
            if (Input.GetKey(downKey))
            {
                transform.Rotate(Vector3.forward * -movementSpeed);
                if (bVerticalClamp)
                {
                    angles = transform.localRotation.eulerAngles;
                    if (angles.z <= verticalClampMin && angles.z > 270)
                    {
                        transform.localRotation = Quaternion.Euler(angles.x, angles.y, verticalClampMin);
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

    void PlayMechMove()
    {
        if (mechMove == null) return;
        // mechMove.loop = true;
        mechMove.Play();
    }
    void PauseMechMove()
    {
        if (mechMove == null) return;
        mechMove.Pause();
        mechMove.Stop();
    }
    void ResumeMechMove()
    {
        if (mechMove == null) return;
        mechMove.UnPause();
        mechMove.Play();
    }
}
