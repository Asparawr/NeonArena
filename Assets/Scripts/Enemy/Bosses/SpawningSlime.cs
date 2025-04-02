using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSlime : MonoBehaviour
{
    EnemyStats enemyStats;
    public Vector2 velocity;
    public float spawnPercentage = 0.02f;
    public List<GameObject> enemiesToSpawn;
    float lastHealthPercentage;
    public float minScale = 0.1f;
    float startScale;
    Transform transformParent;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        lastHealthPercentage = 1;
        startScale = transform.localScale.x;
        transformParent = transform.parent;
    }
    private void Update()
    {
        //change scale lerp
        if (minScale < transform.localScale.x)
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(startScale * enemyStats.health / enemyStats.maxHealth, startScale * enemyStats.health / enemyStats.maxHealth, 1), 0.01f);
        enemyStats.speedMod = 1 + 2 * (1 - enemyStats.health / enemyStats.maxHealth);
    }
    public void HandleHealthDecrease()
    {
        // randomly select an enemy to spawn if health percentage decreased by spawnPercentage
        while (lastHealthPercentage - enemyStats.health / enemyStats.maxHealth > spawnPercentage)
        {
            lastHealthPercentage -= spawnPercentage;
            //randomly rotate
            var newEnemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            newEnemy.GetComponent<Rigidbody2D>().velocity = velocity;
            newEnemy.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
            newEnemy.transform.parent = transformParent;
        }
    }
}
