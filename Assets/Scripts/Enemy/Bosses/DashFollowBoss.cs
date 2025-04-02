using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashFollowBoss : MonoBehaviour
{
    public GameObject burningArea;
    public GameObject add;
    public float addSpawnPercentage = 0.05f;
    public float lastHealthPercentage = 1;
    EnemyStats enemyStats;
    public float burningScale = 0.5f;
    public bool addToBossHealth = true;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (addToBossHealth)
            transform.parent.GetComponent<BossHealthList>().enemyStatsList.Add(enemyStats);

    }
    public void SpawnPool()
    {
        var pool = Instantiate(burningArea, transform.position, Quaternion.identity);
        pool.transform.parent = transform.parent;
        pool.transform.localScale = new Vector3(burningScale, burningScale, 1);
        //set projectile stats on pool
        var poolStats = pool.GetComponent<ProjectileStats>();
        poolStats.damage = enemyStats.GetDamage() / 3;
    }
    public void CheckHealth()
    {
        if (enemyStats.health <= enemyStats.maxHealth / 2)
        {
            transform.parent.GetComponent<BossHealthList>().updateDelay = .1f;
            GetComponent<SpawnEnemies>().Spawn();
            enemyStats.DestroySelf();
            return;
        }
        while (lastHealthPercentage - enemyStats.health / enemyStats.maxHealth >= addSpawnPercentage)
        {
            lastHealthPercentage -= addSpawnPercentage;
            var newAdd = Instantiate(add, transform.position, Quaternion.identity);
            newAdd.transform.parent = transform.parent.parent;
            newAdd.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
            //randomly rotate 
            newAdd.transform.Rotate(0, 0, Random.Range(0, 360));
            //randomly add velocity
            var rb = newAdd.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 5;
        }
    }
}
