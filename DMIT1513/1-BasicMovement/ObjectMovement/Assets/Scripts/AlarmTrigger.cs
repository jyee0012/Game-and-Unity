using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour {

    [SerializeField]
    Light alarmLight;
    [SerializeField]
    GameObject target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Physics.Linecast(transform.position, target.transform.position))
        {
            alarmLight.color = Color.red;
        }
        else
        {
            alarmLight.color = Color.white;
        }
	}
}
