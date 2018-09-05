using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Pathnode : MonoBehaviour
{
    public List<Pathnode> connections = new List<Pathnode>();

	// Update is called once per frame
	void Update ()
    {
        ShowPaths();
    }

    public void AddConnection(Pathnode node)
    {
        connections.Add(node);
    }

    void ShowPaths()
    {
        foreach (Pathnode target in connections)
        {
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z),
                           new Vector3(target.transform.position.x, target.transform.position.y + 0.25f, target.transform.position.z),
                           Color.white, 0.01f, false);
        }
    }
}
