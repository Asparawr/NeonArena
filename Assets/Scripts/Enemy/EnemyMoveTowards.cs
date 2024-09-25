using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTowards : MonoBehaviour
{
    private EnemyStats enemyStats;
    private new Rigidbody2D rigidbody;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyStats.player.transform.position, enemyStats.baseStats.movementSpeed * Time.deltaTime);
    }
}
