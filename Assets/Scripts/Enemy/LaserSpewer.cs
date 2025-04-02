using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpewer : MonoBehaviour
{
    public float angle;
    float currentAngle;
    public float delay;
    float delayTimer;
    public float scale;
    public GameObject laserPrefab;
    EnemyStats enemyStats;
    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > delay)
        {
            delayTimer = 0;
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);//change to rotated
            laser.transform.parent = gameObject.transform;
            laser.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
            laser.transform.Rotate(Vector3.forward, currentAngle);
            laser.transform.localScale = new Vector3(scale, scale, scale);
            currentAngle += angle;
        }

    }
}
