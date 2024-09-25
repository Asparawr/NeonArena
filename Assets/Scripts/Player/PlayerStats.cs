using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // ui stats updates
    public GameObject UIHealth;

    private UIHealthController healthController;

    // base stats and % mods
    public int money = 0;
    public float movementSpeed = 1;
    public float movementSpeedMod = 1;

    public float currentHealth = 1;
    public float maxHealth = 1;
    public float maxHealthMod = 1;

    public float bulletSpeed = 1;
    public float bulletSpeedMod = 1;

    public float damage = 1;
    public float damageMod = 1;

    public float fireRate = 1;
    public float fireRateMod = 1;

    public float range = 1; //in seconds
    public float rangeMod = 1;

    public float moneyMod = 1;

    public float bulletScaleMod = 1;

    //script list to activate on bullet on spawn
    public List<string> bulletModList;

    // damage taking timers
    public float damageImmunity;
    public float damageImmunityTimer;

    private void Start()
    {
        healthController = UIHealth.GetComponent<UIHealthController>();
    }
    void Update()
    {
        damageImmunityTimer += Time.deltaTime;
    }

    public void AddItem(ShopItem item)
    {
        movementSpeedMod += item.movementSpeed;
        maxHealthMod += item.maxHealth;
        bulletSpeedMod += item.bulletSpeed;
        damageMod += item.damage;
        fireRateMod += item.fireRate;
        rangeMod += item.range;
        bulletScaleMod += item.bulletScale;

        foreach (string modName in item.bulletModList)
        {
            bulletModList.Add(modName);
        }
    }

    public void UpdateHealth(float value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthController.UpdateHealth(currentHealth / maxHealth * maxHealthMod);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (damageImmunityTimer > damageImmunity)
            {
                damageImmunityTimer = 0;
                UpdateHealth(-collision.gameObject.GetComponent<EnemyStats>().GetDamage());
            }
        }
    }
}
