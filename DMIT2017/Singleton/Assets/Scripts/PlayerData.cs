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

[Serializable]
public class PlayerInfo
{
    public string name;
    public float score, currentHp;
    [SerializeField, Range(100, 1000)]
    public float maxHp;
    public Vector3 position;
    public List<Item> inventory;
    public PlayerInfo()
    {
        name = "";
        score = 0;
        maxHp = 100;
        currentHp = maxHp;
        inventory = new List<Item>();
    }
}

[CreateAssetMenu()]
public class PlayerData : ScriptableObject
{
    public PlayerInfo playerInfo = new PlayerInfo();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
