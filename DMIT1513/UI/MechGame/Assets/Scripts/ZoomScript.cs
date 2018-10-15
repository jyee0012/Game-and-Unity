using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomScript : MonoBehaviour {

    [SerializeField]
    GameObject image;
    [SerializeField]
    Slider zoomSlider;
    [SerializeField]
    Text zoomText;
    [SerializeField]
    InputField inputZoom;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        float zoomMod = zoomSlider.value / 100;
        zoomText.text = "Zoom: " + zoomSlider.value + "%";
        image.transform.localScale = Vector3.one * zoomMod;
	}
}
