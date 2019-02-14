using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDoorScript : MonoBehaviour
{

    public enum DirectionalMovement { DontMove, MoveUp, MoveDown, MoveLeft, MoveRight }
    public enum DirectionalRotation { DontRotate, RotateCW, RotateCCW }
    [SerializeField]
    GameObject moveableObject = null;
    [SerializeField]
    DirectionalMovement movement = DirectionalMovement.DontMove;
    [SerializeField]
    Vector2 movementLimit = Vector2.zero;
    [SerializeField]
    DirectionalRotation rotate = DirectionalRotation.DontRotate;
    [SerializeField]
    Vector2 rotationLimit = Vector2.zero;
    [SerializeField]
    float timer = 0, movementSpeed = 2f, rotationSpeed = 2f;

    Vector3 objStartPos = Vector3.zero, translateVec = Vector3.zero, rotationVec = Vector3.zero;
    Quaternion objStartRot = Quaternion.identity;
    bool onTrigger = false;

    // Use this for initialization
    void Start()
    {
        if (moveableObject != null)
        {
            objStartPos = moveableObject.transform.position;
            objStartRot = moveableObject.transform.rotation;
        }
        translateVec = Movement(movement);
        rotationVec = Rotation(rotate);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveableObject != null)
        {
            if (onTrigger)
            {
                Move();
            }
            else if (CheckCanMoveRotate(movement, rotate))
            {
                Move(-1);
            }
        }
    }
    bool CheckCanMoveRotate(DirectionalMovement move, DirectionalRotation rot)
    {
        bool canMove = false;
        if (move != DirectionalMovement.DontMove)
        {
            canMove = Vector3.Distance(objStartPos, moveableObject.transform.position) > 0.1f;
        }
        if (rot != DirectionalRotation.DontRotate)
        {
            canMove = Vector3.Distance(objStartRot.eulerAngles, moveableObject.transform.rotation.eulerAngles) < 1;
        }
        return canMove;
    }
    void Move(float generalMultiplier = 1)
    {
        moveableObject.transform.Translate(translateVec * movementSpeed * Time.deltaTime * generalMultiplier);
        moveableObject.transform.Rotate(rotationVec * rotationSpeed * Time.deltaTime * generalMultiplier);
    }
    Vector3 Movement(DirectionalMovement move)
    {
        Vector3 directionalTranslate = Vector3.zero;
        switch (move)
        {
            case DirectionalMovement.MoveDown:
                directionalTranslate = Vector3.up * -1;
                break;
            case DirectionalMovement.MoveUp:
                directionalTranslate = Vector3.up;
                break;
            case DirectionalMovement.MoveLeft:
                directionalTranslate = Vector3.left;
                break;
            case DirectionalMovement.MoveRight:
                directionalTranslate = Vector3.right;
                break;
        }
        return directionalTranslate;
    }
    Vector3 Rotation(DirectionalRotation rot)
    {
        Vector3 directionRotation = Vector3.zero;
        switch (rot)
        {
            case DirectionalRotation.RotateCCW:
                directionRotation = Vector3.left;
                break;
            case DirectionalRotation.RotateCW:
                directionRotation = Vector3.left * -1;
                break;
        }
        return directionRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onTrigger = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onTrigger = false;
        }
    }
}
