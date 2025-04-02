using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameShopController : MonoBehaviour
{

    private ItemHelper helper = new ItemHelper();
    public GameObject ShopUI;
    public List<GameObject> UIItems;
    public TextMeshProUGUI moneyCountText;
    private Animator shopAnimator;
    private List<ShopItem> CurrentShopItems = new List<ShopItem>();
    TurnController turnController;
    GameSetup gameSetup;
    public PlayerController playerController;
    public TextMeshProUGUI turnCountText;

    // rarity lists
    public List<ShopItem> commonItems = new List<ShopItem>();
    public List<ShopItem> rareItems = new List<ShopItem>();
    public List<ShopItem> epicItems = new List<ShopItem>();
    public List<ShopItem> legendaryItems = new List<ShopItem>();
    void Start()
    {
        shopAnimator = ShopUI.GetComponent<Animator>();
        turnController = GetComponent<TurnController>();
        gameSetup = GetComponent<GameSetup>();
        playerController = gameSetup.player.GetComponent<PlayerController>();

        // set variables on shop items
        foreach (GameObject UIItem in UIItems)
        {
            UIItem.GetComponent<UIItemController>().SetReferences();
        }
    }
    public void SetupShopItems()
    {
        // set rarity lists
        List<ShopItem> items = gameSetup.allClasses.playerClasses[gameSetup.saveManager.state.playerClass].items.items;
        Dictionary<int, int> addedItems = gameSetup.saveManager.state.addedItems[gameSetup.saveManager.state.playerClass];
        foreach (int itemKey in addedItems.Keys)
        {
            ShopItem item = items[itemKey];
            for (int i = 0; i < addedItems[itemKey]; i++)
            {
                switch (item.rarity)
                {
                    case Rarity.Common:
                        commonItems.Add(item);
                        break;
                    case Rarity.Rare:
                        rareItems.Add(item);
                        break;
                    case Rarity.Epic:
                        epicItems.Add(item);
                        break;
                    case Rarity.Legendary:
                        legendaryItems.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public void OpenShop()
    {
        turnController.isShopOpened = true;
        //update money text
        moneyCountText.text = playerController.playerStats.money.ToString();
        ShopUI.SetActive(true);
        shopAnimator.SetTrigger("Open");
        turnController.pauseController.OpenShopMenu();
        turnCountText.text = "Next Round: " + (turnController.turnCount + 1).ToString();
        LoadItems();
    }

    public void CloseShop()
    {
        turnController.isShopOpened = false;
        StartCoroutine(CloseShopAnimation());
        turnController.pauseController.CloseShopMenu();
        gameSetup.SaveVariables();
        turnController.StartNextTurn();
    }

    IEnumerator CloseShopAnimation()
    {
        shopAnimator.SetTrigger("Close");

        //wait for animation to complete
        yield return new WaitForSeconds(shopAnimator.GetCurrentAnimatorStateInfo(0).length);

        ShopUI.SetActive(false);
    }

    public void LoadItems(int price = 0)
    {
        if (playerController.playerStats.money >= price)
        {
            //clear current shop inventory
            CurrentShopItems.Clear();
            foreach (GameObject UIItem in UIItems)
            {
                //randomize rarity item in shop inventory 
                float rarity = Random.Range(0, 100);
                rarity /= 100;
                //randomize new item in shop inventory
                if (rarity <= playerController.playerStats.legendaryChance && !ContainsAll(CurrentShopItems, legendaryItems))
                {
                    ShopItem newItem = legendaryItems[Random.Range(0, legendaryItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = legendaryItems[Random.Range(0, legendaryItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem);
                }
                else if (rarity <= playerController.playerStats.epicChance + playerController.playerStats.legendaryChance && !ContainsAll(CurrentShopItems, epicItems))
                {
                    ShopItem newItem = epicItems[Random.Range(0, epicItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = epicItems[Random.Range(0, epicItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem);
                }
                else if (rarity <= playerController.playerStats.rareChance + playerController.playerStats.epicChance + playerController.playerStats.legendaryChance && !ContainsAll(CurrentShopItems, rareItems))
                {
                    ShopItem newItem = rareItems[Random.Range(0, rareItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = rareItems[Random.Range(0, rareItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem);
                }
                else
                {
                    ShopItem newItem = commonItems[Random.Range(0, commonItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = commonItems[Random.Range(0, commonItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem);
                }
            }
            playerController.playerStats.money -= price;
            moneyCountText.text = playerController.playerStats.money.ToString();
        }
    }

    // called by buy buttons, item ID based on list IDs
    public void BuyItem(int itemID)
    {
        ShopItem item = CurrentShopItems[itemID];
        if (playerController.playerStats.money >= gameSetup.helper.itemPrices[System.Enum.GetName(typeof(Rarity), item.rarity)])
        {
            //disable rebuying
            UIItems[itemID].GetComponent<UIItemController>().DisableItem();

            //update player
            playerController.playerStats.money -= gameSetup.helper.itemPrices[System.Enum.GetName(typeof(Rarity), item.rarity)];
            moneyCountText.text = playerController.playerStats.money.ToString();
            playerController.AddItem(item);

            //if not stackable remove item from item lists
            if (!item.stackable)
            {
                switch (item.rarity)
                {
                    case Rarity.Common:
                        commonItems.Remove(item);
                        break;
                    case Rarity.Rare:
                        rareItems.Remove(item);
                        break;
                    case Rarity.Epic:
                        epicItems.Remove(item);
                        break;
                    case Rarity.Legendary:
                        legendaryItems.Remove(item);
                        break;
                    default:
                        break;
                }
            }

        }

    }
    public bool ContainsAll(List<ShopItem> a, List<ShopItem> check)
    {
        if (check.Count == 0) return true;
        List<ShopItem> l = new List<ShopItem>(check);
        foreach (ShopItem shopItem in a)
        {
            if (l.Contains(shopItem))
            {
                l.Remove(shopItem);
                if (l.Count == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
