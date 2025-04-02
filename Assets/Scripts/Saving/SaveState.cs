using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Scripting;

[System.Serializable]
[Preserve]
public class SaveState
{
    public int coins = 100;
    public int premiumCoins = 50;
    public bool adBlock = false;
    public int chests = 1;
    public bool isGameStarted = false;
    public int playerClass = 0;
    public bool fillAllItems = true;
    public bool isMuted = false;
    public List<Dictionary<int, int>> items = new List<Dictionary<int, int>>(); //item + item count
    public List<Dictionary<int, int>> addedItems = new List<Dictionary<int, int>>();
    public List<Dictionary<int, int>> maxInfiMapLevel = new List<Dictionary<int, int>>();
}

