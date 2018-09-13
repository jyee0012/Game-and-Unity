using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerScript : MonoBehaviour {

    [SerializeField]
    GameObject target;
    bool bCanTarget = false;
	// Use this for initialization
	void Start () {
		if (target != null)
        {
            bCanTarget = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (bCanTarget)
        {
            transform.LookAt(target.transform);
        }
	}
}
