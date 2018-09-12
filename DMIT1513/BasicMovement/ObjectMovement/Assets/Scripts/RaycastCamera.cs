using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCamera : MonoBehaviour {

    [SerializeField]
    GameObject target = null;

    RaycastHit hit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Physics.Raycast(transform.position, transform.forward, out hit, 30.0f))
        {
            target.transform.position = hit.point;
        }
	}
}
