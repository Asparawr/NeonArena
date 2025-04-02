using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBoss : MonoBehaviour
{
    EnemyStats enemyStats;
    BossAttacks bossAttacks;
    public EnemyShooting baseAxisShooting;
    public EnemyShooting XAxisShooting;
    public Transform rotateAll;
    public int attackMode = -1;
    float cooldownRotationSpeed = 1;

    //attack timers
    public float attackCooldown = 2;
    public float attackModeTimer = 0;
    float attackTimer = 0;
    public float firstAttackDelay = 1;
    bool toggleAttack = false;
    public float secondAttackDelay = 1;
    public float secondAttackRotationSpeed = 1;
    public float thirdAttackDelay = 1;
    public float thirdAttackRotationSpeed = 1;
    public float fourthAttackDelay = 1;
    public float fourthAttackRotationSpeed = 1;
    public float fifthAttackDelay = 1;
    public float fifthAttackRotationSpeed = 1;
    public float sixthAttackDelay = 1;
    public float sixthAttackRotationSpeed = 1;


    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        bossAttacks = GetComponent<BossAttacks>();
    }

    void Update()
    {
        if (bossAttacks.startAttack)
        {
            if (enemyStats.health < enemyStats.maxHealth / 2)
                attackMode = Random.Range(0, 6);

            else
                attackMode = Random.Range(0, 5);
            attackModeTimer = bossAttacks.delayTimer - attackCooldown;
            bossAttacks.startAttack = false;
        }
        attackModeTimer -= Time.deltaTime;
        if (attackModeTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            switch (attackMode)
            {
                case 0:
                    // shoot from every other axis with delay
                    if (attackTimer < 0)
                    {
                        if (toggleAttack)
                            baseAxisShooting.Shoot();
                        else
                            XAxisShooting.Shoot();
                        toggleAttack = !toggleAttack;
                        attackTimer = firstAttackDelay;
                    }
                    break;
                case 1:
                    // rotate and shoot from every other axis with delay
                    if (attackTimer < 0)
                    {
                        if (toggleAttack)
                            baseAxisShooting.Shoot();
                        else
                            XAxisShooting.Shoot();
                        toggleAttack = !toggleAttack;
                        attackTimer = secondAttackDelay;
                    }
                    Quaternion qRotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 45, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * secondAttackRotationSpeed);
                    break;
                case 2:
                    // rotate and shoot from base axis with delay
                    if (attackTimer < 0)
                    {
                        baseAxisShooting.Shoot();
                        attackTimer = thirdAttackDelay;
                    }
                    Quaternion qRotation2 = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 45, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qRotation2, Time.deltaTime * thirdAttackRotationSpeed);
                    break;
                case 3:
                    // rotate and shoot from both axises with delay
                    if (attackTimer < 0)
                    {
                        baseAxisShooting.Shoot();
                        XAxisShooting.Shoot();
                        attackTimer = fourthAttackDelay;
                    }
                    Quaternion qRotation3 = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 45, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qRotation3, Time.deltaTime * fourthAttackRotationSpeed);
                    break;
                case 4:
                    // rotate and shoot wall from base axis with delay
                    if (attackTimer < 0)
                    {
                        baseAxisShooting.Shoot();
                        attackTimer = fifthAttackDelay;
                    }
                    Quaternion qRotation4 = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 45, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qRotation4, Time.deltaTime * fifthAttackRotationSpeed);
                    break;
                case 5:
                    // rotate and shoot random axis with delay
                    if (attackTimer < 0)
                    {
                        if (Random.Range(0, 2) == 0)
                            baseAxisShooting.ShootRandom();
                        else
                            XAxisShooting.ShootRandom();

                        attackTimer = sixthAttackDelay;
                    }
                    Quaternion qRotation5 = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + 45, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, qRotation5, Time.deltaTime * sixthAttackRotationSpeed);
                    break;
                default:
                    break;

            }
        }
        else
        {
            Quaternion qRotation = Quaternion.AngleAxis(((int)transform.rotation.eulerAngles.z / 90 + 1) * 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * cooldownRotationSpeed);
        }
    }
}