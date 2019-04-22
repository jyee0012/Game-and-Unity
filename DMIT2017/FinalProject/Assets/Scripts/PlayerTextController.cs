using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextController : MonoBehaviour
{
    [SerializeField]
    Text playerText = null;
    [SerializeField]
    int playerNum = 0;
    public bool activePlayer = false;
    Color startColor;
    [SerializeField]
    Color activeColor = Color.green;
    // Start is called before the first frame update
    void Start()
    {
        if (playerText == null) playerText = GetComponentInChildren<Text>();
        if (playerText != null) startColor = playerText.color;
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
        if (playerText != null) playerText.color = (activePlayer) ? activeColor : startColor;
    }
    void ControllerInput()
    {
        playerText.text = "Player " + playerNum;
        // a button press
        if (Input.GetAxis("Button0P" + playerNum) > 0)
        {
	    activePlayer = true;
        }
        // b button press
        if (Input.GetAxis("Button1P" + playerNum) > 0)
        {
	    activePlayer = false;
        }
    }
}
