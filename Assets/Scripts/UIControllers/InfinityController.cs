using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfinityController : MonoBehaviour
{
    public MenuController menuController;
    public GameObject playerModel;
    public GameObject mapChangeRight;
    public GameObject mapChangeLeft;
    public Maps mapsSO;
    private List<Map> maps;
    public int currentMap = 0;
    private ItemHelper helper = new ItemHelper();
    public TextMeshProUGUI maxMaxText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI mapName;
    public RawImage mapImage;
    public TMP_Dropdown hardnessDropdown;
    public GameObject LockScreen;
    private void OnEnable()
    {
        currentMap = 0;
        maps = mapsSO.maps;
        UpdateMapName();
        ChangeMap();
        if (currentMap == maps.Count)
            mapChangeRight.SetActive(false);
        if (currentMap == 0)
            mapChangeLeft.SetActive(false);
    }
    public void UpdateMax()
    {
        while (menuController.saveManager.state.maxInfiMapLevel.Count <= maps.Count)
        {
            menuController.saveManager.state.maxInfiMapLevel.Add(new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } });
            menuController.saveManager.Save();
        }
        int i = 0;
        string newMax = "";
        foreach (string name in helper.hardRewards.Keys)
        {
            newMax += "Max " + name + ": " + menuController.saveManager.state.maxInfiMapLevel[currentMap][i].ToString()
                      + " (50 to get " + helper.hardRewards[name].ToString() + " chests)\n";
            i++;
        }
        maxMaxText.text = newMax;
    }
    public void ChangeMapRight()
    {
        mapChangeLeft.SetActive(true);
        currentMap += 1;
        if (currentMap < maps.Count - 1)
            mapChangeRight.SetActive(true);
        else
            mapChangeRight.SetActive(false);
        ChangeMap();
    }
    public void ChangeMapLeft()
    {
        mapChangeRight.SetActive(true);
        currentMap -= 1;
        if (currentMap == 0)
            mapChangeLeft.SetActive(false);
        else
            mapChangeLeft.SetActive(true);
        ChangeMap();
    }
    public void ChangeMap()
    {
        UpdateMax();
        UpdateClass();
        menuController.ChangeBackGround(maps[currentMap].backgroundID);
        mapName.text = maps[currentMap].mapName;
        if (currentMap - 1 >= 0)
        {
            int i = 0;
            int maxMax = 0;
            foreach (string name in helper.hardRewards.Keys)
            {
                if (maxMax < menuController.saveManager.state.maxInfiMapLevel[currentMap - 1][i])
                    maxMax = menuController.saveManager.state.maxInfiMapLevel[currentMap - 1][i];
                i++;
            }
            if (maxMax < 50)
                LockScreen.SetActive(true);
            else
                LockScreen.SetActive(false);
        }
        else
            LockScreen.SetActive(false);
    }
    public void UpdateClass()
    {
        classText.text = menuController.allClasses.playerClasses[menuController.currentClass].className;
    }
    public void UpdateMapName()
    {
        mapName.color = helper.hardMapColors[hardnessDropdown.value];
        mapImage.color = helper.hardMapColors[hardnessDropdown.value];
    }
    public void Play()
    {
        menuController.saveManager.state.isGameStarted = true;
        menuController.saveManager.state.playerClass = menuController.currentClass;
        menuController.gameSetup.difficulty = hardnessDropdown.value;
        menuController.sceneChanger.isLoading = false;
        menuController.gameSetup.map = currentMap;
        menuController.sceneChanger.dimScreenTimer = menuController.sceneChanger.dimScreenMaxTimer;
        menuController.sceneChanger.changeScene = "Game";
    }
}
