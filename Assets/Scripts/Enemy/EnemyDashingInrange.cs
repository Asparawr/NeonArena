using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDashingInRange : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyFollowing enemyFollowing;
    Rigidbody2D rb;
    Transform player;
    public float dashSpeedMod = 1;
    public float range = 1;
    public float rotationSpeed = 100;
    public float accelerationSpeed = 5;

    public float rotateTimerMax;
    float rotateTimer;
    public float dashTimerMax;
    float dashTimer;
    bool isRotating = false;
    public bool isDashing = false;

    public UnityEvent Dashed;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyFollowing = GetComponent<EnemyFollowing>();
        rb = GetComponent<Rigidbody2D>();
        player = enemyStats.player.transform;
    }

    void Update()
    {
        //start rotating in in range, dash after rotating
        if (!isRotating && !isDashing && range > Vector3.Distance(transform.position, player.position))
        {
            isRotating = true;
            rotateTimer = rotateTimerMax;
            enemyFollowing.enabled = false;
        }
        else if (isRotating)
        {
            //decrease velocity
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, accelerationSpeed * Time.deltaTime);
            rotateTimer -= Time.deltaTime;
            //rotate towards player 
            var targetDirection = enemyStats.player.transform.position - transform.position;
            var newRotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            if (rotateTimer <= 0)
            {
                // 50% chance to invoke event
                if (Random.Range(0, 2) == 0)
                {
                    Dashed.Invoke();
                }
                isRotating = false;
                isDashing = true;
                dashTimer = dashTimerMax;
            }
        }
        else if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            rb.velocity = Vector2.MoveTowards(rb.velocity, dashSpeedMod * enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(transform.up.x, transform.up.y), accelerationSpeed * Time.deltaTime);

            if (dashTimer <= 0)
            {
                isDashing = false;
                enemyFollowing.enabled = true;
            }
        }
    }
}
