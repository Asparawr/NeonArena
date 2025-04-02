using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootDashSelector : MonoBehaviour
{
    public float actionMaxTimer;
    float actionTimer;
    EnemyShooting enemyShooting;
    EnemyDashing enemyDashing;
    void Start()
    {
        enemyShooting = GetComponent<EnemyShooting>();
        enemyDashing = GetComponent<EnemyDashing>();
    }

    void Update()
    {
        actionTimer += Time.deltaTime;
        if (actionTimer > actionMaxTimer)
        {
            actionTimer = 0;
            var action = Random.Range(0, 2);
            if (action == 0)
            {
                enemyDashing.Dash();
            }
            else
            {
                enemyShooting.Shoot();
            }
        }
    }
}
