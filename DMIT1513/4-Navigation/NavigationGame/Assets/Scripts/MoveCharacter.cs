using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MoveCharacter : MonoBehaviour
{
    private int fingerID = -1;
    private void Awake()
    {
        #if !UNITY_EDITOR
             fingerID = 0; 
        #endif
    }
    #region Variables
    [SerializeField]
    GameObject selectedUnit, targetUnit, movementIndicator, mainCam, mapCam, regionMang, multiselectIndicator;
    RaycastHit hit;
    [SerializeField]
    int waypointDist = 10;
    [SerializeField]
    bool canMoveCamera = true, cameraHasBoundary = false, isRegionMap = false, autoSwapCam = false;
    [SerializeField]
    float movementSpeed = 20, rotationSpeed = 20;
    [SerializeField]
    Vector2 minBoundary = Vector2.zero, maxBoundary = Vector2.zero;
    [SerializeField]
    List<GameObject> selectedUnits;
    [SerializeField]
    KeyCode selectUnitBtn = KeyCode.Mouse0, deselectUnitBtn = KeyCode.Mouse1, multiSelectBtn = KeyCode.LeftControl, 
        speedUpCamBtn = KeyCode.LeftShift, swapCamBtn = KeyCode.C, camZoomBtn = KeyCode.LeftShift;
    Vector3 cameraStartPos, mouseStartPos = Vector3.zero, mouseEndPos = Vector3.zero, mouseCurrentPos = Vector3.zero;
    Quaternion cameraStartRot;
    float vInput, hInput, rxInput, ryInput, baseMoveSpeed;
    bool bSwapCam = true;
    GameObject currentCam = null;
    #endregion

    #region Start
    void Start()
    {
        if (mainCam == null) mainCam = GameObject.Find("Main Camera");
        if (regionMang == null) regionMang = GameObject.Find("RegionManager");

        cameraStartPos = mainCam.transform.position;
        cameraStartRot = mainCam.transform.rotation;
        movementIndicator.SetActive(false);
        multiselectIndicator.SetActive(false);
        baseMoveSpeed = movementSpeed;
        SwapCamera(true);
        if (autoSwapCam)
        {
            bSwapCam = !bSwapCam;
        }
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
        MassSelectUnit();
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
            DeselectAllUnits();
            movementIndicator.SetActive(false);
            selectedUnit = null;
        }
    }
    #endregion
    #region Select Unit
    void SelectUnit()
    {
        if (Input.GetKeyDown(selectUnitBtn))
        {
            bool hitUI = false;
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject(fingerID))    // is the touch on the GUI
            {
                // GUI Action
                hitUI = true;
            }
            if (Physics.Raycast(ray, out hit) && !hitUI)
            {
                //string debugString = hit.transform.gameObject.name;
                //if (hit.transform.transform.parent != null && hit.transform.transform.parent != hit.transform) debugString += ":" + hit.transform.parent.gameObject.name;
                //Debug.Log(debugString);
                if (regionMang != null && regionMang.GetComponent<RegionManager>() != null && isRegionMap)
                {
                    RegionManager tempRegMang = regionMang.GetComponent<RegionManager>();
                    if (hit.transform.gameObject != null) tempRegMang.CheckIfRegion(hit.transform.gameObject);
                    if (hit.transform.parent != null) tempRegMang.CheckIfRegion(hit.transform.parent.gameObject);
                    if (hit.transform.root.gameObject != null) tempRegMang.CheckIfRegion(hit.transform.root.gameObject);
                }
                if (Input.GetKey(multiSelectBtn) && hit.transform.tag == "Moveable")
                {
                    selectedUnit = hit.transform.gameObject;
                    AddUnitToSelected(selectedUnit);
                }
                // Debug.Log(hit.transform.tag);
                else if (hit.transform.tag == "Moveable")
                {
                    selectedUnit = hit.transform.gameObject;
                    DeselectAllUnits();
                    AddUnitToSelected(selectedUnit);
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
    void MassSelectUnit()
    {
        Vector3 rayDirection = -Vector3.up;
        bool canBoxCast = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            multiselectIndicator.SetActive(true);
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                mouseStartPos = hit.point;
                //Debug.Log("MouseDown: " + mouseStartPos);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            multiselectIndicator.SetActive(false);
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                mouseEndPos = hit.point;
                //Debug.Log("MouseUp: " + mouseEndPos);
                rayDirection = ray.direction;
                canBoxCast = true;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                mouseCurrentPos = hit.point;
            }
            if (multiselectIndicator != null)
            {
                Vector3 multiselectCenter = Vector3.Lerp(mouseStartPos, mouseCurrentPos, 0.5f), 
                    multiselectSize = mouseStartPos - mouseCurrentPos;

                multiselectCenter.y = mouseStartPos.y + 1;
                multiselectSize = new Vector3(Mathf.Abs(multiselectSize.x), 1, Mathf.Abs(multiselectSize.z));
                multiselectIndicator.transform.position = multiselectCenter;
                multiselectIndicator.transform.localScale = multiselectSize;
            }
        }
        if (canBoxCast)
        {
            Vector3 mouseCenter = Vector3.Lerp(mouseStartPos, mouseEndPos, 0.5f),
                mouseSize = mouseStartPos - mouseEndPos;
            mouseCenter.y = mouseStartPos.y + 1;
            mouseSize = new Vector3(Mathf.Abs(mouseSize.x), mouseStartPos.y, Mathf.Abs(mouseSize.z));
            RaycastHit[] allHits = Physics.BoxCastAll(mouseCenter, mouseSize / 2, rayDirection);
            //Debug.Log("BoxCast: true | Length: " + allHits.Length + " | ");

            foreach (RaycastHit boxHit in allHits)
            {
                if (boxHit.transform.tag == "Moveable")
                {
                    GameObject boxHitObj = boxHit.transform.gameObject;

                    if (boxHitObj != null && !selectedUnits.Contains(boxHitObj))
                    {
                        if (selectedUnit == null) selectedUnit = boxHitObj;
                        AddUnitToSelected(boxHitObj);
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

        if (boundedPos.y > 200)
            boundedPos.y = 200;
        if (boundedPos.y < 0)
            boundedPos.y = 0;
        currentCam.transform.position = boundedPos;

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
    void AddUnitToSelected(GameObject newUnit)
    {
        if (newUnit != null && !selectedUnits.Contains(newUnit))
        {
            newUnit.GetComponent<PlayableUnits>().isSelected = true;
            newUnit.GetComponent<PlayableUnits>().WakeUp();
            selectedUnits.Add(newUnit);
        }

    }
    void DeselectAllUnits()
    {
        foreach (GameObject unit in selectedUnits)
        {
            //if (unit.GetComponent<PlayableUnits>() != null)
            unit.GetComponent<PlayableUnits>().isSelected = false;
        }
        selectedUnits.Clear();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        /* lets say mouse start is 113,512 and mouse end is 142,300
         * mouseStartX - mouseEndX = -30
         */
        Vector3 mouseCenter = Vector3.Lerp(mouseStartPos, mouseEndPos, 0.5f),
            mouseSize = mouseStartPos - mouseEndPos;
        mouseCenter.y = mouseStartPos.y + 1;
        mouseSize = new Vector3(Mathf.Abs(mouseSize.x), mouseStartPos.y, Mathf.Abs(mouseSize.z));
        Gizmos.DrawCube(mouseCenter,mouseSize);

    }
}
