using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashing : MonoBehaviour
{
    public float dashingPeriod = 1; //in seconds
    private float dashingTimer;
    private Vector2 posDifference;

    private EnemyStats enemyStats;
    private new Rigidbody2D rigidbody;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.drag = 1f / dashingPeriod * enemyStats.baseStats.movementSpeed;
    }

    void Update()
    {
        dashingTimer += Time.deltaTime;
        if (dashingTimer > dashingPeriod)
        {
            posDifference = transform.position - enemyStats.player.transform.position;
            if (Mathf.Abs(posDifference.x) > Mathf.Abs(posDifference.y))
            {
                if (posDifference.x < 0)
                    rigidbody.velocity += new Vector2(dashingPeriod * enemyStats.baseStats.movementSpeed, 0);
                else
                    rigidbody.velocity += new Vector2(-dashingPeriod * enemyStats.baseStats.movementSpeed, 0);
            }
            else
            {
                if (posDifference.y < 0)
                    rigidbody.velocity += new Vector2(0, dashingPeriod * enemyStats.baseStats.movementSpeed);
                else
                    rigidbody.velocity += new Vector2(0, -dashingPeriod * enemyStats.baseStats.movementSpeed);
            }
            dashingTimer = 0;
        }
    }
}
