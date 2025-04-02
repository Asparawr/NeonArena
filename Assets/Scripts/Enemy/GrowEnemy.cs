using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowEnemy : MonoBehaviour
{
    public float scale = 3.3f;
    float currentScale = 0;
    public float growSpeed = 1;
    public GameObject enemy;
    Transform spawnedEnemy;
    public EnemyStats enemyStats;
    bool spawned = false;
    public bool isStatic = true;
    private void Start()
    {
        if (enemyStats == null)
            enemyStats = GetComponent<EnemyStats>();
    }
    void Update()
    {
        if (spawnedEnemy == null && !spawned)
        {
            spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity).transform;
            spawnedEnemy.transform.parent = gameObject.transform;
            spawnedEnemy.transform.localScale = new Vector3(0, 0, 0);
            spawned = true;
            //rotate towards 0
            spawnedEnemy.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (currentScale >= scale)
        {
            if (spawnedEnemy != null)
            {
                spawnedEnemy.GetComponent<ScriptsEnabler>().EnableScripts();
                spawnedEnemy.parent = enemyStats.transform.parent;
                //change layer to enemy
                spawnedEnemy.gameObject.layer = LayerMask.NameToLayer("SpawningEnemy");
                spawnedEnemy.GetComponent<EnemyStats>().Setup(enemyStats.difficultyMod, 0, 0);
                Rigidbody2D gameObjectsRigidBody = spawnedEnemy.gameObject.GetComponent<Rigidbody2D>();
                if (gameObjectsRigidBody == null)
                    gameObjectsRigidBody = spawnedEnemy.gameObject.AddComponent<Rigidbody2D>();
                gameObjectsRigidBody.gravityScale = 0;
                if (isStatic)
                    gameObjectsRigidBody.bodyType = RigidbodyType2D.Static;
                else
                    gameObjectsRigidBody.bodyType = RigidbodyType2D.Dynamic;
                spawnedEnemy = null;
            }
            spawned = false;
            currentScale = 0;
        }
        else
        {
            currentScale += growSpeed * Time.deltaTime;
            if (spawnedEnemy != null)
                spawnedEnemy.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }
}
