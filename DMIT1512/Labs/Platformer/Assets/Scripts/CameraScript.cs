using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        if(player.transform.position.y < 8)
        {
            transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 12f, transform.position.z);
        }
    }
}
