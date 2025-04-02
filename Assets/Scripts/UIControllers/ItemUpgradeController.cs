using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUpgradeController : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemDeckCountText;
    public TextMeshProUGUI plusText;
    public TextMeshProUGUI minusText;
    public UpgradeMenuController menuController;

    public int itemCount;
    private int itemID;
    private int maxDeckCount = 2;
    public void Add()
    {
        if (itemCount != maxDeckCount)
        {
            itemCount++;
            UpdateAdding();
            menuController.UpdateAddedItemCount(itemID, itemCount);
        }
    }
    public void Remove()
    {
        if (itemCount != 0)
        {
            itemCount--;
            UpdateAdding();
            menuController.UpdateAddedItemCount(itemID, itemCount);
            menuController.addAllToggle.isOn = false;
        }
    }
    public void UpdateAdding()
    {
        if (itemCount == 0) minusText.color = new Color32(0, 0, 0, 150);
        else minusText.color = new Color32(0, 0, 0, 255);
        if (itemCount == maxDeckCount) plusText.color = new Color32(0, 0, 0, 150);
        else plusText.color = new Color32(0, 0, 0, 255);
        itemDeckCountText.text = itemCount.ToString() + "/" + maxDeckCount.ToString();
    }
    public void Setup(ShopItem item, int itemCount, int intemDeckCount, int itemID, UpgradeMenuController menuController)
    {
        this.menuController = menuController;
        this.itemID = itemID;
        this.itemCount = intemDeckCount;
        itemIcon.sprite = item.iconSprite;
        itemDescription.text = item.GetEffects();
        Color color = menuController.helper.colors[Enum.GetName(typeof(Rarity), item.rarity)];
        itemDescription.color = color;
        itemIcon.color = color;
        maxDeckCount = Mathf.Min(2, itemCount);
        UpdateAdding();
    }
}
