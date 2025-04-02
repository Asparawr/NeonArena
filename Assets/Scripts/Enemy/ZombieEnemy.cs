using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    ScriptsEnabler scriptsEnabler;
    public float maxRespawnTimer = 5;
    public SpriteRenderer spriteRenderer;
    Color startingColor;
    public Color respawnColor;
    float respawnTimer;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        startingColor = spriteRenderer.color;
        scriptsEnabler = GetComponent<ScriptsEnabler>();
    }

    void Update()
    {
        //respawn timer
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startingColor, respawnColor, respawnTimer / maxRespawnTimer);
        }
        else
        {
            spriteRenderer.color = startingColor;
            if (!enemyStats.destroyImmunity)
            {
                enemyStats.destroyImmunity = true;
                scriptsEnabler.EnableScripts();
                enemyStats.health = enemyStats.maxHealth / 2;
            }
        }
    }
    public void CheckStatus()
    {
        if (enemyStats.health <= 0)
        {
            enemyStats.destroyImmunity = false;
            //disable scripts
            scriptsEnabler.DisableScripts();
            //set color to respawn color
            spriteRenderer.color = respawnColor;
            //set respawn timer
            respawnTimer = maxRespawnTimer;
        }
    }
}
