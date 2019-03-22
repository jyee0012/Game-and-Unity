using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera p1Camera = null, p2Camera = null, p3Camera = null;
    [SerializeField]
    Image canvasDivider = null;
    [SerializeField]
    Canvas canvas = null;
    // Start is called before the first frame update
    void Start()
    {
        ChangeCameras();
    }

    // Update is called once per frame
    void Update()
    {

    }
    bool CheckMultiDisplay()
    {
        return (Display.displays.Length > 1);
    }
    void ChangeCameras()
    {
        bool multiDisplay = CheckMultiDisplay();
        Debug.Log(multiDisplay);
        if (p1Camera != null)
        {
            p1Camera.rect = multiDisplay ? new Rect(0, 0.5f, 1, 0.5f) : new Rect(0, 0.5f, 0.5f, 0.5f);
            p1Camera.targetDisplay = multiDisplay ? 1 : 0;
        }
        if (p2Camera != null)
        {
            p2Camera.rect = multiDisplay ? new Rect(0, 0, 1, 0.5f) : new Rect(0, 0, 0.5f, 0.5f);
            p2Camera.targetDisplay = multiDisplay ? 1 : 0;
        }
        if (p3Camera != null)
        {
            p3Camera.rect = multiDisplay ? new Rect(0, 0, 1, 1) : new Rect(0.5f, 0, 0.5f, 1);
        }
        if (canvasDivider != null)
        {
            canvasDivider.rectTransform.sizeDelta = multiDisplay ? new Vector2(800, 5) : new Vector2(400, 5);
            canvasDivider.rectTransform.localPosition = multiDisplay ? Vector3.zero : new Vector3(-200, 0);
        }
        if (canvas != null)
        {
            canvas.targetDisplay = multiDisplay ? 1 : 0;
        }
        if (multiDisplay) Display.displays[1].Activate();
    }
}
