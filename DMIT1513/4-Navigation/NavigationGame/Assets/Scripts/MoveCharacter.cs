using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject selectedUnit, targetUnit, selectedIndicator, movementIndicator, mainCam;
    RaycastHit hit;
    [SerializeField]
    int waypointDist = 10;
    [SerializeField]
    bool canMoveCamera = true, cameraHasBoundary = false;
    [SerializeField]
    float movementSpeed = 20, rotationSpeed = 20;

    Vector3 cameraStartPos;
    Quaternion cameraStartRot;
    float vInput, hInput, rxInput, ryInput;

    void Start()
    {
        cameraStartPos = transform.position;
        cameraStartRot = transform.rotation;
        if (mainCam == null) mainCam = GameObject.Find("Main Camera");
        selectedIndicator.SetActive(false);
        movementIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlaceIndicators();
        SelectUnit();
        DeselectUnit();
        CameraControls(canMoveCamera);
    }
    #region Placed Indicators
    void PlaceIndicators()
    {

        if (selectedUnit != null)
        {
            if (!selectedIndicator.activeInHierarchy) selectedIndicator.SetActive(true);
            if (!movementIndicator.activeInHierarchy) movementIndicator.SetActive(true);

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
            #region Playable Unit Script
            if (GetComponent<PlayableUnits>() != null)
            {
                GetComponent<PlayableUnits>().isSelected = true;
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
                else if (selectedUnit != null && hit.transform.tag == "EnemyUnit")
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    targetUnit = hit.transform.gameObject;
                    selectedUnit.GetComponent<PlayableUnits>().target = hit.transform.gameObject;
                }
                else if (selectedUnit != null)
                {
                    selectedUnit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                }
            }
        }
    }
    void CameraControls(bool canMove = true)
    {
        if (!canMove) return;

        Cursor.lockState = CursorLockMode.Confined;

        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        rxInput = Input.GetAxis("Mouse X");
        ryInput = Input.GetAxis("Mouse Y");
        Vector3 newPos = new Vector3(hInput * Time.deltaTime * movementSpeed, vInput * Time.deltaTime * movementSpeed, 0);
        transform.Translate(newPos);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(new Vector3(0,0,ryInput * Time.deltaTime * movementSpeed));
            //transform.Rotate(Vector3.up, rxInput * Time.deltaTime * rotationSpeed);
            //transform.Rotate(Vector3.left, ryInput * Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = cameraStartPos;
            transform.rotation = cameraStartRot;
        }
    }
}
