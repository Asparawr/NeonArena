using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public List<EnemySpawnerPoint> spawnerPoints;
    EnemyStats enemyStats;
    public Transform main;
    public List<GameObject> mines;
    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        foreach (EnemySpawnerPoint spawnerPoint in spawnerPoints)
        {
            spawnerPoint.timer += Time.deltaTime;
            if (spawnerPoint.timer > spawnerPoint.interval)
            {
                if (spawnerPoint.targetTransform != null)
                {
                    var newMine = Instantiate(spawnerPoint.EnemyToSpawn, spawnerPoint.targetTransform.position, Quaternion.Euler(0, 0, spawnerPoint.degree));
                    newMine.GetComponent<Rigidbody2D>().velocity = spawnerPoint.velocity;
                    newMine.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
                    if (main != null)
                        newMine.transform.parent = main.parent;
                    else
                        newMine.transform.parent = transform.parent;
                    spawnerPoint.timer = 0;
                    mines.Add(newMine);
                }
            }
        }
    }
    public void DestroyMines()
    {
        foreach (GameObject mine in mines)
        {
            if (mine != null)
                mine.GetComponent<EnemyStats>().DestroySelf();
        }
    }
}
