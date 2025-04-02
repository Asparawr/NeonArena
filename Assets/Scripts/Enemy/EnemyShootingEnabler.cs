using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingEnabler : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyShooting enemyShooting;
    Transform player;
    public float range = 5;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = enemyStats.player.transform;
        enemyShooting = GetComponent<EnemyShooting>();
    }

    void Update()
    {
        //enable shooting if in range
        if (Vector3.Distance(transform.position, player.position) < range)
        {
            enemyShooting.isEnabled = true;
        }
        else
        {
            enemyShooting.isEnabled = false;
        }
    }
}
