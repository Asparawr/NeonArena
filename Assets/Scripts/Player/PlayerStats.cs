using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{

    // base stats and % mods
    public float money = 0;
    public float moneyMod = 1;
    public float movementSpeed = 1;

    public float currentHealth = 1;
    public float maxHealth = 1;

    public float bulletSpeed = 1;
    public float bulletSpeedMod = 1;

    public float damage = 1;
    public float fireRate = 1;
    public float range = 1; //in seconds
    public float bulletScale = 1;
    public float playerScale = 1;
    public float damageImmunity = 0.5f;
    public float critChance = 0;
    public float critDamage = 2;

    //effects variables
    public bool warriorAttack = false;
    public int bulletPenetration = 0;
    public float split = 0;
    public float slowAura = 1;
    public float burnAura = 0;
    public float slow = 1;
    public float burnDamage = 0;
    public float poisonDamage = 0;
    public float homing = 0;
    public float stunChance = 0;
    public float stun = .5f; // in seconds
    public float shieldTimerMax = 20;
    public bool shieldActive = false;

    //ranger
    public float vampirysm = 0;
    public float bounce = 0;
    public float spread = 0;
    public float threeShotChance = 0;
    public float fiveShotChance = 0;
    public float mineChance = 0;
    public float eightShotChance = 0;
    public float backShotChance = 0;
    public float startingSpeed = 0;

    //warrior
    public float knockBack = 0;
    public float attackKillBoost = 0;
    public float fireRateKillBoost = 1;
    public float attackLowBoost = 0;
    public float fireRateLowBoost = 1;
    public float addProjectileChance = 0;
    public float retaliationDamage = 0;
    public float projectileShieldSize = 0;
    public float projectileNegateChance = 0;
    public float armor = 0;
    public float lowHealthThreshold = 0.3f;

    // orbitals
    public float orbitalDamage = 0;
    public int orbitalCount = 1;
    public float orbitalSpeed = 20;

    //chance based
    public float fireballChance = 0;
    public float fireballScale = 1.5f;

    //debuffs on player
    public float slowDebuff = 1;
    public float burnDebuff = 1;
    public float poisonDebuff = 1;



    //item rarity chances
    public float rareChance = 0;
    public float epicChance = 0;
    public float legendaryChance = 0;
    public int respawnCount = 0;
    public bool adRespawned = false;
}
