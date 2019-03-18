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
    public Item(ItemScript newItemDetail, int amount)
    {
        itemDetail = newItemDetail;
        itemAmount = amount;
    }
}

[Serializable]
[CreateAssetMenu()]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public float score, currentHp;
    [SerializeField, Range(100, 1000)]
    public float maxHp;
    public Vector3 position;
    public List<Item> inventory;
    public bool inDungeon;

    public void AddInventoryItem(ItemScript newItem, int amount = 1)
    {
        bool addedItem = false;
        foreach (Item item in inventory)
        {
            if (item.itemDetail == newItem)
            {
                addedItem = true;
                item.itemAmount += amount;
            }
        }
        if (!addedItem)
        {
            inventory.Add(new Item(newItem, amount));
        }
    }
    public void EnterDungeon(Vector3 playerPos)
    {
        inDungeon = true;
        position = playerPos;
    }
    public void ExitDungeon()
    {
        inDungeon = false;
        position = Vector3.zero;
    }
    public void TakeDamage(float dmg = 1)
    {
        currentHp -= dmg;
        if (currentHp >= maxHp) currentHp = maxHp;
        else if (currentHp <= 0) currentHp = 0;
    }
    public void GainScore(float points = 100)
    {
        score += points;
    }
    public void FullHeal()
    {
        currentHp = maxHp;
    }
    public void ResetScore()
    {
        score = 0;
    }
    public void ClearInventory()
    {
        inventory.Clear();
    }
}
