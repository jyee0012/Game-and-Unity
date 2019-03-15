using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIScript : MonoBehaviour
{
    [SerializeField]
    Text itemNameText = null, itemQuantityText = null;
    [SerializeField]
    Image itemImg = null;
    [SerializeField]
    Sprite defaultSprite = null;

    [Space]
    public ItemScript currentItem = null;
    public int itemAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem != null)
        {
            SetItemInfo();
        }
        else
        {
            HideItemInfo();
        }
    }
    void SetItemInfo()
    {
        if (itemNameText != null) itemNameText.text = currentItem.itemName;
        if (itemNameText != null) itemQuantityText.text = itemAmount.ToString();
        if (itemImg != null) itemImg.sprite = currentItem.itemIcon;
    }
    void HideItemInfo()
    {
        if (itemNameText != null) itemNameText.text = "";
        if (itemNameText != null) itemQuantityText.text = "";
        if (itemImg != null && defaultSprite != null) itemImg.sprite = defaultSprite;
    }
}
