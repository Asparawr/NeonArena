using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingWithProjectiles : MonoBehaviour
{
    public float initRotation = 0;
    public int numberOfProjectiles = 6;
    public float projectileSpeed = 10;
    public float range = 3;
    public GameObject projectilePrefab;
    public float scale = 1;
    public EnemyStats enemyStats;

    private void Start()
    {
        if (enemyStats == null)
            enemyStats = GetComponent<EnemyStats>();
    }
    public void Explode()
    {
        // add current rotation
        var baseRotation = initRotation + transform.rotation.eulerAngles.z;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            SpawnProjectile(Quaternion.Euler(0, 0, baseRotation + (360 / numberOfProjectiles) * i));
        }
    }
    public void ExplodeRandomized()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            SpawnProjectile(Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }
    void SpawnProjectile(Quaternion rotation)
    {
        var projectile = Instantiate(projectilePrefab, transform.position, rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * projectileSpeed;
        projectile.transform.localScale *= scale;
        projectile.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
        Destroy(projectile, range);
    }
}
