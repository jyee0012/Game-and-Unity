using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public ItemScript itemDetail;
    public int itemAmount;
    public Item()
    {
        itemDetail = null;
        itemAmount = 0;
    }
}

[CreateAssetMenu()]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public float score, currentHp;
    [SerializeField, Range(100, 1000)]
    public float maxHp;
    public Vector3 position;
    public List<Item> inventory;
}
