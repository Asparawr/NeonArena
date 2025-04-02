using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemController : MonoBehaviour
{
    public GameObject itemIcon;
    public GameObject itemDescription;
    public GameObject itemPrice;
    public GameObject inactivePanel;

    private Image itemIconImage;
    private TextMeshProUGUI itemDescriptionText;
    private TextMeshProUGUI itemPriceText;

    private ItemHelper helper = new ItemHelper();
    public void SetReferences()
    {
        itemIconImage = itemIcon.GetComponent<Image>();
        itemDescriptionText = itemDescription.GetComponent<TextMeshProUGUI>();
        itemPriceText = itemPrice.GetComponent<TextMeshProUGUI>();
    }

    public void NewItem(ShopItem item)
    {
        string itemRarity = Enum.GetName(typeof(Rarity), item.rarity);
        Color color = helper.colors[itemRarity];
        inactivePanel.SetActive(false);

        itemIconImage.sprite = item.iconSprite;
        itemIconImage.color = color;

        itemDescriptionText.text = item.GetEffects();
        itemDescriptionText.color = color;
        itemPriceText.text = helper.itemPrices[itemRarity].ToString();
        itemPriceText.color = color;
    }

    public void DisableItem()
    {
        inactivePanel.SetActive(true);
    }
}
