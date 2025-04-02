using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuController : MonoBehaviour
{
    public MenuController menuController;
    public Toggle addAllToggle;
    SaveManager saveManager;
    public Transform itemGrid;
    public ItemHelper helper;
    private int addedSum;
    public TMPro.TextMeshProUGUI limitText;
    public GameObject PopupCommon;
    public GameObject PopupAmount;
    private Dictionary<int, int> rarityStates = new Dictionary<int, int>(){
            {0, 1}, //Common
            {1, 1}, //Rare
            {2, 1}, //Epic
            {3, 1} //Legendary
        };
    public void Setup()
    {
        saveManager = menuController.saveManager;
        helper = menuController.helper;
        //fill all toggle
        addAllToggle.isOn = menuController.saveManager.state.fillAllItems;
        menuController.loadedClass = -1;
        LoadClassItems();
        menuController.SwapClass();
    }

    public void LoadClassItems(bool forceLoad = false)
    {
        if (!forceLoad && menuController.loadedClass == menuController.currentClass)
            return;
        menuController.loadedClass = menuController.currentClass;
        //load current class items
        List<ShopItem> items = menuController.allClasses.playerClasses[menuController.currentClass].items.items;

        foreach (Transform child in itemGrid)
        {
        }
        //list of children on grid
        var itemGridItems = itemGrid.GetComponentsInChildren<ItemUpgradeController>(true);
        int childId = 0;
        foreach (int rarityState in rarityStates.Keys)
        {
            if (rarityStates[rarityState] == 1)
            {
                foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
                {
                    if (saveManager.state.items[menuController.currentClass][item] > 0)
                    {
                        if (saveManager.state.addedItems[menuController.currentClass].ContainsKey(item))
                        {
                            if (((int)items[item].rarity) == rarityState)
                            {
                                itemGrid.GetChild(childId).gameObject.SetActive(true);
                                itemGridItems[childId].Setup(items[item], saveManager.state.items[menuController.currentClass][item], saveManager.state.addedItems[menuController.currentClass][item], item, this);
                                childId += 1;
                            }
                        }
                        else
                        {
                            if (((int)items[item].rarity) == rarityState)
                            {
                                itemGrid.GetChild(childId).gameObject.SetActive(true);
                                itemGridItems[childId].Setup(items[item], saveManager.state.items[menuController.currentClass][item], 0, item, this);
                                childId += 1;
                            }
                        }
                    }
                }
            }
        }
        //disable unused grid
        for (int i = childId; i < itemGrid.childCount; i++)
        {
            itemGrid.GetChild(i).gameObject.SetActive(false);
        }
        //calculate added items sum
        addedSum = 0;
        foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
        {
            if (saveManager.state.addedItems[menuController.currentClass].ContainsKey(item))
                addedSum += saveManager.state.addedItems[menuController.currentClass][item];
        }
        limitText.text = "Required\n" + addedSum.ToString() + "/20";
    }

    public void UpdateAddedItemCount(int item, int count, bool save = true)
    {
        if (saveManager.state.addedItems[menuController.currentClass].ContainsKey(item))
            addedSum += count - saveManager.state.addedItems[menuController.currentClass][item];
        else
            addedSum += count;
        limitText.text = "Required\n" + addedSum.ToString() + "/20";
        saveManager.state.addedItems[menuController.currentClass][item] = count;
        if (save)
            saveManager.Save();
    }
    public void UpdateItemCount(int item, int count)
    {
        if (saveManager.state.items[menuController.currentClass].ContainsKey(item))
            saveManager.state.items[menuController.currentClass][item] += count;
        else
            saveManager.state.items[menuController.currentClass][item] = count;
        saveManager.Save();
    }
    public void ToggleRarity(int rarityID)
    {
        rarityStates[rarityID] *= -1;
        LoadClassItems(true);
    }
    public bool CheckDeck()
    {
        if (CommonCount() < 8)
        {
            PopupCommon.SetActive(true);
            menuController.PopupBackground.SetActive(true);
            return false;
        }
        else if (addedSum < 20)
        {
            PopupAmount.SetActive(true);
            menuController.PopupBackground.SetActive(true);
            return false;
        }
        return true;
    }
    public int CommonCount()
    {
        int commonCount = 0;

        List<ShopItem> items = menuController.allClasses.playerClasses[menuController.currentClass].items.items;
        foreach (var item in saveManager.state.addedItems[menuController.currentClass].Keys)
        {
            if (items[item].rarity == 0) commonCount += saveManager.state.addedItems[menuController.currentClass][item];
        }

        return commonCount;
    }
    public void AutoFill()
    {
        int commonCount = CommonCount();
        List<ShopItem> items = menuController.allClasses.playerClasses[menuController.currentClass].items.items;
        if (commonCount < 8)
        {
            foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
            {
                if (items[item].rarity == 0 && saveManager.state.items[menuController.currentClass][item] > 0 && saveManager.state.addedItems[menuController.currentClass][item] == 0)
                {
                    commonCount += 1;
                    UpdateAddedItemCount(item, saveManager.state.addedItems[menuController.currentClass][item] + 1);
                }
                if (commonCount == 8) break;
                if (items[item].rarity == 0 && saveManager.state.items[menuController.currentClass][item] == 2 && saveManager.state.addedItems[menuController.currentClass][item] == 1)
                {
                    commonCount += 1;
                    UpdateAddedItemCount(item, saveManager.state.addedItems[menuController.currentClass][item] + 1);
                }
                if (commonCount == 8) break;
            }
        }

        if (addedSum < 20)
        {
            foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
            {
                if (saveManager.state.addedItems[menuController.currentClass][item] == 0 && saveManager.state.items[menuController.currentClass][item] != 0)
                    UpdateAddedItemCount(item, saveManager.state.addedItems[menuController.currentClass][item] + 1);
                if (addedSum == 20) break;
                if (saveManager.state.addedItems[menuController.currentClass][item] == 1 && saveManager.state.items[menuController.currentClass][item] > 1)
                    UpdateAddedItemCount(item, saveManager.state.addedItems[menuController.currentClass][item] + 1);
                if (addedSum == 20) break;
            }
        }
        saveManager.Save();
        LoadClassItems(true);
    }
    public void ToggleFillAll()
    {
        saveManager.state.fillAllItems = addAllToggle.isOn;
        saveManager.Save();
        if (saveManager.state.fillAllItems)
            FillAll();
    }
    public void FillAll()
    {
        foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
        {
            UpdateAddedItemCount(item, saveManager.state.items[menuController.currentClass][item], false);
        }
        LoadClassItems(true);
        saveManager.Save();
    }
    public void RemoveAll()
    {
        addAllToggle.isOn = false;
        ToggleFillAll();
        var keys = saveManager.state.addedItems[menuController.currentClass].Keys;
        foreach (var item in saveManager.state.items[menuController.currentClass].Keys)
        {
            UpdateAddedItemCount(item, 0);
        }
        saveManager.Save();
        LoadClassItems(true);
    }
}
