using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnerPoint> spawnerPoints;

    void Update()
    {
        foreach (EnemySpawnerPoint spawnerPoint in spawnerPoints)
        {
            spawnerPoint.timer += Time.deltaTime;
            if (spawnerPoint.timer > spawnerPoint.interval)
            {
                if (spawnerPoint.targetTransform != null)
                {
                    var newEnemy = Instantiate(spawnerPoint.EnemyToSpawn, spawnerPoint.targetTransform.position, Quaternion.Euler(0, 0, spawnerPoint.degree));
                    newEnemy.GetComponent<Rigidbody2D>().velocity = spawnerPoint.velocity;
                    spawnerPoint.timer = 0;
                }
            }
        }
    }
}

//class for holding spawner points data
[System.Serializable]
public class EnemySpawnerPoint
{
    public Transform targetTransform;
    public Vector2 velocity;
    public float degree;
    public float interval;
    public float timer;
    public GameObject EnemyToSpawn;
}