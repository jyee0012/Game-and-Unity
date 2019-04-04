using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleMovement : MonoBehaviour
{

    [SerializeField]
    float movementSpeed = 2f, health, maxHealth = 10;
    [SerializeField]
    bool useController = true;

    public int playerNum = 1;

    float leftTread, rightTread, vInput, hInput;
    Vector3 startPos;
    Quaternion startRot;
    int enemyScore = 0;
    // Use this for initialization
    void Start()
    {
        //GetAllControllers();
        startPos = transform.position;
        startRot = transform.rotation;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        BaseMovement();
    }
    void BaseMovement()
    {
        #region Controller Controls
        /*
        axis1.text = Input.GetAxis("Axis1").ToString(); //left analog horizontal
        axis2.text = Input.GetAxis("Axis2").ToString(); //left analog vertical
        axis3.text = Input.GetAxis("Axis3").ToString(); //offset of triggers
        axis4.text = Input.GetAxis("Axis4").ToString(); //right analog horizontal
        axis5.text = Input.GetAxis("Axis5").ToString(); //right analog vertical
        axis6.text = Input.GetAxis("Axis6").ToString(); //d-pad horizontal
        axis7.text = Input.GetAxis("Axis7").ToString(); //d-pad vertical
        axis8.text = Input.GetAxis("Axis8").ToString();
        axis9.text = Input.GetAxis("Axis9").ToString(); //left trigger
        axis10.text = Input.GetAxis("Axis10").ToString(); //right trigger
        button0.text = Input.GetAxis("Button0").ToString(); //a button
        button1.text = Input.GetAxis("Button1").ToString(); //b button
        button2.text = Input.GetAxis("Button2").ToString(); //x button
        button3.text = Input.GetAxis("Button3").ToString(); //y button
        button4.text = Input.GetAxis("Button4").ToString(); //left bumper
        button5.text = Input.GetAxis("Button5").ToString(); //right bumper
        button6.text = Input.GetAxis("Button6").ToString(); //back button
        button7.text = Input.GetAxis("Button7").ToString(); //start button
        */
        #endregion
        if (useController)
        {
            leftTread = Input.GetAxis("Axis2P" + playerNum);
            rightTread = Input.GetAxis("Axis5P" + playerNum);

            transform.Translate(-Vector3.forward * (leftTread + rightTread) * Time.deltaTime * movementSpeed);
            transform.Rotate(Vector3.up * (-leftTread + rightTread));
        }
        else
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.forward * (vInput) * Time.deltaTime * movementSpeed);
            transform.Rotate(Vector3.up * (hInput));
        }
        if (transform.position.y < -10) Respawn();
    }
    void GetAllControllers()
    {
        string[] controllers = Input.GetJoystickNames();
        //Debug.Log(controllers.Length + " controllers");
        VehicleMovement[] tanks = FindObjectsOfType<VehicleMovement>();
        VehicleMovement otherTank = null;
        if (tanks.Length > 1)
        {
            foreach (VehicleMovement tank in tanks)
            {
                if (tank != this) otherTank = tank;
            }
        }
        for (int i = 0; i < controllers.Length; i++)
        {
            Debug.Log(i + ":" + controllers[i]);
            if (controllers[i].Length > 0)
            {
                if (otherTank.playerNum == i || otherTank == null)
                {
                    playerNum = i+1;
                    return;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
    void Respawn()
    {
        health = maxHealth;
        transform.position = startPos;
        transform.rotation = startRot;
        enemyScore++;
    }
    void TakeDamage(float dmg = 1f)
    {
        health -= dmg;
        if (health > maxHealth) health = maxHealth;
        else if (health <= 0) Respawn();
    }
}
