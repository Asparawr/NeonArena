using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class MenuAdController : MonoBehaviour
{
    public MenuController menuController;
    SaveManager saveManager;
    private RewardedAd rewardedAd;
    public GameObject connectionPopup;
    public GameObject rewardPopup;
    public GameObject adBaner;
    bool activateRewardPopup = false;

    private InterstitialAd interstitial;
    public void Setup()
    {
        menuController = GetComponent<MenuController>();
        saveManager = menuController.saveManager;

        if (!saveManager.state.adBlock)
        {
            this.rewardedAd = new RewardedAd(saveManager.adUnitId);
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            //this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
            LoadAd();


            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(saveManager.adUnitIdIter);
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToShow += HandleInterAdFailedToShow;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleInterAdClosed;
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
        }

    }
    private void Update()
    {

        if (activateRewardPopup)
        {
            activateRewardPopup = false;
            rewardPopup.SetActive(true);
        }
    }
    public void LoadAd()
    {
        // List<string> deviceIds = new List<string>();
        // deviceIds.Add("F288AC08234EF6B979D21F3C17A50F1D");
        // RequestConfiguration requestConfiguration = new RequestConfiguration
        //     .Builder()
        //     .SetTestDeviceIds(deviceIds)
        //     .build();
        // MobileAds.SetRequestConfiguration(requestConfiguration);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        connectionPopup.SetActive(true);
        this.LoadAd();
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        saveManager.state.coins += menuController.helper.rewardPerAd;
        saveManager.Save();
        adBaner.SetActive(false);
        activateRewardPopup = true;
    }

    public void ShowRewardAd()
    {

        if (!saveManager.state.adBlock)
        {
            Debug.Log(this.rewardedAd.IsLoaded());
            if (this.rewardedAd.IsLoaded())
                this.rewardedAd.Show();
            else
            {
                connectionPopup.SetActive(true);
                this.LoadAd();
            }
        }
        else
        {
            saveManager.state.coins += menuController.helper.rewardPerAd;
            saveManager.Save();
            rewardPopup.SetActive(true);
            adBaner.SetActive(false);
        }
    }
    public void ShowAdInter()
    {
        if (!saveManager.state.adBlock && this.interstitial.IsLoaded())
            this.interstitial.Show();
    }
    public void HandleInterAdClosed(object sender, System.EventArgs args)
    {
    }
    public void HandleInterAdFailedToShow(object sender, AdErrorEventArgs args)
    {
    }

}
