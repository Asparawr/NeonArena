using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public MenuController menuController;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI premiumCoinText;
    public GameObject adblockItem;
    public void UpdateCoins()
    {
        coinText.text = menuController.saveManager.state.coins.ToString();
        premiumCoinText.text = menuController.saveManager.state.premiumCoins.ToString();
    }
    public void BoughtAdBlock()
    {
        menuController.saveManager.state.adBlock = true;
        menuController.saveManager.Save();
        adblockItem.SetActive(false);
    }
    public void BoughtCoins(float amount)
    {
        menuController.saveManager.state.premiumCoins += (int)amount;
        menuController.saveManager.Save();
        UpdateCoins();
    }
    public void ConnectionError()
    {
        menuController.menuAdController.connectionPopup.SetActive(true);
    }
}
