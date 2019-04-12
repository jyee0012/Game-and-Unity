using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

    [SerializeField]
    bool vCanClamp = false, hCanClamp = false;
    [SerializeField]
    float rotationSpeed = 100;
    [SerializeField]
    Vector2 vClampMinMax = Vector2.zero, hClampMinMax = Vector2.zero;
    [SerializeField]
    GameObject weaponMount;
    public bool useController = true;
    public int playerNum = 1;
    
    float tInput, wInput;
    Vector3 angles;

    // Use this for initialization
    void Start ()
    {
        if (hClampMinMax.x < 0) hClampMinMax.x = 360 + hClampMinMax.x;
        if (vClampMinMax.x < 0) vClampMinMax.x = 360 + vClampMinMax.x;
    }
	
	// Update is called once per frame
	void Update () {
        TurretMovement();
        TurretClamp();
	}
    void TurretMovement()
    {
        if (useController)
        {
            tInput = Input.GetAxis("Axis1P" + playerNum) * -1;
            wInput = Input.GetAxis("Axis2P" + playerNum);
        }
        else
        {
            tInput = Input.GetAxis("Horizontal") * -1;
            wInput = Input.GetAxis("Vertical");
        }
        transform.Rotate(Vector3.up, tInput * Time.deltaTime * -rotationSpeed);
        if (weaponMount != null)
        {
            weaponMount.transform.Rotate(Vector3.right, wInput * Time.deltaTime * -rotationSpeed);
        }
    }
    void TurretClamp()
    {
        if (!vCanClamp && !hCanClamp) return;
        if (hCanClamp)
        {
            angles = transform.localRotation.eulerAngles;
            //Debug.Log("Angles: " + angles + " | ");
            if (angles.y >= hClampMinMax.y && angles.y < 180)
            {
                transform.localRotation = Quaternion.Euler(angles.x, hClampMinMax.y, angles.z);
            }
            if (angles.y <= hClampMinMax.x && angles.y > 180)
            {
                transform.localRotation = Quaternion.Euler(angles.x, hClampMinMax.x, angles.z);
            }
        }
        if (vCanClamp)
        {
            angles = weaponMount.transform.localRotation.eulerAngles;
            if (angles.x <= vClampMinMax.x && angles.x > 180)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(vClampMinMax.x, 0, angles.z);
            }
            if (angles.x >= vClampMinMax.y && angles.x < 180)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(vClampMinMax.y, 0, angles.z);
            }
        }
    }
}
