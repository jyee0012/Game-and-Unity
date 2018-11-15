using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    #region Variables
    [SerializeField]
    GameObject selectedUnit, targetUnit, movementIndicator, mainCam, mapCam;
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
    List<GameObject> selectedUnits;
    [SerializeField]
    KeyCode selectUnitBtn = KeyCode.Mouse0, deselectUnitBtn = KeyCode.Mouse1, multiSelectBtn = KeyCode.LeftControl, 
        speedUpCamBtn = KeyCode.LeftShift, swapCamBtn = KeyCode.C, camZoomBtn = KeyCode.LeftShift;
    Vector3 cameraStartPos;
    Quaternion cameraStartRot;
    float vInput, hInput, rxInput, ryInput, baseMoveSpeed;
    bool bSwapCam = true;
    #endregion

    #region Start
    void Start()
    {
        cameraStartPos = transform.position;
        cameraStartRot = transform.rotation;
        if (mainCam == null) mainCam = GameObject.Find("Main Camera");
        movementIndicator.SetActive(false);
        baseMoveSpeed = movementSpeed;
        SwapCamera(true);
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
        if (selectedUnit == null || !selectedUnit.activeInHierarchy)
        {
            selectedUnit = null;
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
        CameraControls(canMoveCamera, cameraHasBoundary, mainCam);
    }
    #endregion

    #region Placed Indicators
    void PlaceIndicators()
    {

        if (selectedUnit != null)
        {
            //if (!selectedIndicator.activeInHierarchy) selectedIndicator.SetActive(true);
            if (!movementIndicator.activeInHierarchy) movementIndicator.SetActive(true);
            
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
            foreach (GameObject unit in selectedUnits)
            {
                //if (unit.GetComponent<PlayableUnits>() != null)
                unit.GetComponent<PlayableUnits>().isSelected = false;
            }
            selectedUnit.GetComponent<PlayableUnits>().isSelected = false;
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
                string debugString = hit.transform.gameObject.name;
                if (hit.transform.transform.parent != hit.transform) debugString += ":" + hit.transform.parent.gameObject.name;
                Debug.Log(debugString);
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
                    foreach (GameObject unit in selectedUnits)
                    {
                        //if (unit.GetComponent<PlayableUnits>() != null)
                        unit.GetComponent<PlayableUnits>().isSelected = false;
                    }
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
    void CameraControls(bool canMove = true, bool hasBoundaries = false, GameObject currentCam = null)
    {
        bSwapCam = (Input.GetKeyDown(swapCamBtn)) ? !bSwapCam : bSwapCam;
        SwapCamera(bSwapCam);
        if (!canMove || (currentCam == null || !currentCam.activeInHierarchy)) return;

        Cursor.lockState = CursorLockMode.Confined;

        movementSpeed = (Input.GetKey(speedUpCamBtn)) ? baseMoveSpeed * 2 : baseMoveSpeed;

        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rxInput = Input.GetAxis("Mouse X");
        ryInput = Input.GetAxis("Mouse Y");
        Vector3 newPos = new Vector3(hInput * Time.deltaTime * movementSpeed, vInput * Time.deltaTime * movementSpeed, 0);
        currentCam.transform.Translate(newPos);

        Vector3 boundedPos = currentCam.transform.position;

        if (boundedPos.x > maxBoundary.x)
            boundedPos.x = maxBoundary.x;
        if (boundedPos.x < minBoundary.x)
            boundedPos.x = minBoundary.x;
        if (boundedPos.z > maxBoundary.y)
            boundedPos.z = maxBoundary.y;
        if (boundedPos.z < minBoundary.y)
            boundedPos.z = minBoundary.y;

        if (hasBoundaries)
            currentCam.transform.position = boundedPos;


        if (Input.GetKey(camZoomBtn))
        {
            currentCam.transform.Translate(new Vector3(0, 0, ryInput * Time.deltaTime * movementSpeed));
            //transform.Rotate(Vector3.up, rxInput * Time.deltaTime * rotationSpeed);
            //transform.Rotate(Vector3.left, ryInput * Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentCam.transform.position = cameraStartPos;
            currentCam.transform.rotation = cameraStartRot;
        }
    }
    void SwapCamera(bool swapCam)
    {
        if (mainCam == null || mapCam == null) return;

        mainCam.SetActive(swapCam);
        mapCam.SetActive(!swapCam);
    }
    #endregion
}
