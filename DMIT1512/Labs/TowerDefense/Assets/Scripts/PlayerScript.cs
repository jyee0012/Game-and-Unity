using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // Trigger Game Over
            GameObject.Find("PrefabPool").GetComponent<PrefabPool>().win = false;
            GameObject.Find("PrefabPool").GetComponent<PrefabPool>().DisableEverything();
            GameManager.SLoadGameSpace(2);
        }
    }
}
