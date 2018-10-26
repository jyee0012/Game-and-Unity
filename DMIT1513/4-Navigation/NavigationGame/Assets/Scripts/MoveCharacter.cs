using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject selectedUnit, selectedIndicator, movementIndicator, mainCam;
    RaycastHit hit;
    [SerializeField]
    int waypointDist = 10;

    void Start()
    {
        if (mainCam == null) mainCam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        PlaceIndicators();
    }
    #region Placed Indicators
    void PlaceIndicators()
    {
        if (selectedUnit != null)
        {
            #region Selected Unit
            if (selectedIndicator != null)
            {
                Vector3 selectedUnitBase = selectedUnit.transform.position;
                selectedUnitBase.y -= (selectedUnit.transform.position.y / 2);
                selectedIndicator.transform.position = selectedUnitBase;
            }
            #endregion
            #region Movement/Waypoint Indicator
            if (movementIndicator != null)
            {
                Vector3 hitPointMarker = hit.point;
                hitPointMarker.y += waypointDist;
                movementIndicator.transform.position = hitPointMarker;
            }
            #endregion
        }
    }
    #endregion
    void DeselectUnit()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            selectedUnit = null;
        }
    }
    void SelectUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Debug.Log(hit.transform.tag);
                if (hit.transform.tag == "Moveable")
                {
                    selectedUnit = hit.transform.gameObject;
                }
                else if (selectedUnit != null)
                {
                    selectedUnit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                }
            }
        }
    }
}
