using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnController : MonoBehaviour
{
    public GameObject player;
    public GameObject ShopUI;

    public GameObject moneyCount;

    private PlayerStats playerStats;
    private TextMeshProUGUI moneyCountText;


    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        moneyCountText = moneyCount.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
     //   OpenShop();
    }

    public void OpenShop()
    {
        //update money text
        moneyCountText.text = playerStats.money.ToString();
        ShopUI.SetActive(true);
    }

    public void CloseShop()
    {
        ShopUI.SetActive(false);
    }
}
