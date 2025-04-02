using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedBossHealth : MonoBehaviour
{
    GameObject bossHealthObject;
    UIHealthController bossHealth;
    float maxHealth;
    void Start()
    {
        bossHealthObject = GameObject.Find("BossHealth");
        bossHealth = bossHealthObject.GetComponent<UIHealthController>();
        bossHealth.ShowBar();
        foreach (var child in transform.GetComponentsInChildren<EnemyStats>())
        {
            maxHealth += child.maxHealth;
        }
    }
    void FixedUpdate()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        var health = 0f;
        foreach (var child in transform.GetComponentsInChildren<EnemyStats>())
        {
            health += child.health;
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
