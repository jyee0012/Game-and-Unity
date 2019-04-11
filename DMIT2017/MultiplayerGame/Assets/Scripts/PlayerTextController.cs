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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
    }
    void ControllerInput()
    {
        if (playerText == null) return;
        playerText.text = "Player " + playerNum;
        // a button press
        if (Input.GetAxis("Button0P" + playerNum) > 0)
        {
            playerText.color = Color.green;
        }
        // b button press
        if (Input.GetAxis("Button1P" + playerNum) > 0)
        {
            playerText.color = Color.red;
        }
    }
}
