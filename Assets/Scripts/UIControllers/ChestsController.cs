using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestsController : MonoBehaviour
{
    public MenuController menuController;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI premiumCoinText;
    public TextMeshProUGUI chestCountText;
    public TextMeshProUGUI dropsText;
    public TextMeshProUGUI popupChestAmount;
    public TextMeshProUGUI popupPrice;
    public int currentPrice;
    public bool currentCoinPremium;
    public GameObject buyPopup;
    public GameObject notEnoughPopup;
    public GameObject coinIcon;
    public GameObject premiumIcon;
    public bool openNext = false;

    public GameObject itemCrafting;
    public Transform itemOpenGrid;
    public Transform itemCraftingGrid;
    public Toggle animationToggle;
    public GameObject openOkButton;
    List<List<int>> rarityItems = new List<List<int>>
    {   new List<int>(){},
        new List<int>(){},
        new List<int>(){},
        new List<int>(){}
    };

    private void Awake()
    {
        UpdateCoins();
        UpdateDrops();
        List<ShopItem> allItems = menuController.allClasses.playerClasses[menuController.currentClass].items.items;
        int i = 0;
        foreach (var item in allItems)
        {
            rarityItems[(int)item.rarity].Add(i);
            i += 1;
        }
    }
    public void UpdateCoins()
    {
        coinText.text = menuController.saveManager.state.coins.ToString();
        premiumCoinText.text = menuController.saveManager.state.premiumCoins.ToString();
        chestCountText.text = menuController.saveManager.state.chests.ToString();
    }
    public void UpdateDrops()
    {
        int[] counters = new int[] { 0, 0, 0, 0 };
        Dictionary<int, int> ownedItems = menuController.saveManager.state.items[menuController.currentClass];
        List<ShopItem> allItems = menuController.allClasses.playerClasses[menuController.currentClass].items.items;
        foreach (var item in allItems)
        {
            counters[(int)item.rarity] += 2;
        }
        foreach (var item in ownedItems.Keys)
        {
            counters[(int)allItems[item].rarity] -= ownedItems[item];
        }
        dropsText.text = "Drops left:\n" + counters[0].ToString() + " commons\n" + counters[1].ToString() + " rares\n" + counters[2].ToString() + " epics\n" + counters[3].ToString() + " legendaries\n";
    }
    public void ShowDrops()
    {
        //clean item grid
        foreach (Transform child in itemCraftingGrid)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (int state in new List<int>() { 0, 1, 2, 3 })
        {
            int i = 0;
            foreach (var item in menuController.allClasses.playerClasses[menuController.currentClass].items.items)
            {
                if (((int)item.rarity) == state)
                {
                    bool ifCointains = menuController.saveManager.state.items[menuController.currentClass].ContainsKey(i);
                    if (!ifCointains)
                    {
                        GameObject newItem = Instantiate(itemCrafting, itemCraftingGrid);
                        newItem.GetComponent<ItemCraftingController>().Setup(item, 2, i, this);
                    }
                    if (ifCointains && menuController.saveManager.state.items[menuController.currentClass][i] != 2)
                    {
                        Debug.Log(2 - menuController.saveManager.state.items[menuController.currentClass][i]);
                        GameObject newItem = Instantiate(itemCrafting, itemCraftingGrid);
                        newItem.GetComponent<ItemCraftingController>().Setup(item, 2 - menuController.saveManager.state.items[menuController.currentClass][i], i, this);
                    }
                }
                i++;
            }
        }
    }
    public void Open()
    {
        if (menuController.saveManager.state.chests > 0)
        {
            openOkButton.SetActive(true);
            menuController.saveManager.state.chests -= 1;
            var itemChances = menuController.helper.itemChances;
            List<ShopItem> allItems = menuController.allClasses.playerClasses[menuController.currentClass].items.items;
            foreach (Transform itemPanel in itemOpenGrid)
            {
                float rarity = Random.Range(0, 100) / 100f;
                int itemId = 0;
                int dustCount = 0;
                string outRarity;
                //set itemId based on rarity
                if (rarity <= itemChances["Legendary"])
                {
                    itemId = rarityItems[3][Random.Range(0, rarityItems[3].Count - 1)];
                    outRarity = "Legendary";
                }
                else if (rarity <= itemChances["Epic"] + itemChances["Legendary"])
                {
                    itemId = rarityItems[2][Random.Range(0, rarityItems[2].Count - 1)];
                    outRarity = "Epic";
                }
                else if (rarity <= itemChances["Rare"] + itemChances["Epic"] + itemChances["Legendary"])
                {
                    itemId = rarityItems[1][Random.Range(0, rarityItems[1].Count - 1)];
                    outRarity = "Rare";
                }
                else
                {
                    itemId = rarityItems[0][Random.Range(0, rarityItems[0].Count)];
                    outRarity = "Common";
                }
                //set variables and ui
                if (menuController.saveManager.state.items[menuController.currentClass].ContainsKey(itemId) && menuController.saveManager.state.items[menuController.currentClass][itemId] == 2)
                {
                    dustCount = menuController.helper.itemDusting[outRarity];
                    menuController.saveManager.state.coins += dustCount;
                }
                else if (menuController.saveManager.state.items[menuController.currentClass].ContainsKey(itemId))
                {
                    menuController.saveManager.state.items[menuController.currentClass][itemId] += 1;
                }
                else
                {
                    menuController.saveManager.state.items[menuController.currentClass][itemId] = 1;
                }
                itemPanel.GetComponent<ItemOpenController>().Open(allItems[itemId], this, animationToggle.isOn, dustCount);
            }
            UpdateCoins();
            menuController.saveManager.Save();
        }
    }
    public void ResetOpening()
    {
        foreach (Transform itemPanel in itemOpenGrid)
        {
            itemPanel.GetComponent<ItemOpenController>().Reset();
        }
    }
    public void BuyPopup(int amount)
    {
        if (menuController.saveManager.state.coins < amount * 100)
        {
            notEnoughPopup.SetActive(true);
            return;
        }
        buyPopup.SetActive(true);
        popupChestAmount.text = "Purchase " + amount.ToString() + " chests?";
        amount *= 100;
        popupPrice.text = amount.ToString();
        currentPrice = amount;
        coinIcon.SetActive(true);
        premiumIcon.SetActive(false);
        currentCoinPremium = false;
    }
    public void BuyPremiumPopup(int amount)
    {
        if (menuController.saveManager.state.premiumCoins < amount * 20)
        {
            notEnoughPopup.SetActive(true);
            return;
        }
        buyPopup.SetActive(true);
        popupChestAmount.text = "Purchase " + amount.ToString() + " chests?";
        amount *= 20;
        popupPrice.text = amount.ToString();
        currentPrice = amount;
        coinIcon.SetActive(false);
        premiumIcon.SetActive(true);
        currentCoinPremium = true;
    }
    public void Buy()
    {
        if (currentCoinPremium)
        {
            menuController.saveManager.state.premiumCoins -= currentPrice;
            menuController.saveManager.state.chests += currentPrice / 20;
        }
        else
        {
            menuController.saveManager.state.coins -= currentPrice;
            menuController.saveManager.state.chests += currentPrice / 100;
        }
        menuController.saveManager.Save();
        UpdateCoins();
    }
}
