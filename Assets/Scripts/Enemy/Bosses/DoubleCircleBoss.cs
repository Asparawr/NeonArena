using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCircleBoss : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyFollowing enemyFollowing;
    EnemyShooting enemyShooting;
    EnemySpawner enemySpawner;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyFollowing = GetComponent<EnemyFollowing>();
        enemyShooting = GetComponent<EnemyShooting>();
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void Enrage()
    {
        enemyStats.difficultyMod *= 1.5f;
        if (enemyFollowing != null)
            enemyFollowing.accelerationSpeed *= 1.5f;
        enemyShooting.shootingPeriod *= 0.5f;
        enemySpawner.spawnerPoints[0].interval *= 0.5f;
        //grow size over time 
        StartCoroutine(Grow());
    }
    IEnumerator Grow()
    {
        float size = transform.localScale.x;
        while (transform.localScale.x < size * 1.5f)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
