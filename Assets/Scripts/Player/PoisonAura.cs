using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAura : MonoBehaviour
{
    public float poisonDamage = 4;
    public float poisonDuration = 5;
    public float poisonTickRate = 1f;
    public EnemyStats enemyStats;
    private void Start()
    {
        if (enemyStats != null)
        {
            poisonDamage *= enemyStats.difficultyMod;
        }
    }
}
