using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSquareBoss : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyDashing enemyDashing;
    EnemyShooting enemyShooting;
    BossAttacks bossAttacks;
    public float baseRotationSpeed = 1;
    public float rotationSpeed;
    float rotationCount = 0;
    float angle = 0;
    Quaternion qRotation;
    float rotationTimer = 0;
    public GameObject burningArea;
    Rigidbody2D rb;

    void Start()
    {
        //get components
        enemyStats = GetComponent<EnemyStats>();
        enemyDashing = GetComponent<EnemyDashing>();
        enemyShooting = GetComponent<EnemyShooting>();
        bossAttacks = GetComponent<BossAttacks>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * rotationSpeed);
        rotationTimer -= Time.deltaTime;
        if (rotationCount > 0 && rotationTimer <= 0)
        {
            angle += 45;
            qRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rotationTimer = 1 / rotationSpeed;
            enemyShooting.Shoot();
            rotationCount -= 1;
        }
        else if (bossAttacks.startAttack && rb.velocity.magnitude < 0.5f)
        {
            enemyDashing.isEnabled = false;
            bossAttacks.startAttack = false;
            var attackType = Random.Range(0, 4);
            switch (attackType)
            {
                case 0: //rotate 
                    rotationCount = 3;
                    angle += 45;
                    qRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    rotationSpeed = baseRotationSpeed;
                    rotationTimer = 1 / rotationSpeed;
                    break;
                case 1: //shoot 
                    enemyShooting.Shoot();
                    break;
                case 2: //rotate
                    enemyDashing.isEnabled = true;
                    rotationCount = 2;
                    angle += 45;
                    qRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    rotationSpeed = baseRotationSpeed;
                    rotationTimer = 1 / rotationSpeed;
                    break;
                case 3: //rotate 
                    rotationCount = 10;
                    angle += 45;
                    qRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    rotationSpeed = baseRotationSpeed * 2;
                    rotationTimer = 1 / rotationSpeed;
                    break;
            }
        }
        else if (rotationCount == 0)
        {
            enemyDashing.isEnabled = true;
        }
    }
    public void SpawnPool()
    {
        var pool = Instantiate(burningArea, transform.position, Quaternion.identity);
        //set projectile stats on pool
        var poolStats = pool.GetComponent<ProjectileStats>();
        poolStats.damage = enemyStats.GetDamage() / 3;
    }
}
