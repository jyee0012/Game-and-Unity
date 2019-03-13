using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ItemDetail
{
    public string itemName, itemDesc;
    public Sprite itemIcon;
    public ItemDetail()
    {
        itemName = "";
        itemDesc = "";
        itemIcon = null;
    }
}

[CreateAssetMenu()]
public class ItemScript : ScriptableObject
{
    public ItemDetail item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
