using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoss : MonoBehaviour
{
    BossAttacks bossAttacks;
    EnemyStats enemyStats;
    bool armorReleased = false;
    int attackType = 0;
    public CombinedWithCenterController armorBreakController;
    public GameObject laserPrefab;
    public GameObject weavePrefab;
    public GameObject enemyPrefab;
    public Transform laserSpawnRight;
    public Transform laserSpawnLeft;
    public Transform weaveSpawnCenter;
    public Transform weaveSpawnRight;
    public Transform weaveSpawnLeft;
    public Transform enemySpawnRight;
    public Transform enemySpawnLeft;
    GameObject enemyRight;
    GameObject enemyLeft;
    void Start()
    {
        bossAttacks = GetComponent<BossAttacks>();
        enemyStats = GetComponent<EnemyStats>();
        enemyStats.Setup(1, 100, 100);//TODO remove
    }

    void Update()
    {
        if (bossAttacks.startAttack)
        {
            bossAttacks.startAttack = false;
            if (enemyRight == null && enemyLeft == null)
                attackType = Random.Range(0, 3);
            else
                attackType = Random.Range(0, 2);
            switch (attackType)
            {
                case 0: //laser 
                    var laserRight = Instantiate(laserPrefab, laserSpawnRight.position, Quaternion.identity);
                    var laserLeft = Instantiate(laserPrefab, laserSpawnLeft.position, Quaternion.identity);
                    laserRight.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
                    laserLeft.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
                    break;

                case 1: //weave 
                    var weaveRight = Instantiate(weavePrefab, weaveSpawnRight.position, weaveSpawnRight.rotation);
                    var weaveLeft = Instantiate(weavePrefab, weaveSpawnLeft.position, weaveSpawnLeft.rotation);
                    weaveRight.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
                    weaveLeft.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
                    weaveRight.GetComponent<MovingProjectile>().Setup(0, -1, 5, 10);
                    weaveLeft.GetComponent<MovingProjectile>().Setup(0, -1, 5, 10);
                    if (armorReleased)
                    {
                        var weaveCenter = Instantiate(weavePrefab, weaveSpawnCenter.position, weaveSpawnCenter.rotation);
                        weaveCenter.GetComponent<ProjectileStats>().damage = enemyStats.GetDamage();
                        weaveCenter.GetComponent<MovingProjectile>().Setup(0, -1, 5, 10);
                    }
                    break;

                case 2: //spawn
                    enemyRight = Instantiate(enemyPrefab, enemySpawnRight.position, enemySpawnRight.rotation);
                    enemyLeft = Instantiate(enemyPrefab, enemySpawnLeft.position, enemySpawnLeft.rotation);
                    enemyRight.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
                    enemyRight.transform.parent = transform.parent;
                    enemyLeft.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
                    enemyLeft.transform.parent = transform.parent;
                    break;

                default:
                    break;
            }
        }
        if (!armorReleased && enemyStats.health < enemyStats.baseStats.health * enemyStats.difficultyMod / 2)
        {
            armorReleased = true;
            armorBreakController.Break();
        }
    }
}
