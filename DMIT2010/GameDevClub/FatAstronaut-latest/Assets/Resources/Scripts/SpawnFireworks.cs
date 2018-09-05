using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireworks : MonoBehaviour {

    public int MaxFireworks;
    public int MinFireworks;

    public float interval = 0.5f;
    public float currentIntervalTime;

    public void Start()
    {
        currentIntervalTime = Time.time + interval;
    }

    // Update is called once per frame
    void Update () {

        if (Time.time >= currentIntervalTime)
        {
            int numFireworks = Random.Range(MinFireworks, MaxFireworks);

            for (int i = 0; i < numFireworks; i++)
            {
                float xPos = Random.Range(-5.5f, 5.5f);
                float yPos = Random.Range(-4.2f, 4.2f);

                GameObject fireworks = (GameObject)Instantiate(Resources.Load("Prefabs/Fireworks"));

                fireworks.transform.position = new Vector3(xPos, yPos, 0.0f);
                fireworks.SetActive(true);
            }

            currentIntervalTime = Time.time + interval;
        }
	}
}
