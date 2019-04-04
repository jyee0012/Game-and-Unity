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
            tInput = Input.GetAxis("Axis1P" + playerNum);
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
        angles = weaponMount.transform.rotation.eulerAngles;
        if (hCanClamp)
        {
            if (angles.x >= hClampMinMax.y)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(hClampMinMax.y, angles.y, angles.z);
            }
            if (angles.x <= hClampMinMax.x && angles.x > 270)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(hClampMinMax.x, angles.y, angles.z);
            }
        }
        if (vCanClamp)
        {
            if (angles.x >= vClampMinMax.y)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(vClampMinMax.y, angles.y, angles.z);
            }
            if (angles.x <= vClampMinMax.x && angles.x > 270)
            {
                weaponMount.transform.localRotation = Quaternion.Euler(vClampMinMax.x, angles.y, angles.z);
            }
        }
    }
}
