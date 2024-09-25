using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    private EnemyStats enemyStats;
    private new Rigidbody2D rigidbody;

    public float rotationSpeed = 1;
    public float accelerationSpeed = 1;
    private Quaternion newRotation;
    private Vector3 targetDirection;
    private Vector2 newVelocity;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //rotate towards player
        targetDirection = enemyStats.player.transform.position - transform.position;
        newRotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        //move forward ramping up speed
        rigidbody.velocity =  Vector2.MoveTowards(rigidbody.velocity, enemyStats.baseStats.movementSpeed * new Vector2(transform.up.x, transform.up.y), accelerationSpeed * Time.deltaTime);
    }
}
