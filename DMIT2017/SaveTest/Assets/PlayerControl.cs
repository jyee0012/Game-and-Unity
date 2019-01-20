using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    float vInput, hInput, movementSpeed = 2f;

    [SerializeField]
    KeyCode jumpKey = KeyCode.Space;
    
    #region Start
    void Start()
    {
    }
    #endregion
    #region Update
    // Update is called once per frame
    void Update()
    {
    }
    #endregion
    #region Fixed Update
    private void FixedUpdate()
    {

    }
    #endregion

    void PlayerMovement()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(hInput * Time.deltaTime * movementSpeed, vInput * Time.deltaTime * movementSpeed, 0));
    }
}
