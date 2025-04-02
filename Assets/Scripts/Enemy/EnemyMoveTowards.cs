using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTowards : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Rigidbody2D rb;
    public float stopRange = 0;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //gradually change velocity towards player
        if (Vector2.Distance(transform.position, enemyStats.player.transform.position) > stopRange)
            rb.velocity = Vector2.MoveTowards(rb.velocity, (enemyStats.speedMod * enemyStats.baseStats.movementSpeed * (enemyStats.player.transform.position - transform.position).normalized), enemyStats.speedMod * enemyStats.baseStats.movementSpeed * Time.deltaTime);

        //transform.position = Vector2.MoveTowards(transform.position, enemyStats.player.transform.position, enemyStats.speedMod * enemyStats.baseStats.movementSpeed * Time.deltaTime);
    }
}
