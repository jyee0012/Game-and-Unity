using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int id;
    public float movementSpeed = 1.0f;

    public float GetMovementSpeed() { return movementSpeed; }
    public int getID() { return id; }

    public void setMovementSpeed(float speed) { movementSpeed = speed; }
}
