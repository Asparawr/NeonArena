using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float shootingPeriod = 1; //in seconds
    public float scale = 1;
    private float shootingTimer;

    public EnemyStats enemyStats;
    public bool isEnabled = true;
    public List<GameObject> shootPoints;
    public GameObject projectile;

    void Start()
    {
        Setup();
    }
    void OnEnable()
    {
        Setup();
    }
    void Setup()
    {
        if (enemyStats == null)
            enemyStats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        shootingTimer += Time.deltaTime;
        if (isEnabled && shootingTimer > shootingPeriod)
        {
            shootingTimer = 0;
            Shoot();
        }
    }
    public void Shoot()
    {
        foreach (GameObject shootPoint in shootPoints)
        {
            GameObject bullet = Instantiate(projectile, shootPoint.transform.position, shootPoint.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * enemyStats.baseStats.projectileSpeed;
            bullet.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
            bullet.GetComponent<ProjectileStats>().enemyStats = enemyStats;
            Destroy(bullet, enemyStats.baseStats.range);
            // modiify scale by the scale of the enemy
            bullet.transform.localScale *= transform.localScale.x * scale;
        }
    }
    public void ShootRandom()
    {
        int randomIndex = Random.Range(0, shootPoints.Count);
        GameObject bullet = Instantiate(projectile, shootPoints[randomIndex].transform.position, shootPoints[randomIndex].transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * enemyStats.baseStats.projectileSpeed;
        bullet.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
        bullet.GetComponent<ProjectileStats>().enemyStats = enemyStats;
        Destroy(bullet, enemyStats.baseStats.range);
        // modiify scale by the scale of the enemy
        bullet.transform.localScale *= transform.localScale.x;
    }
}
