using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TurnController : MonoBehaviour
{
    public GameObject player;
    public GameObject ShopUI;
    public GameObject moneyCount;

    public List<GameObject> UIItems;
    public List<ShopItem> CurrentShopItems;
    public List<ShopItem> ItemPool;

    private Animator shopAnimator;

    private PlayerStats playerStats;
    private TextMeshProUGUI moneyCountText;
    private PauseController pauseController;

    // rarity lists
    public List<ShopItem> commonItems;
    public List<ShopItem> rareItems;
    public List<ShopItem> epicItems;
    public List<ShopItem> legendaryItems;

    // rarity chances and colors
    public float rareChance = 0;
    public float epicChance = 0;
    public float legendaryChance = 0;

    public Color commonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        moneyCountText = moneyCount.GetComponent<TextMeshProUGUI>();
        pauseController = GetComponent<PauseController>();
        shopAnimator = ShopUI.GetComponent<Animator>();

        // set variables on shop items
        foreach (GameObject UIItem in UIItems)
        {
            UIItem.GetComponent<UIItemController>().SetReferences();
        }

        // set rarity lists
        foreach (ShopItem item in ItemPool)
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


    public void OpenShop()
    {
        //update money text
        moneyCountText.text = playerStats.money.ToString();
        ShopUI.SetActive(true);
        shopAnimator.SetTrigger("Open");
        pauseController.unPausable = true;
        pauseController.Pause();
        LoadItems();
    }

    public void CloseShop()
    {
        StartCoroutine(CloseShopAnimation());
        pauseController.unPausable = false;
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
        if (playerStats.money >= price)
        {
            //clear current shop inventory
            CurrentShopItems.Clear();
            foreach (GameObject UIItem in UIItems)
            {
                //randomize rarity item in shop inventory 0
                float rarity = Random.Range(0, 100);
                //randomize new item in shop inventory
                if (rarity < legendaryChance && !ContainsAll(CurrentShopItems, legendaryItems))
                {
                    ShopItem newItem = legendaryItems[Random.Range(0, legendaryItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = legendaryItems[Random.Range(0, legendaryItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem, legendaryColor);
                }
                else if (rarity < epicChance + legendaryChance && !ContainsAll(CurrentShopItems, epicItems))
                {
                    ShopItem newItem = epicItems[Random.Range(0, epicItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = epicItems[Random.Range(0, epicItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem, epicColor);
                }
                else if (rarity < rareChance + epicChance + legendaryChance && !ContainsAll(CurrentShopItems, rareItems))
                {
                    ShopItem newItem = rareItems[Random.Range(0, rareItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = rareItems[Random.Range(0, rareItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem, rareColor);
                }
                else
                {
                    ShopItem newItem = commonItems[Random.Range(0, commonItems.Count - 1)];
                    while (CurrentShopItems.Contains(newItem))
                    {
                        newItem = commonItems[Random.Range(0, commonItems.Count)];
                    }

                    CurrentShopItems.Add(newItem);
                    UIItem.GetComponent<UIItemController>().NewItem(newItem, commonColor);
                }
            }
            playerStats.money -= price;
            moneyCountText.text = playerStats.money.ToString();
        }
    }

    // called by buy buttons, item ID based on list IDs
    public void BuyItem(int itemID)
    {
        ShopItem item = CurrentShopItems[itemID];
        if (playerStats.money >= item.price)
        {
            //disable rebuying
            UIItems[itemID].GetComponent<UIItemController>().DisableItem();

            // update intem rarity chances
            rareChance += item.rareChance;
            epicChance += item.epicChance;
            legendaryChance += item.legendaryChance;

            //update player
            playerStats.money -= item.price;
            moneyCountText.text = playerStats.money.ToString();
            playerStats.AddItem(item);

            //if not stackable remove item from item lists
            if (!item.stackable) {
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
