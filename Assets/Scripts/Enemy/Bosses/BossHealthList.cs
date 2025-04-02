using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthList : MonoBehaviour
{
    public List<EnemyStats> enemyStatsList;
    UIHealthController bossHealth;
    float maxHealth;
    GameObject bossHealthObject;
    public float updateDelay;
    void Start()
    {
        bossHealthObject = GameObject.Find("BossHealth");
        bossHealth = bossHealthObject.GetComponent<UIHealthController>();
        bossHealth.ShowBar();
        foreach (var stats in enemyStatsList)
        {
            maxHealth += stats.maxHealth;
        }
    }
    void FixedUpdate()
    {
        if (updateDelay <= 0)
        {
            UpdateHealth();
        }
        else
            updateDelay -= Time.deltaTime;
    }
    public void UpdateHealth()
    {
        var health = 0f;
        foreach (var stats in enemyStatsList)
        {
            if (stats != null)
                health += stats.health;
        }
        bossHealth.UpdateHealth(health / maxHealth);
        if (health <= 0)
        {
            bossHealth.HideBar();
            Destroy(gameObject);
        }
    }
    public void HideHealth()
    {
        bossHealth.HideBar();
    }
}
