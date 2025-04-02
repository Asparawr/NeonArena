using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class MenuController : MonoBehaviour
{
    public List<GameObject> menuScenes;
    // 0 - main
    // 1 - game mode
    // 2 - upgrades
    // 3 - story mode
    // 4 - infinity mode
    // 5 - shop
    // 6 - chests
    // 7 - options
    public Transform backgrounds;
    public Button continueButton;
    public GameObject backButton;
    public GameObject playButton;
    public GameObject upgradeButton;
    public GameObject shopButton;
    public GameObject eventSystem;
    public Stack<int> sceneHistory = new Stack<int>();
    int activeScene;

    public float sideChangeSpeed;
    public LeanTweenType sideChangeAnimationType;
    public SaveManager saveManager;
    public TextMeshProUGUI classText;
    public AllClasses allClasses;
    public int currentClass = 0;
    public ItemHelper helper = new ItemHelper();
    public GameObject PopupBackground;
    public GameObject PopupAbandon;
    public GameObject PopupUpgradeAbandon;
    public GameObject PopupQuitGame;
    public GameSetupVariables gameSetup;
    public SceneChanger sceneChanger;
    public InfinityController infinityController;
    public int loadedClass = 0;
    public MenuAdController menuAdController;
    public UpgradeMenuController upgradeMenuController;
    public GameObject muteButton;

    void Start()
    {

        Time.timeScale = 1;

        //link components
        backgrounds = GameObject.FindGameObjectsWithTag("BackGrounds")[0].transform;
        eventSystem = GameObject.FindGameObjectsWithTag("EventSystem")[0];
        saveManager = GameObject.FindGameObjectsWithTag("SaveManager")[0].GetComponent<SaveManager>();
        gameSetup = eventSystem.GetComponent<GameSetupVariables>();
        sceneChanger = eventSystem.GetComponent<SceneChanger>();
        menuAdController = GetComponent<MenuAdController>();

        //visual setup
        ChangeBackGround(0);
        sceneHistory.Push(0);
        if (saveManager.state.isGameStarted)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
        if (saveManager.state.isMuted)
            muteButton.SetActive(false);
        //play main music
        var audioManager = FindObjectOfType<AudioManager>();
        audioManager.StopAll();
        audioManager.Play("Main");

        //setup controllers
        upgradeMenuController.Setup();
        menuAdController.Setup();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeScene == 0)
            {
                PopupQuitGame.SetActive(true);
                PopupBackground.SetActive(true);
            }
            else
                SwapLastScene();
        }
    }
    public void TryQuit()
    {
        PopupQuitGame.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ResetMenu()
    {
        menuScenes[0].SetActive(true);
        sceneHistory = new Stack<int>();
        sceneHistory.Push(0);
    }

    public void Continue()
    {
        ChangeBackGround(infinityController.mapsSO.maps[saveManager.sceneState.mapId].backgroundID);
        gameSetup.difficulty = saveManager.sceneState.difficulty;
        gameSetup.map = saveManager.sceneState.mapId;
        sceneChanger.dimScreenTimer = sceneChanger.dimScreenMaxTimer;
        sceneChanger.isLoading = true;
        sceneChanger.dimScreenTimer = sceneChanger.dimScreenMaxTimer;
        sceneChanger.changeScene = "Game";
    }
    public void TryNewGame()
    {
        if (saveManager.state.isGameStarted)
        {
            PopupAbandon.SetActive(true);
            PopupBackground.SetActive(true);
        }
        else
            NewGame();
    }
    public void TryUpgrade()
    {
        if (saveManager.state.isGameStarted)
        {
            PopupUpgradeAbandon.SetActive(true);
            PopupBackground.SetActive(true);
        }
        else
        {
            SwapUpgradeScene();
        }
    }
    public void NewGame()
    {
        Abandon();
        SwapLeftScene(4);
        playButton.SetActive(false);
        backButton.SetActive(true);
    }
    public void Abandon()
    {
        if (saveManager.state.isGameStarted)
        {
            saveManager.state.isGameStarted = false;
            saveManager.Save();
            continueButton.interactable = false;
            // menuAdController.ShowAdInter();
        }
    }
    public void SwapRightScene(int sceneID)
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        sceneHistory.Push(activeScene);
        SwapScene(sceneID, true);
        StartCoroutine(SwapSceneCleanup(sceneID, true));
    }
    public void SwapLeftScene(int sceneID)
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        sceneHistory.Push(activeScene);
        SwapScene(sceneID, false);
        StartCoroutine(SwapSceneCleanup(sceneID, true));
    }

    void SwapScene(int sceneID, bool swapRight)
    {
        // disable user input
        eventSystem.SetActive(false);
        // activate new scene
        menuScenes[sceneID].SetActive(true);
        // reposition new scene
        if (swapRight)
            menuScenes[sceneID].GetComponent<RectTransform>().anchoredPosition = new Vector3(1200, 0, 0);
        else
            menuScenes[sceneID].GetComponent<RectTransform>().anchoredPosition = new Vector3(-1200, 0, 0);

        // set animation
        LeanTween.moveX(menuScenes[sceneID], 0, sideChangeSpeed).setEase(sideChangeAnimationType);
        // reposition old scene
        if (swapRight)
            LeanTween.moveX(menuScenes[activeScene].GetComponent<RectTransform>(), -1200, sideChangeSpeed).setEase(sideChangeAnimationType);
        else
            LeanTween.moveX(menuScenes[activeScene].GetComponent<RectTransform>(), 1200, sideChangeSpeed).setEase(sideChangeAnimationType);
    }

    public void SwapLastScene()
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        if (sceneHistory.Peek() == 0)
        {
            playButton.SetActive(true);
            backButton.SetActive(false);
            ChangeBackGround(0);
        }

        if (activeScene == 2)
        {
            upgradeButton.SetActive(true);
            shopButton.SetActive(false);
        }
        if (activeScene == 5 || activeScene == 6)
        {
            upgradeButton.SetActive(false);
            shopButton.SetActive(true);
            upgradeMenuController.LoadClassItems();
        }
        SwapScene(sceneHistory.Peek(), false);

        StartCoroutine(SwapSceneCleanup(sceneHistory.Peek(), false));
    }

    public void SwapUpgradeScene()
    {

        playButton.SetActive(false);
        backButton.SetActive(true);
        SwapScene(2, true);
        if (activeScene != 5 && activeScene != 6)
        {
            sceneHistory.Push(activeScene);
            StartCoroutine(SwapSceneCleanup(2, true));
        }
        else
            StartCoroutine(SwapSceneCleanup(2, false));
        upgradeButton.SetActive(false);
        shopButton.SetActive(true);
        if (saveManager.state.fillAllItems)
            upgradeMenuController.FillAll();
        else
            upgradeMenuController.LoadClassItems();

    }

    public void SwapShopScene()
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        sceneHistory.Push(activeScene);
        SwapScene(5, true);
        StartCoroutine(SwapSceneCleanup(5, true));
        upgradeButton.SetActive(true);
        shopButton.SetActive(false);
    }

    IEnumerator SwapSceneCleanup(int sceneID, bool isPushing)
    {
        //wait for animation to complete
        yield return new WaitForSeconds(sideChangeSpeed);

        // do cleanup
        if (isPushing)
            menuScenes[sceneHistory.Peek()].SetActive(false);
        else
        {
            menuScenes[activeScene].SetActive(false);
            sceneHistory.Pop();
        }
        activeScene = sceneID;
        // enable user input
        eventSystem.SetActive(true);
    }
    public void SwapClassRight()
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        currentClass += 1;
        if (currentClass >= allClasses.playerClasses.Count)
            currentClass = 0;
        SwapClass();
    }
    public void SwapClassLeft()
    {
        if (activeScene == 2 && !upgradeMenuController.CheckDeck()) return;
        currentClass -= 1;
        if (currentClass < 0)
            currentClass = allClasses.playerClasses.Count - 1;
        SwapClass();
    }
    public void SwapClass()
    {
        classText.text = allClasses.playerClasses[currentClass].className;
        if (activeScene == 2)
        {
            if (saveManager.state.fillAllItems)
                upgradeMenuController.FillAll();
            else
                upgradeMenuController.LoadClassItems();
        }
    }

    public void ChangeBackGround(int to)
    {
        //disable any active backgrounds
        for (int i = 0; i < backgrounds.childCount; i++)
            backgrounds.GetChild(i).gameObject.SetActive(false);

        //set background
        backgrounds.GetChild(to).gameObject.SetActive(true);

        // set particles
        foreach (Transform child in backgrounds.GetChild(to))
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                particleSystem.Simulate(particleSystem.main.duration);
                particleSystem.Play();
            }
        }
    }
}
