using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendHealthChange : MonoBehaviour
{
    public EnemyStats enemyStats;
    public EnemyStats mainEnemyStats;
    public float lastEnemyHealth;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
            lastEnemyHealth = enemyStats.health;
    }
    public void SendHealth()
    {
        mainEnemyStats.UpdateHealth(-(lastEnemyHealth - enemyStats.health));
        lastEnemyHealth = enemyStats.health;
    }
}
