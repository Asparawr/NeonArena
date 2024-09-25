using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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
}
