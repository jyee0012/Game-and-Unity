using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu()]
public class ItemScript : ScriptableObject
{
    public string itemName, itemDesc;
    public Sprite itemIcon;
}
