using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float startingLaserTimer;
    float laserTimer;
    public float laserStopTime;
    AlphaSpawner alphaSpawner;
    public Transform player;
    public float rotationSpeed = 1;

    void Start()
    {
        laserTimer = startingLaserTimer;
        alphaSpawner = GetComponent<AlphaSpawner>();
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (alphaSpawner.spawnerTimer <= 0)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                GetComponent<AlphaDespawner>().enabled = true;
                transform.parent = null;
                this.enabled = false;
            }
        }
        else if (alphaSpawner.spawnerTimer >= laserStopTime)
        {
            Vector3 vectorToTarget = player.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
        }
    }

}
