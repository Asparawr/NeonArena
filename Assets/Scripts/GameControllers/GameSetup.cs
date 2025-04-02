using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class GameSetup : MonoBehaviour
{

    public ItemHelper helper = new ItemHelper();
    public GameObject player;
    public GameObject playerModel;
    public Map map;
    int mapId;
    public int mapDifficulty;
    public Maps mapsSO;
    public SaveManager saveManager;
    private SceneChanger sceneChanger;
    public AllClasses allClasses;
    public PlayerController playerController;
    TurnController turnController;
    GameShopController shopController;
    public GameObject rewardNotification;
    public GameObject AbandonNotification;

    private RewardedAd rewardedAd;
    public GameObject connectionPopup;

    private InterstitialAd interstitial;
    public GameObject muteButton;
    public bool isLeaving = false;
    public void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        turnController = GetComponent<TurnController>();
        shopController = GetComponent<GameShopController>();
    }
    public void AbandonConfirmation()
    {
        if (playerController.playerStats.respawnCount > 0 || playerController.playerStats.adRespawned == false)
        {
            AbandonNotification.SetActive(true);
        }
        else
        {
            Abandon();
        }
    }
    public void Abandon()
    {
        turnController.turnCount -= 1;
        SaveVariables();
        var rewards = saveManager.UpdateHighScores();
        if (rewards > 0 || turnController.turnCount > 0)
        {
            rewardNotification.SetActive(true);
            var coinReward = turnController.turnCount / 10 * helper.rewardPerTen + turnController.turnCount * helper.rewardPerOne;
            rewardNotification.GetComponent<RewardNotification>().SetReward(rewards, coinReward);
            saveManager.state.coins += coinReward;
            saveManager.Save();
        }
        else
            LeaveGame();
    }
    public void LeaveGame()
    {
        saveManager.state.isGameStarted = false;
        saveManager.Save();
        isLeaving = true;
        ShowAdInter();
    }

    public void MainMenu()
    {
        sceneChanger.dimScreenTimer = sceneChanger.dimScreenMaxTimer;
        sceneChanger.changeScene = "MainMenu";
    }
    void SetupPlayer()
    {
        var model = Instantiate(allClasses.playerClasses[saveManager.state.playerClass].playerModel, playerModel.transform);
        //playerController.playerModel = model;
        playerController.playerProjectile = allClasses.playerClasses[saveManager.state.playerClass].playerProjectile;
        foreach (Transform child in model.transform)
            if (child.tag == "BulletSpawner")
                playerController.bulletSpawner = child.gameObject;
        playerController.UpdatePlayerScale();
        if (allClasses.playerClasses[saveManager.state.playerClass].meeleClass)
        {
            playerController.playerStats.warriorAttack = true;
            playerController.playerStats.range *= 0.3f;
            playerController.playerStats.bulletPenetration = 100000;
        }
    }
    public void SetupVariables(GameSetupVariables gameSetup, SaveManager saveManager, SceneChanger sceneChanger)
    {
        this.sceneChanger = sceneChanger;
        mapId = gameSetup.map;
        mapDifficulty = gameSetup.mapDifficulty;
        map = mapsSO.maps[mapId];
        turnController.difficulty = helper.hardMapLevels[(int)gameSetup.difficulty];
        turnController.difficultyIncrease *= helper.hardMapLevels[(int)gameSetup.difficulty];
        this.saveManager = saveManager;
        turnController.enemyValue = turnController.enemyValueStarting;
        SetupPlayer();
        SaveVariables();
        shopController.SetupShopItems();
        turnController.StartNextTurn();

        //play map music
        var audioManager = FindObjectOfType<AudioManager>();
        audioManager.StopAll();
        audioManager.Play(map.audioName);

        if (!saveManager.state.adBlock)
            SetupAds();
    }
    public void LoadVariables(SaveManager saveManager, SceneChanger sceneChanger)
    {
        this.saveManager = saveManager;
        this.sceneChanger = sceneChanger;
        mapId = saveManager.sceneState.mapId;
        map = mapsSO.maps[mapId];
        turnController.difficulty = saveManager.sceneState.difficulty;
        turnController.enemyValue = saveManager.sceneState.enemyValue;
        turnController.turnCount = saveManager.sceneState.turn;
        saveManager.LoadPlayer(ref playerController.playerStats);
        SetupPlayer();
        shopController.SetupShopItems();
        turnController.StartNextTurn();
    }
    public void SaveVariables()
    {
        saveManager.sceneState.mapId = mapId;
        saveManager.sceneState.mapDifficulty = mapDifficulty;
        saveManager.sceneState.difficulty = turnController.difficulty;
        saveManager.sceneState.enemyValue = turnController.enemyValue;
        saveManager.sceneState.turn = turnController.turnCount;
        saveManager.SavePlayer(ref playerController.playerStats);
        saveManager.SaveScene();
    }
    public void SetupAds()
    {
        //respawn ad
        //ads
        this.rewardedAd = new RewardedAd(saveManager.adUnitId);
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        LoadAd();

        //game over ad
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(saveManager.adUnitIdIter);
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToShow += HandleRewardedAdFailedToShowInter;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleRewardedAdClosedInter;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        if (saveManager.state.isMuted)
            muteButton.SetActive(false);
    }

    public void HandleRewardedAdFailedToShowInter(object sender, AdErrorEventArgs args)
    {
        if (isLeaving)
            MainMenu();
    }
    public void HandleRewardedAdClosedInter(object sender, System.EventArgs args)
    {
        if (isLeaving)
            MainMenu();
    }

    public void ShowAdInter()
    {
        if (!saveManager.state.adBlock && this.interstitial.IsLoaded())
            this.interstitial.Show();
        else
            MainMenu();
    }
    public void LoadAd()
    {
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
        playerController.playerStats.adRespawned = true;
    }
    public void HandleRewardedAdClosed(object sender, System.EventArgs args)
    {
        turnController.RespawnLaunch();
    }
    public void ShowAd()
    {
        if (!saveManager.state.adBlock)
        {
            if (this.rewardedAd.IsLoaded())
                this.rewardedAd.Show();
            else
                connectionPopup.SetActive(true);
        }
        else
        {
            playerController.playerStats.adRespawned = true;
            turnController.RespawnLaunch();
        }
    }
}
