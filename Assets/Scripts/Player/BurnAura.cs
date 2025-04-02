using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAura : MonoBehaviour
{
    public float burnDamage = 0;
    public float burnDuration = 3;
    public float burnTickRate = 0.5f;
    public EnemyStats enemyStats;
    private void Start()
    {
        if (enemyStats != null)
        {
            burnDamage *= enemyStats.difficultyMod;
        }
    }
}
