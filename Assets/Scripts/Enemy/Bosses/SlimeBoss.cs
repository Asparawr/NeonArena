using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
    BossAttacks bossAttacks;
    float dashingTimer = 0;
    float originalDashingPeriod;
    EnemyDashing enemyDashing;
    float angle = 0;
    public float rotationSpeed = 1;
    public float dashingSpeedMod;
    Quaternion qRotation;
    void Start()
    {
        bossAttacks = GetComponent<BossAttacks>();
        enemyDashing = GetComponent<EnemyDashing>();
        originalDashingPeriod = enemyDashing.dashingPeriod;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * rotationSpeed);
        if (dashingTimer > 0)
        {
            dashingTimer -= Time.deltaTime;
            if (dashingTimer <= 0)
            {
                enemyDashing.dashingPeriod = originalDashingPeriod;
                enemyDashing.dashingSpeedMod = 1f;
            }
        }
        else if (bossAttacks.startAttack)
        {
            bossAttacks.startAttack = false;
            var attackType = Random.Range(0, 2);
            switch (attackType)
            {
                case 0: //rotate 
                    angle += 45;
                    qRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
                case 1: //change dashing period 
                    enemyDashing.dashingPeriod = 1f;
                    enemyDashing.dashingSpeedMod = dashingSpeedMod;
                    dashingTimer = 3f;
                    break;
            }
        }
    }
}
