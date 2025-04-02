using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    private EnemyStats enemyStats;
    Transform player;
    private Rigidbody2D rb;

    public float rotationSpeed = 1;
    public float accelerationSpeed = 1;
    private Quaternion newRotation;
    private Vector3 targetDirection;
    private Vector2 newVelocity;
    public float stopRange = 0;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = enemyStats.player.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //rotate towards player
        targetDirection = enemyStats.player.transform.position - transform.position;
        newRotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        //move forward ramping up speed
        if (Vector3.Distance(transform.position, player.position) > stopRange)
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(transform.up.x, transform.up.y), accelerationSpeed * Time.deltaTime);
        }
        else
        {
            //reduce velocity towards 0
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, accelerationSpeed * Time.deltaTime);
        }
    }
}
