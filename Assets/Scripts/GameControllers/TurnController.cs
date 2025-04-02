using Enum = System.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class TurnController : MonoBehaviour
{
    public float difficulty = 1;
    public int turnCount = 0;
    public PauseController pauseController;
    public BorderColliderScaler borderColliderScaler;
    public float enemySpawnX;
    public float enemySpawnY;
    public float enemyValueStarting;
    public float enemyValueIncrease;
    public float enemyValue;
    public float leftEnemyValue;
    public float difficultyIncrease;
    public float enemySpawnTimerMaxBase = 0.05f;
    public float enemySpawnTimerMax = 0.05f;
    private float enemySpawnTimer;
    public Transform SpawnedEnemies;
    public bool isShopOpened = false;
    GameShopController shopController;
    public GameSetup gameSetup;
    public bool started = false;
    public GameObject deathMenu;
    public TextMeshProUGUI respawnText;
    public Button respawnButton;
    public float cameraSpawnOffset = 13;
    public float borderSpawnOffset = 2.5f;

    void Start()
    {
        enemyValue = enemyValueStarting;
        pauseController = GetComponent<PauseController>();
        shopController = GetComponent<GameShopController>();
        gameSetup = GetComponent<GameSetup>();
        Time.timeScale = 1.5f;

        //set spawn variables
        enemySpawnX = borderColliderScaler.ScaleX;
        enemySpawnY = borderColliderScaler.ScaleY;
    }

    private void Update()
    {
        if (turnCount % 10 == 0 || gameSetup.map.enemies.Count == 0)
        {//boss turn
            if (leftEnemyValue > 0)
            {
                int enemyId = 0;
                int enemySet = 0;
                if (turnCount <= gameSetup.map.bosses.Count * 10)//dont repeat boss lists
                {
                    enemySet = System.Math.Max(0, (int)(turnCount / 10) - 1);
                    enemyId = Random.Range(0, gameSetup.map.bosses[enemySet].boss.Count);
                }
                else
                {
                    enemySet = Random.Range(0, gameSetup.map.bosses.Count);
                    enemyId = Random.Range(0, gameSetup.map.bosses[enemySet].boss.Count);
                }
                var enemyStats = gameSetup.map.bosses[enemySet].boss[enemyId].GetComponent<EnemyStats>().baseStats;
                SpawnEnemy(gameSetup.map.bosses[enemySet].boss[enemyId], enemyStats.spawnPointX, enemyStats.spawnPointY);
                leftEnemyValue = -1;
            }
        }
        else if (enemySpawnTimer > 0)
        {//normal turn countdown
            enemySpawnTimer -= Time.deltaTime;
            if (SpawnedEnemies.childCount <= 10)
                enemySpawnTimer = 0;
        }
        else if (leftEnemyValue > 0)
        {//normal turn
            int enemySet = Random.Range(0, Mathf.Min((int)(turnCount / 10), gameSetup.map.enemies.Count));
            //int enemySet = (int)(turnCount / 10);
            int enemyId = Random.Range(0, gameSetup.map.enemies[enemySet].enemy.Count);
            SpawnEnemy(gameSetup.map.enemies[enemySet].enemy[enemyId]);
            if (SpawnedEnemies.childCount >= 10)
                enemySpawnTimerMax = enemySpawnTimerMaxBase + (SpawnedEnemies.childCount - 10) * 0.5f;
            else
                enemySpawnTimerMax = enemySpawnTimerMaxBase;
            enemySpawnTimer = enemySpawnTimerMax;
        }

        if (!isShopOpened && SpawnedEnemies.childCount == 0 && started && leftEnemyValue <= 0)
        {//turn end
            if (turnCount % 10 == 0)
                shopController.playerController.playerStats.money += gameSetup.helper.bossMoneyGain * shopController.playerController.playerStats.moneyMod;
            else
                shopController.playerController.playerStats.money += gameSetup.helper.moneyGain * shopController.playerController.playerStats.moneyMod;
            gameSetup.playerController.invurnerable = true;
            // if (turnCount % 9 == 0)
            //     gameSetup.ShowAdInter();
            shopController.OpenShop();
        }
    }

    public void StartNextTurn()
    {
        gameSetup.playerController.playerStats.currentHealth = gameSetup.playerController.playerStats.maxHealth;
        gameSetup.playerController.invurnerable = false;
        pauseController.player.GetComponent<PlayerController>().UpdateHealth(pauseController.player.GetComponent<PlayerController>().playerStats.maxHealth / 5 * (5 - gameSetup.helper.hardMapLevels[gameSetup.mapDifficulty] / .5f));
        leftEnemyValue = enemyValue;
        enemyValue += enemyValueIncrease;
        difficulty += difficultyIncrease * gameSetup.helper.hardMapLevels[gameSetup.mapDifficulty];
        enemySpawnTimer = 0;
        turnCount += 1;
        started = true;
    }
    public void SpawnEnemy(GameObject enemy, float posX = 0, float posY = 0)
    {
        GameObject newEnemy = Instantiate(enemy, SpawnedEnemies);
        newEnemy.GetComponent<EnemyStats>().Setup(difficulty, borderColliderScaler.ScaleX, borderColliderScaler.ScaleY);
        leftEnemyValue -= newEnemy.GetComponent<EnemyStats>().price;

        //set position
        if (posX != 0 || posY != 0)//special coords enemies
            newEnemy.transform.localPosition = new Vector3(posX, posY, 0);
        else
        {//default enemies
            Vector3 newPosition;
            //while position is in range of camera
            do
            {
                newPosition = new Vector3(Random.Range(-enemySpawnX + borderSpawnOffset, enemySpawnX - borderSpawnOffset), Random.Range(-enemySpawnY + borderSpawnOffset, enemySpawnY - borderSpawnOffset), 0);
            } while (Vector3.Distance(newPosition, gameSetup.playerController.transform.position) < cameraSpawnOffset);
            newEnemy.transform.localPosition = newPosition;
        }
    }
    public void HandleDeath()
    {
        pauseController.paused = false;
        pauseController.Pause(false);
        gameSetup.playerController.DisableJoysticks();
        deathMenu.SetActive(true);

        //set respawn text
        Color respawnTextColor = respawnText.color;
        if (gameSetup.playerController.playerStats.respawnCount > 0)
        {
            respawnTextColor.a = 1;
            respawnText.text = "[" + gameSetup.playerController.playerStats.respawnCount + " left]";
        }
        else if (gameSetup.playerController.playerStats.adRespawned == false)
        {
            respawnTextColor.a = 1;
            if (gameSetup.saveManager.state.adBlock)
                respawnText.text = "[free]";
            else
            {
                respawnText.text = "[watch ad]";
            }
        }
        else
        {
            respawnText.text = "[" + gameSetup.playerController.playerStats.respawnCount + " left]";
            respawnTextColor.a = 0.5f;
            respawnButton.interactable = false;
        }
        respawnText.color = respawnTextColor;

    }
    public void Respawn()
    {
        if (gameSetup.playerController.playerStats.respawnCount > 0)
        {
            gameSetup.playerController.playerStats.respawnCount -= 1;
            RespawnLaunch();
        }
        else
        {
            gameSetup.ShowAd();
        }
    }
    public void RespawnLaunch()
    {
        gameSetup.playerController.damageImmunityTimer = -3;
        //reset debuff timers
        gameSetup.playerController.debuffHandler.ResetDebuffTimers();

        gameSetup.playerController.playerStats.currentHealth = gameSetup.playerController.playerStats.maxHealth;

        deathMenu.SetActive(false);
        pauseController.Unpause();
        gameSetup.playerController.EnableJoysticks();
    }
    public void Exit()
    {
        gameSetup.Abandon();
    }
}
