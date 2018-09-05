using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[CustomEditor(typeof(PathnodeContainer))]
public class PathnodeContainerEditor : Editor
{

    private static bool m_editMode = false;
    private static int m_count = 0;
    private static GameObject m_container;

    void OnSceneGUI()
    {
        if (m_editMode)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;


                if (Physics.Raycast(worldRay, out hitInfo))
                {
                    GameObject pathnode = Resources.Load("PathNode") as GameObject;
                    GameObject pathnodeInstance = Instantiate(pathnode) as GameObject;
                    pathnodeInstance.transform.position = hitInfo.point;
                    pathnodeInstance.name = "Pathnode" + m_count.ToString();
                    pathnodeInstance.transform.parent = GameObject.Find("PathnodeCreator").transform;
                    //PathNode pathnodeScript = pathnodeInstance.GetComponent("PathNode") as PathNode;

                    EditorUtility.SetDirty(pathnodeInstance);

                    m_count++;
                }

            }
        }

    }
    public override void OnInspectorGUI()
    {
        if (m_editMode)
        {
            if (GUILayout.Button("Disable Editing"))
            {
                m_editMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("Enable Editing"))
            {
                m_editMode = true;

                // GET Current number of waypoints currently in window and start from there
                UnityEngine.Object[] pathnodes = FindObjectsOfType(typeof(PathNode));
                m_count = pathnodes.Length;
                if (m_count < 0) m_count = 0;

                m_container = GameObject.Find("PathnodeCreator");

            }
        }

        if (GUILayout.Button("Reset Node Count"))
        {
            m_count = 0;
        }

        if (GUILayout.Button("Delete All Nodes"))
        {
            for (int i = 0; i < m_container.transform.childCount;)
            {
                Destroy(m_container.transform.GetChild(0).gameObject);
            }
        }

    }
}
