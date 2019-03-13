using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNumber : MonoBehaviour
{
    GameController myController;

    [SerializeField]
    Text numText = null;

    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.FindObjectOfType<GameController>();
        numText.text = myController.value.ToString();
    }
}
