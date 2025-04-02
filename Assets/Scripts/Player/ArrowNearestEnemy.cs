using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowNearestEnemy : MonoBehaviour
{
    public SpriteRenderer arrow;
    GameObject nearestEnemy;
    public float alphaMod = 1;
    public float maxUpdateTimer = 0.5f;
    float updateTimer = 0;
    void FixedUpdate()
    {
        if (updateTimer > 0)
        {
            updateTimer -= Time.deltaTime;
        }
        else
        {
            //find nearest enemy with tag "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float nearestDistance = 0;
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (nearestEnemy == null || distance < nearestDistance)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
                    }
                }
            }
            updateTimer = maxUpdateTimer;
        }
        //rotate to nearest enemy and scale arrow alpha depending on distance
        if (nearestEnemy != null)
        {
            Vector3 difference = nearestEnemy.transform.position - transform.position;
            float distance = Vector2.Distance(transform.position, nearestEnemy.transform.position);

            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            if (distance > 0)
                arrow.color = new Color(1, 1, 1, 1 - 1 / (distance * alphaMod));

        }
        else
        {
            arrow.color = new Color(1, 1, 1, 0);
        }
    }
}
