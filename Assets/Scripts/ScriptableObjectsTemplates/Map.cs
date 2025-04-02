using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Map")]
public class Map : ScriptableObject
{
    public int backgroundID;
    public string mapName;
    public string audioName;


    [System.Serializable]
    public class Enemies
    {
        public List<GameObject> enemy;
    }
    public List<Enemies> enemies;
    [System.Serializable]
    public class Bosses
    {
        public List<GameObject> boss;
    }
    public List<Bosses> bosses;
}
