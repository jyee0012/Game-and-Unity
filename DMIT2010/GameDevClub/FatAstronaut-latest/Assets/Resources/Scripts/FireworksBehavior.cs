using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksBehavior : MonoBehaviour {

    public float deathTime = 2.0f;

    public float currentDeathTime;

    public void Start()
    {
        currentDeathTime = Time.time + deathTime;
    }

    // Update is called once per frame
    void Update () {


		if (!GetComponent<ParticleSystem>().IsAlive())
            Destroy(this);
        else
        {
            if (Time.time >= currentDeathTime)
                Destroy(this);
        }
	}
}
