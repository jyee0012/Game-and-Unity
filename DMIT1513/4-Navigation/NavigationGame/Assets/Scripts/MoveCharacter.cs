﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    #region Variables
    [SerializeField]
    GameObject selectedUnit, targetUnit, selectedIndicator, movementIndicator, mainCam;
    RaycastHit hit;
    [SerializeField]
    int waypointDist = 10;
    [SerializeField]
    bool canMoveCamera = true, cameraHasBoundary = false;
    [SerializeField]
    float movementSpeed = 20, rotationSpeed = 20;
    [SerializeField]
    Vector2 minBoundary = Vector2.zero, maxBoundary = Vector2.zero;
    [SerializeField]
    List<GameObject> selectedUnits, selectedIndicators;
    [SerializeField]
    KeyCode selectUnitBtn = KeyCode.Mouse0, deselectUnitBtn = KeyCode.Mouse1, multiSelectBtn = KeyCode.LeftControl;
    Vector3 cameraStartPos;
    Quaternion cameraStartRot;
    float vInput, hInput, rxInput, ryInput;
    #endregion

    #region Start
    void Start()
    {
        cameraStartPos = transform.position;
        cameraStartRot = transform.rotation;
        if (mainCam == null) mainCam = GameObject.Find("Main Camera");
        selectedIndicator.SetActive(false);
        movementIndicator.SetActive(false);
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        if (selectedUnit == null || !selectedUnit.activeInHierarchy)
        {
            selectedUnit = null;
            selectedIndicator.SetActive(false);
            movementIndicator.SetActive(false);
        }
    }
    #endregion
    #region Fixed Update
    private void FixedUpdate()
    {
        PlaceIndicators();
        SelectUnit();
        DeselectUnit();
        CameraControls(canMoveCamera, cameraHasBoundary);
    }
    #endregion

    #region Placed Indicators
    void PlaceIndicators()
    {

        if (selectedUnit != null)
        {
            //if (!selectedIndicator.activeInHierarchy) selectedIndicator.SetActive(true);
            if (!movementIndicator.activeInHierarchy) movementIndicator.SetActive(true);

            #region Selected Unit
            if (selectedIndicator != null)
            {
                //Vector3 selectedUnitBase = selectedUnit.transform.position;
                //selectedUnitBase.y -= (selectedUnit.transform.position.y / 2);
                //selectedIndicator.transform.position = selectedUnitBase;
                if (selectedUnits.Count > 0)
                {
                    selectedIndicators = new List<GameObject>(selectedUnits.Count);
                    for (int i = 0; i <= selectedUnits.Count; i++)
                    {
                        Debug.Log(i);
                        if (selectedIndicators[i] == null)
                            selectedIndicators[i] = Instantiate(selectedIndicator, selectedUnits[i].transform.position, selectedUnits[i].transform.rotation);

                        Vector3 selectedUnitBase = selectedUnits[i].transform.position;
                        selectedUnitBase.y -= (selectedUnits[i].transform.position.y / 2);
                        selectedIndicators[i].transform.position = selectedUnitBase;

                        if (!selectedIndicators[i].activeInHierarchy) selectedIndicators[i].SetActive(true);


                        if (selectedUnits[i] != selectedUnits[0] && i > 0)
                        {
                            //Debug.Log(i + ":" + selectedUnits[i].name + " = " + selectedIndicators[i-1].name);
                            //selectedIndicators[i-1] = Instantiate(selectedIndicator, selectedUnits[i].transform.position, selectedUnits[i].transform.rotation);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < selectedIndicators.Count; i++)
                {
                    Destroy(selectedIndicators[i]);
                }
                selectedIndicators.Clear();
            }
            #endregion
            #region Movement/Waypoint Indicator
            if (movementIndicator != null)
            {
                Vector3 hitPointMarker = selectedUnit.GetComponent<NavMeshAgent>().destination;
                hitPointMarker.y += waypointDist;
                movementIndicator.transform.position = hitPointMarker;
            }
            #endregion
            #region Playable Unit Script
            if (GetComponent<PlayableUnits>() != null)
            {
                GetComponent<PlayableUnits>().isSelected = true;
            }
            #endregion

        }
    }
    #endregion
    #region Deselect Unit
    void DeselectUnit()
    {
        if (Input.GetKeyDown(deselectUnitBtn) && selectedUnit != null)
        {
            selectedUnit.GetComponent<PlayableUnits>().isSelected = false;
            selectedIndicator.SetActive(false);
            movementIndicator.SetActive(false);
            selectedUnit = null;
            selectedUnits.Clear();
        }
    }
    #endregion
    #region Select Unit
    void SelectUnit()
    {
        if (Input.GetKeyDown(selectUnitBtn))
        {
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetKey(multiSelectBtn) && hit.transform.tag == "Moveable")
                {
                    if (selectedUnit != null && !selectedUnits.Contains(selectedUnit))
                        selectedUnits.Add(selectedUnit);

                    selectedUnit = hit.transform.gameObject;
                    selectedUnit.GetComponent<PlayableUnits>().isSelected = true;
                    selectedUnit.GetComponent<PlayableUnits>().WakeUp();
                    selectedUnits.Add(selectedUnit);
                }
                // Debug.Log(hit.transform.tag);
                else if (hit.transform.tag == "Moveable")
                {
                    selectedUnit = hit.transform.gameObject;
                    selectedUnit.GetComponent<PlayableUnits>().isSelected = true;
                    selectedUnit.GetComponent<PlayableUnits>().WakeUp();
                    selectedUnits.Clear();
                    selectedUnits.Add(selectedUnit);
                }
                else if (selectedUnit != null && hit.transform.tag == "EnemyUnit")
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    targetUnit = hit.transform.gameObject;
                    selectedUnit.GetComponent<PlayableUnits>().target = targetUnit;

                    foreach (GameObject unit in selectedUnits)
                    {
                        unit.GetComponent<PlayableUnits>().target = targetUnit;
                    }
                }
                else if (selectedUnit != null)
                {
                    selectedUnit.GetComponent<NavMeshAgent>().SetDestination(hit.point);

                    foreach (GameObject unit in selectedUnits)
                    {
                        unit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                    }
                }
            }
        }
    }
    #endregion
    #region Camera Controls
    void CameraControls(bool canMove = true, bool hasBoundaries = false)
    {
        if (!canMove) return;

        Cursor.lockState = CursorLockMode.Confined;

        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rxInput = Input.GetAxis("Mouse X");
        ryInput = Input.GetAxis("Mouse Y");
        Vector3 newPos = new Vector3(hInput * Time.deltaTime * movementSpeed, vInput * Time.deltaTime * movementSpeed, 0);
        transform.Translate(newPos);

        Vector3 boundedPos = transform.position;

        if (transform.position.x > maxBoundary.x)
            boundedPos.x = maxBoundary.x;
        if (transform.position.x < minBoundary.x)
            boundedPos.x = minBoundary.x;
        if (transform.position.z > maxBoundary.y)
            boundedPos.z = maxBoundary.y;
        if (transform.position.z < minBoundary.y)
            boundedPos.z = minBoundary.y;

        transform.position = boundedPos;


        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(new Vector3(0, 0, ryInput * Time.deltaTime * movementSpeed));
            //transform.Rotate(Vector3.up, rxInput * Time.deltaTime * rotationSpeed);
            //transform.Rotate(Vector3.left, ryInput * Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = cameraStartPos;
            transform.rotation = cameraStartRot;
        }
    }
    #endregion
}
