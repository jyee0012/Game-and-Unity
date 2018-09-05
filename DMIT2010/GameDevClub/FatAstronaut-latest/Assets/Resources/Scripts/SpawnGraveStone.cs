using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnGraveStone : MonoBehaviour {

    public Vector3 endLocation;
    public Vector3 startLocation;

    public bool isMoving;

    public float risingSpeed = 1.0f;
	
    public void SetLocation(Vector3 location)
    {
        endLocation = location;
        startLocation = location - new Vector3(0, 2, 0);
        transform.position = startLocation;

        gameObject.SetActive(true);

        isMoving = true;
    }

	// Update is called once per frame
	void Update () {
		if (isMoving)
        {
            transform.position += Vector3.up * risingSpeed * Time.deltaTime;

            if (transform.position.y >= endLocation.y)
            {
                transform.position = endLocation;
                isMoving = false;

                //Load the Death Scene
                SceneManager.LoadScene("Death_Scene");
            }
        }
	}
}
