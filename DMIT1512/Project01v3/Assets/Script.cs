using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script : MonoBehaviour
{
    public float fuel, speed = 2f; // speed.....ye
    float move, // the combination of Time.deltaTime * speed
        maxFuel = 1000,
        timeStamp; // A time stamp that will hold a Time.time
    GameObject sprite; // my sprite game object
    bool once = true, // a boolean that should be used only once at a time
        controls = true,
        fuelmeupbaby = false;
    public Slider fuelGauge;
    // Use this for initialization
    void Start()
    {
        // initialize values and variables.
        sprite = GameObject.FindGameObjectWithTag("Sprite");

        fuelGauge.maxValue = maxFuel;
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        #region Once Reset
        if (!once)
        {
            timeStamp = Time.time + 2f;
        }
        if (timeStamp <= Time.time)
        {
            once = true;
        }
        #endregion
        fuelGauge.value = fuel;
        if (fuel >= 0)
        {
            move = Time.deltaTime * speed;
            PlayerInput();
            //Debug.Log(Input.GetAxis("Horizontal") + ":" + Input.GetAxis("Vertical"));
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * move);
        }
        else
        {
            fuelGauge.fillRect.gameObject.SetActive(false);
        }
        #region Getting Gas
        if (fuelmeupbaby)
        {
            GainGas();
        }
        #endregion
    }
    #region Basic Movement
    void BasicMovement(string input)
    {
        if (input == "right")
        {
            transform.Translate(new Vector3(1, 0, 0) * move);
            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        if (input == "left")
        {
            transform.Translate(new Vector3(-1, 0, 0) * move);
            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        if (input == "up")
        {
            transform.Translate(new Vector3(0, 1, 0) * move);
            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        if (input == "down")
        {
            transform.Translate(new Vector3(0, -1, 0) * move);
            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        fuel -= 0.25f * speed;
    }
    #endregion
    #region Player Input
    void PlayerInput()
    {
        #region Speed Increase
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 5f;
            if (once)
            {
                Debug.Log("Gotta Go Fast!");
                once = false;
            }
        }
        else
        {
            speed = 2f;
        }
        #endregion
        #region Key Input
        if (controls)
        {
            if (Input.GetKey(KeyCode.W))
            {
                BasicMovement("up");
            }
            if (Input.GetKey(KeyCode.S))
            {
                BasicMovement("down");
            }
            if (Input.GetKey(KeyCode.D))
            {
                BasicMovement("right");
            }
            if (Input.GetKey(KeyCode.A))
            {
                BasicMovement("left");
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.J))
            {
                transform.Rotate(new Vector3(0, 0, 1));
            }
            else if (Input.GetKey(KeyCode.L))
            {
                transform.Rotate(new Vector3(0, 0, -1));
            }
            if (Input.GetKey(KeyCode.I))
            {
                transform.Translate(new Vector3(0, 1, 0) * move);
            }
            if (Input.GetKey(KeyCode.K))
            {
                transform.Translate(new Vector3(0, -1, 0) * move);
            }
        }
        #endregion

        #region Diagonal Rotation
        if (controls)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -225));
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 225));
            }
        }

        #endregion
    }
    #endregion
    void GainGas()
    {
        fuel += 2f;
    }
    void GainGas(float num)
    {
        fuel += num;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gas Station")
        {
            fuelmeupbaby = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gas Station")
        {
            fuelmeupbaby = false;
        }
    }
}
