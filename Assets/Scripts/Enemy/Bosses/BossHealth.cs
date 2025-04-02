using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    GameObject bossHealthObject;
    UIHealthController bossHealth;
    EnemyStats enemyStats;
    void Start()
    {
        bossHealthObject = GameObject.Find("BossHealth");
        bossHealth = bossHealthObject.GetComponent<UIHealthController>();
        bossHealth.ShowBar();
        enemyStats = GetComponent<EnemyStats>();
    }

    public void UpdateHealth()
    {
        bossHealth.UpdateHealth(enemyStats.health / enemyStats.maxHealth);
        if (enemyStats.health <= 0)
        {
            HideHealth();
        }
    }
    public void HideHealth()
    {
        bossHealth.HideBar();
    }
}
