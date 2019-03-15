using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : MonoBehaviour
{
    [SerializeField]
    List<ItemScript> lootList = new List<ItemScript>();

    [SerializeField, Range(0,20)]
    int lootAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (lootAmount <= 0) lootAmount = Random.Range(1, 6);
            if (lootList.Count > 0)
            {
                other.GetComponent<PlayerController>().playerData.AddInventoryItem(RandomItem(), lootAmount);
            }
            Destroy(gameObject);
        }
    }
    public ItemScript RandomItem()
    {
        int randomIndex = Random.Range(0, lootList.Count);
        return lootList[randomIndex];
    }
}
