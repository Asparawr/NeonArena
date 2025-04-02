using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemHelper
{
    public Dictionary<string, float> itemChances = new Dictionary<string, float>(){
            {"Common", 1},
            {"Rare", 0.2f},
            {"Epic", 0.05f},
            {"Legendary", 0.02f}
        };
    public Dictionary<string, int> itemDusting = new Dictionary<string, int>(){
            {"Common", 5},
            {"Rare", 20},
            {"Epic", 50},
            {"Legendary", 150}
        };

    public Dictionary<string, int> itemPrices = new Dictionary<string, int>(){
            {"Common", 200},
            {"Rare", 400},
            {"Epic", 800},
            {"Legendary", 1600}
        };
    public Dictionary<string, Color> colors = new Dictionary<string, Color>(){
            {"Common", new Color(0.9f, 0.8f, 0.8f, 1)},
            {"Rare", new Color(0.46f, 0.66f, 0.26f, 0.8f)},
            {"Epic", new Color(0.76f, 0.24f, 0.68f, 0.8f)},
            {"Legendary", new Color(0.81f, 0.4f, 0.13f, 0.8f)}
        };
    public Dictionary<string, int> hardRewards = new Dictionary<string, int>(){
            {"Easy", 3},
            {"Normal", 8},
            {"Hard", 15},
            {"Nightmare", 30}
        };
    public Dictionary<int, Color> hardMapColors = new Dictionary<int, Color>(){
            {0, new Color(1f, 1f, 1f, 1)},
            {1, new Color(0.7f, 0.8f, 0.45f, 1f)},
            {2, new Color(0.3f, 0.1f, 0.8f, 1f)},
            {3, new Color(1f, 0f, 0f, 1f)}
        };
    public Dictionary<int, float> hardMapLevels = new Dictionary<int, float>(){
            {0, .5f},
            {1, 1f},
            {2, 1.5f},
            {3, 2f}
        };
    public float moneyGain = 600;
    public float bossMoneyGain = 1200;
    public int rewardPerTen = 200;
    public int rewardPerOne = 5;
    public int rewardPerAd = 100;
}
