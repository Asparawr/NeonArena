using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCycleShield : MonoBehaviour
{
    public float shieldActiveTimerMax = 2;
    float shieldActiveTimer = 0;
    public float sizeChangeSpeed = 0.1f;
    public float minScale = 0.1f;
    public Transform shield;
    bool shieldActive = false;
    bool shieldGrowing = false;
    EnemyStats enemyStats;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void FixedUpdate()
    {
        if (shieldActive)
        {
            shieldActiveTimer -= Time.deltaTime;
            if (shieldActiveTimer <= 0)
            {
                shieldActive = false;
                enemyStats.damageImmunity = false;
            }
        }
        else if (shieldGrowing)
        {
            shield.localScale = Vector3.MoveTowards(shield.localScale, Vector3.one, sizeChangeSpeed * Time.deltaTime);
            if (shield.localScale.x >= 1)
            {
                shieldGrowing = false;
                shieldActive = true;
                shieldActiveTimer = shieldActiveTimerMax;
                enemyStats.damageImmunity = true;
            }
        }
        else
        {
            shield.localScale = Vector3.MoveTowards(shield.localScale, Vector3.zero, sizeChangeSpeed * Time.deltaTime);
            if (shield.localScale.x <= minScale)
                shieldGrowing = true;
        }
    }
}
