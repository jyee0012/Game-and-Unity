using UnityEngine;
using System.Collections;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    static GameObject[] otherNodes;

    [MenuItem("Grid Generation/Build Paths")]

    static void BuildPathNetwork()
    {
        otherNodes = GameObject.FindGameObjectsWithTag("Pathnode");

        for (int i = 0; i < otherNodes.Length; i++)
        {
            for (int j = 0; j < otherNodes.Length; j++)
            {
                if (Vector3.Distance(otherNodes[i].transform.position, otherNodes[j].transform.position) <= 20.0f
                    && otherNodes[i] != otherNodes[j])
                {
                    otherNodes[i].GetComponent<Pathnode>().AddConnection(otherNodes[j].GetComponent<Pathnode>());
                    EditorUtility.SetDirty(otherNodes[i].GetComponent<Pathnode>());
                }
            }
        }
    }

}
