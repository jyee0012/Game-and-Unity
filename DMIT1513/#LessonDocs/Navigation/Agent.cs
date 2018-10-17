using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public GameObject sphere;
    RaycastHit hit;
    NavMeshAgent selected;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 1000))
            {
                if (selected != null && hit.transform.tag == "Floor")
                {
                    sphere.transform.position = hit.point;
                    selected.destination = hit.point;
                }
                else if (hit.transform.tag == "Agent")
                {
                    selected = hit.transform.gameObject.GetComponent<NavMeshAgent>();
                }
            }
        }
    }
}
