using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour
{
    EnemyStats enemyStats;
    Transform player;
    public float rotationSpeed = 1;
    public float attackMaxTimer = 2;
    float attackTimer;
    public float rotateMaxTimer = 3;
    float rotateTimer;
    public bool isAttacking = false;
    Rigidbody2D rb;
    public UnityEvent Dashed;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        player = enemyStats.player.transform;
        rb.drag = 1f / attackMaxTimer;
    }

    void Update()
    {
        if (rotateTimer > 0)
        {
            rotateTimer -= Time.deltaTime;
            Vector3 vectorToTarget = player.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);

        }
        else if (attackTimer > 0)
        {
            if (!isAttacking)
            {
                Dashed.Invoke();
                isAttacking = true;
                Vector2 direction = (transform.position - player.transform.position);
                rb.AddRelativeForce(Vector2.right * enemyStats.baseStats.movementSpeed * enemyStats.speedMod);
            }
            attackTimer -= Time.deltaTime;
        }
        else
        {
            rotateTimer = rotateMaxTimer;
            attackTimer = attackMaxTimer;
            isAttacking = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        rotateTimer = rotateMaxTimer;
        isAttacking = false;
    }

}
