using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    public Transform child;
    public Transform spawnPoint;
    EnemyStats enemyStats;
    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();

    }
    public void Spawn()
    {
        foreach (GameObject enemy in enemies)
        {
            SpawnOne(enemy);
        }
    }
    public void SpawnRandom()
    {
        SpawnOne(enemies[Random.Range(0, enemies.Count)]);
    }
    void SpawnOne(GameObject enemy)
    {
        var pos = transform.position;
        var rot = Quaternion.identity;
        if (spawnPoint != null)
        {
            pos = spawnPoint.position;
            rot = spawnPoint.rotation;
        }
        //add random, small vector to pos
        pos += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        GameObject newEnemy = Instantiate(enemy, pos, Quaternion.identity);
        //newEnemy.gameObject.layer = LayerMask.NameToLayer("SpawningEnemy");
        if (child != null)
            newEnemy.transform.parent = child.parent;
        else
            newEnemy.transform.parent = transform.parent;
        newEnemy.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
    }
}
