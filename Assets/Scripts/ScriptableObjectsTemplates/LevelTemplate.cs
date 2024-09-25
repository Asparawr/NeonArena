using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelTemplate")]
public class LevelTemplate : ScriptableObject
{
    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int number;
    }
    [System.Serializable]
    public class Weave
    {
        public List<Enemy> enemy;
    }
    public List<Weave> weaves;
}
