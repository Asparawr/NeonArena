using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowingAxis : MonoBehaviour
{
    private EnemyStats enemyStats;
    public float maxAxisTimer = 5;
    private float axisTimer;
    bool xAxis;
    float weaveMod = 1;
    public float weaveMaxTimer = 1;
    public float weaveMaxMinTimer = 1;
    float weaveTimer;
    Rigidbody2D rb;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //move towards player setting velocity on one axis, switch axis on timer
        if (axisTimer <= 0)
        {
            xAxis = !xAxis;
            //random from 0 to max
            axisTimer = Random.Range(1, maxAxisTimer);
        }
        else
        {
            axisTimer -= Time.deltaTime;
        }
        //wave timer 
        if (weaveTimer <= 0)
        {
            weaveMod = -weaveMod;
            //random from weaveMaxMinTimer to max
            weaveTimer = Random.Range(weaveMaxMinTimer, weaveMaxTimer);

        }
        else
        {
            weaveTimer -= Time.deltaTime;
        }
        if (xAxis)
        {
            //gradually move towards player on x Axis
            //rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(enemyStats.speedMod * enemyStats.baseStats.movementSpeed * (enemyStats.player.transform.position.x - transform.position.x), weaveMod), enemyStats.baseStats.movementSpeed * enemyStats.speedMod * Time.deltaTime);
            rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(enemyStats.speedMod * enemyStats.baseStats.movementSpeed * (enemyStats.player.transform.position.x - transform.position.x), 0), enemyStats.baseStats.movementSpeed * enemyStats.speedMod * Time.deltaTime);
        }
        else
        {
            //move towards player on y Axis
            //rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(weaveMod, enemyStats.speedMod * enemyStats.baseStats.movementSpeed * (enemyStats.player.transform.position.y - transform.position.y)), enemyStats.baseStats.movementSpeed * enemyStats.speedMod * Time.deltaTime);
            rb.velocity = Vector2.MoveTowards(rb.velocity, new Vector2(0, enemyStats.speedMod * enemyStats.baseStats.movementSpeed * (enemyStats.player.transform.position.y - transform.position.y)), enemyStats.baseStats.movementSpeed * enemyStats.speedMod * Time.deltaTime);
        }
    }
}
