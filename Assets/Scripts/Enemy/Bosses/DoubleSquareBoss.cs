using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSquareBoss : MonoBehaviour
{
    EnemyStats enemyStats;
    BossAttacks bossAttacks;
    EnemyDashing enemyDashing;
    EnemyShooting enemyShooting;
    ExplodingWithProjectiles explodingWithProjectiles;
    public GameObject explodingAddPrefab;
    public float rotationSpeed = 1;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        bossAttacks = GetComponent<BossAttacks>();
        enemyDashing = GetComponent<EnemyDashing>();
        enemyShooting = GetComponent<EnemyShooting>();
        explodingWithProjectiles = GetComponent<ExplodingWithProjectiles>();
    }
    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void TryAttack()
    {
        if (bossAttacks.startAttack)
        {
            bossAttacks.startAttack = false;
            int attack = Random.Range(0, 2);
            switch (attack)
            {
                case 0:
                    //  explode
                    explodingWithProjectiles.Explode();
                    break;
                case 1:
                    //spawn add
                    var add = Instantiate(explodingAddPrefab, transform.position, Quaternion.identity);
                    add.transform.parent = gameObject.transform.parent.parent;
                    add.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, enemyStats.borderPosX, enemyStats.borderPosY);
                    break;
            }
        }
    }
    public void Enrage()
    {
        enemyStats.difficultyMod *= 1.5f;
        bossAttacks.delayMax *= 0.5f;
        bossAttacks.delayMin *= 0.5f;
        enemyDashing.dashingPeriod *= 0.5f;
        rotationSpeed *= 2f;
        enemyShooting.shootingPeriod *= 0.5f;
        //grow size over time 
        StartCoroutine(Grow());
    }
    IEnumerator Grow()
    {
        float size = transform.localScale.x;
        while (transform.localScale.x < size * 1.5f)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
