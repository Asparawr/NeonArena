using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public BaseEnemyStats stats;
    private GameObject player;
    private float angle;
    private Vector3 dir;
    public GameObject bulletSpawner;
    private float time;
    public GameObject bulletPrefab;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    
    void Update()
    {
        time += Time.deltaTime;
        if (time >= stats.rateOfFire)
        {
            time = 0.0f;
            Shoot();
        }
    }

    private void LateUpdate()
    {
        dir = player.transform.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Vector2.Distance(this.transform.position, player.transform.position) > 5.0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.movementSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * stats.bulletSpeed;
    }
}
