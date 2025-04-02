using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProToExploEnemyStats : MonoBehaviour
{
    public ExplodingWithProjectiles explodingWithProjectiles;
    public ProjectileStats projectileStats;
    void Start()
    {
        explodingWithProjectiles.enemyStats = projectileStats.enemyStats;
    }

}
