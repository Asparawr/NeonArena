using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemController : MonoBehaviour
{
    public GameObject itemIcon;
    public GameObject itemDescription;
    public GameObject itemPrice;
    public GameObject inactivePanel;

    private SVGImage itemIconImage;
    private TextMeshProUGUI itemDescriptionText;
    private TextMeshProUGUI itemPriceText;

    public void SetReferences()
    {
        itemIconImage = itemIcon.GetComponent<SVGImage>();
        itemDescriptionText = itemDescription.GetComponent<TextMeshProUGUI>();
        itemPriceText = itemPrice.GetComponent<TextMeshProUGUI>();
    }

    public void NewItem(ShopItem item, Color color)
    {
        inactivePanel.SetActive(false);

        itemIconImage.sprite = item.iconSprite;
        itemIconImage.color = color;

        itemDescriptionText.text = item.GetEffects();
        itemDescriptionText.color = color;

        itemPriceText.text = item.price.ToString();
        itemPriceText.color = color;
    }

    public void DisableItem()
    {
        inactivePanel.SetActive(true);
    }
}
