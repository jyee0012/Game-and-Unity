using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public bool hit = false;
    public Material hitMAT;
    // Use this for initialization
    void Start()
    {
        if (hitMAT == null)
        {
            hitMAT = Resources.Load("RedMAT") as Material;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<MeshRenderer>().material = hitMAT;
        hit = true;
    }
}
