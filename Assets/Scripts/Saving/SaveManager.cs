using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;
    public SaveSceneState sceneState = new SaveSceneState();
    public List<ClassItems> classItems;
    public int classCount = 2;
    public string adUnitId;
    public string adUnitIdIter;

    private void Start()
    {

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8092520563331474/6922660360";
        adUnitIdIter = "ca-app-pub-8092520563331474/2308627934";
        //adUnitId = "ca-app-pub-8092520563331474/6922660360";
        //adUnitIdIter = "ca-app-pub-8092520563331474/2308627934";
#elif UNITY_IPHONE
        //TODO: add IOS
        adUnitId = "ca-app-pub-8092520563331474/3525452780";
        adUnitIdIter = "ca-app-pub-8092520563331474/3142309405";
#else
            adUnitId = "unexpected_platform";
#endif
        DontDestroyOnLoad(gameObject);
        Instance = this;
        SetStartingData();
        // UnlockAll();
        Load();
        LoadScene();
    }
    void OnApplicationQuit()
    {
        Save();
    }
    public void Save()
    {
        PlayerPrefs.SetString("save", Serializer.Serialize<SaveState>(state));
        PlayerPrefs.Save();
    }

    public void SaveScene()
    {
        PlayerPrefs.SetString("saveScene", Serializer.Serialize<SaveSceneState>(sceneState));
        PlayerPrefs.Save();
    }
    public void SavePlayer(ref PlayerStats playerStats)
    {
        PlayerPrefs.SetString("savePlayer", Serializer.Serialize<PlayerStats>(playerStats));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
            state = Serializer.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        else
        {
            SetStartingData();
        }
    }
    public bool LoadScene()
    {
        if (PlayerPrefs.HasKey("saveScene"))
        {
            sceneState = Serializer.Deserialize<SaveSceneState>(PlayerPrefs.GetString("saveScene"));
            return true;
        }
        else return false;
    }
    public bool LoadPlayer(ref PlayerStats playerStats)
    {
        if (PlayerPrefs.HasKey("saveScene"))
        {
            playerStats = Serializer.Deserialize<PlayerStats>(PlayerPrefs.GetString("savePlayer"));
            return true;
        }
        else
        {
            SetStartingData();
            return false;
        }
    }
    public void SetStartingData()
    {

        // set starting data
        state = new SaveState();
        state.coins = 100;
        state.premiumCoins = 50;
        state.adBlock = false;
        state.fillAllItems = true;
        for (int j = 0; j < classItems.Count; j++)
        {
            state.items.Add(new Dictionary<int, int>());
            state.addedItems.Add(new Dictionary<int, int>());
            for (int i = 0; i < 7; i++)
            {
                state.items[j].Add(i, 2);
                state.addedItems[j].Add(i, 2);
            }
            for (int i = 0; i < 7; i++)
            {
                state.items[j].Add(i + 7, 1);
                state.addedItems[j].Add(i + 7, 1);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            state.maxInfiMapLevel.Add(new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } });
        }
        Save();
    }
    public void UnlockAll()
    {
        state = new SaveState();
        state.coins = 100000;
        state.premiumCoins = 5000;
        state.adBlock = false;
        for (int j = 0; j < classItems.Count; j++)
        {
            state.items.Add(new Dictionary<int, int>());
            state.addedItems.Add(new Dictionary<int, int>());
            for (int i = 0; i < classItems[j].items.Count; i++)
            {
                state.items[j].Add(i, 2);
                state.addedItems[j].Add(i, 2);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            state.maxInfiMapLevel.Add(new Dictionary<int, int>() { { 0, 50 }, { 1, 50 }, { 2, 50 }, { 3, 50 } });
        }
        Save();
    }
    public int UpdateHighScores()
    {
        var rewards = 0;
        if (state.maxInfiMapLevel[sceneState.mapId][sceneState.mapDifficulty] < sceneState.turn)
        {
            if (sceneState.turn > 50)
            {
                int i = 0;
                var helper = new ItemHelper();
                foreach (string difficulty in helper.hardRewards.Keys)
                {
                    if (state.maxInfiMapLevel[sceneState.mapId][sceneState.mapDifficulty] < 50)
                    {
                        rewards += helper.hardRewards[difficulty];
                    }
                    i++;
                    state.maxInfiMapLevel[sceneState.mapId][i] = sceneState.turn;
                    if (i == sceneState.mapDifficulty)
                    {
                        break;
                    }
                }
                var rewardsKeys = helper.hardRewards.Keys;
            }
        }
        state.chests += rewards;
        return rewards;
    }

}
