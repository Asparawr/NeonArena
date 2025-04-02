using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaShootingBoss : MonoBehaviour
{
    public List<EnemyShooting> enemyShootingList;
    public float disableShootingTimerMax;
    float disableShootingTimer;
    EnemyStats enemyStats;
    bool enraged = false;
    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }
    private void Update()
    {
        if (!enraged && enemyStats.health <= enemyStats.maxHealth / 2)
        {
            Enrage();
            enraged = true;
        }

        if (disableShootingTimer > 0)
        {
            disableShootingTimer -= Time.deltaTime;
            if (disableShootingTimer <= 0)
            {
                foreach (var shooting in enemyShootingList)
                {
                    shooting.enabled = true;
                }
            }
        }
    }
    public void Enrage()
    {
        GetComponent<EnemyStats>().speedMod *= 1.5f;
        foreach (var shooting in enemyShootingList)
        {
            shooting.shootingPeriod /= 2;
        }
        GetComponent<LaserSpewer>().delay /= 2;
    }
    public void DisableShooting()
    {
        foreach (var shooting in enemyShootingList)
        {
            shooting.enabled = false;
        }
        disableShootingTimer = disableShootingTimerMax;
    }
}
