using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraftingController : MonoBehaviour
{
    public int coin;
    public int premiumCoin;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI premiumCoinText;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemCounter;
    private int itemCount;
    public Image itemIcon;
    public ChestsController chestController;
    private int itemID;
    public void BuyCoin()
    {
        if (chestController.menuController.saveManager.state.coins < coin)
        {
            chestController.notEnoughPopup.SetActive(true);
            return;
        }
        chestController.menuController.upgradeMenuController.UpdateItemCount(itemID, 1);
        chestController.menuController.saveManager.state.coins -= coin;
        chestController.UpdateCoins();
        chestController.menuController.saveManager.Save();
        if (itemCount != 1)
        {
            itemCount--;
            itemCounter.text = itemCount.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void BuyPremiumCoin()
    {
        if (chestController.menuController.saveManager.state.premiumCoins < premiumCoin)
        {
            chestController.notEnoughPopup.SetActive(true);
            return;
        }
        chestController.menuController.upgradeMenuController.UpdateItemCount(itemID, 1);
        chestController.menuController.saveManager.state.premiumCoins -= premiumCoin;
        chestController.UpdateCoins();
        chestController.menuController.saveManager.Save();
        if (itemCount != 1)
        {
            itemCount--;
            itemCounter.text = itemCount.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Setup(ShopItem item, int itemCount, int itemID, ChestsController chestController)
    {
        this.chestController = chestController;
        this.itemID = itemID;
        this.itemCount = itemCount;
        itemCounter.text = itemCount.ToString();
        itemIcon.sprite = item.iconSprite;
        itemDescription.text = item.GetEffects();
        Color color = chestController.menuController.helper.colors[Enum.GetName(typeof(Rarity), item.rarity)];
        coin = chestController.menuController.helper.itemPrices[Enum.GetName(typeof(Rarity), item.rarity)];
        premiumCoin = coin / 5;
        coinText.text = coin.ToString();
        premiumCoinText.text = premiumCoin.ToString();
        itemDescription.color = color;
        itemIcon.color = color;
    }
}
