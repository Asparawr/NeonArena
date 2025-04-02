using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "Item", menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    //ui variables
    public Sprite iconSprite;
    public Rarity rarity;

    //wheteher it can be bought several times
    public bool stackable = true;

    //stat changes
    public float movementSpeed = 0;
    public float maxHealth = 0;
    public float bulletSpeed = 0;
    public float damage = 0;
    public float fireRate = 0;
    public float range = 0;
    public float bulletScale = 0;
    public float moneyMod = 0;
    public float playerScale = 0;
    public int respawnCount = 0;
    public float critChance = 0;
    public float critDam = 0;


    public int bulletPenetration = 0;

    //in seconds
    public float damageImmunity = 0;

    //rarity shop chances
    public float rareChance = 0;
    public float epicChance = 0;
    public float legendaryChance = 0;

    //bullet effects
    public float split;
    public float slowAura;
    public float slow;
    public float burnAura;
    public float burn;
    public float poison;
    public float homing;
    public float stun; // in seconds
    public float stunChance;
    public float vampirysm;
    public float bounce;
    public float spread; // in degrees

    //ranger
    public float threeShotChance = 0;
    public float fiveShotChance = 0;
    public float mineChance = 0;
    public float eightShotChance = 0;
    public float backShotChance = 0;
    public float startingSpeed = 0;
    //warrior
    public float knockBack = 0;
    public float attackKillBoost = 0;
    public float fireRateKillBoost = 0;
    public float attackLowBoost = 0;
    public float fireRateLowBoost = 0;
    public float addProjectileChance = 0;
    public float retaliationDamage = 0;
    public float bulletShieldSize = 0;
    public float projectileNegateChance = 0;
    public float armor = 0;
    public float lowHealthThreshold = 0f;
    public float shieldTimer = 0;

    //orbitals
    public int orbitalCount;
    public float orbitalDamage;
    public float orbitalSpeed;

    //chance based
    public float fireballChance = 0;
    public float fireballScale = 0;

    // Update is called once per frame
    public string GetEffects()
    {
        string output = "";
        if (movementSpeed != 0) output += "Speed " + PositiveFloatCheck(movementSpeed) + (movementSpeed * 100).ToString() + "%\n";
        if (maxHealth != 0) output += "Health " + PositiveFloatCheck(maxHealth) + (maxHealth * 100).ToString() + "%\n";
        if (bulletSpeed != 0) output += "Bullet speed " + PositiveFloatCheck(bulletSpeed) + (bulletSpeed * 100).ToString() + "%\n";
        if (damage != 0) output += "Dmg " + PositiveFloatCheck(damage) + (damage * 100).ToString() + "%\n";
        if (fireRate != 0) output += "Fire Rate " + PositiveFloatCheck(fireRate) + (fireRate * 100).ToString() + "%\n";
        if (range != 0) output += "Range " + PositiveFloatCheck(range) + (range * 100).ToString() + "%\n";
        if (moneyMod != 0) output += "Money gain " + PositiveFloatCheck(moneyMod) + (moneyMod * 100).ToString() + "%\n";
        if (playerScale != 0) output += "Player size " + PositiveFloatCheck(playerScale) + (playerScale * 100).ToString() + "%\n";
        if (respawnCount != 0) output += PositiveFloatCheck(respawnCount) + respawnCount.ToString() + " lives\n";
        if (critChance != 0) output += "Crit chance " + PositiveFloatCheck(critChance) + (critChance * 100).ToString() + "%\n";
        if (critDam != 0) output += "Crit dmg " + PositiveFloatCheck(critDam) + (critDam * 100).ToString() + "%\n";

        if (bulletPenetration != 0) output += "Penetration " + PositiveFloatCheck(bulletPenetration) + bulletPenetration.ToString() + "\n";
        if (damageImmunity != 0) output += "Dmg Immunity on hit " + PositiveFloatCheck(damageImmunity) + (damageImmunity).ToString() + " seconds\n";
        if (bulletScale != 0) output += "Bullet size " + PositiveFloatCheck(bulletScale) + (bulletScale * 100).ToString() + "%\n";
        if (rareChance != 0) output += "Rare chance " + PositiveFloatCheck(rareChance) + (rareChance).ToString() + "%\n";
        if (epicChance != 0) output += "Epic chance " + PositiveFloatCheck(epicChance) + (epicChance).ToString() + "%\n";
        if (legendaryChance != 0) output += "Legendary chance " + PositiveFloatCheck(legendaryChance) + (legendaryChance).ToString() + "%\n";
        if (split != 0) output += "Split " + PositiveFloatCheck(split) + split.ToString() + "\n";
        if (slowAura != 0) output += "Slow Aura " + PositiveFloatCheck(slowAura) + (slowAura * 100).ToString() + "%\n";
        if (slow != 0) output += "Slow " + PositiveFloatCheck(slow) + (slow * 100).ToString() + "%\n";
        if (burnAura != 0) output += "Burn Aura dmg " + PositiveFloatCheck(burnAura) + burnAura.ToString() + "\n";
        if (burn != 0) output += "Burn dmg " + PositiveFloatCheck(burn) + burn.ToString() + "\n";
        if (poison != 0) output += "Poison dmg " + PositiveFloatCheck(poison) + poison.ToString() + "\n";
        if (homing != 0) output += "Homing " + PositiveFloatCheck(homing) + (homing * 100).ToString() + "%\n";
        if (stun != 0) output += "Stun " + PositiveFloatCheck(stun) + stun.ToString() + " seconds\n";
        if (stunChance != 0) output += "Stun chance " + PositiveFloatCheck(stunChance) + (stunChance * 100).ToString() + "%\n";
        if (vampirysm != 0) output += "Vampirysm " + PositiveFloatCheck(vampirysm) + vampirysm.ToString() + "%\n";
        if (bounce != 0) output += "Bounce " + PositiveFloatCheck(bounce) + bounce.ToString() + "\n";
        if (spread != 0) output += "Spread " + PositiveFloatCheck(spread) + spread.ToString() + " degrees\n";
        if (threeShotChance != 0) output += "Three shot " + PositiveFloatCheck(threeShotChance) + (threeShotChance * 100).ToString() + "%\n";
        if (fiveShotChance != 0) output += "Five shot " + PositiveFloatCheck(fiveShotChance) + (fiveShotChance * 100).ToString() + "%\n";
        if (mineChance != 0) output += "Mine " + PositiveFloatCheck(mineChance) + (mineChance * 100).ToString() + "%\n";
        if (eightShotChance != 0) output += "Eight shot " + PositiveFloatCheck(eightShotChance) + (eightShotChance * 100).ToString() + "%\n";
        if (backShotChance != 0) output += "Back shot " + PositiveFloatCheck(backShotChance) + (backShotChance * 100).ToString() + "%\n";
        if (startingSpeed != 0) output += "Projectile speed boost" + PositiveFloatCheck(startingSpeed) + (startingSpeed * 100).ToString() + "%\n";

        if (knockBack != 0) output += "Knockback " + PositiveFloatCheck(knockBack) + knockBack.ToString() + "\n";
        if (attackKillBoost != 0) output += "Attack on kill " + PositiveFloatCheck(attackKillBoost) + (attackKillBoost * 100).ToString() + "%\n";
        if (fireRateKillBoost != 0) output += "Fire rate on kill " + PositiveFloatCheck(fireRateKillBoost) + (fireRateKillBoost * 100).ToString() + "%\n";
        if (attackLowBoost != 0) output += "Low attack " + PositiveFloatCheck(attackLowBoost) + (attackLowBoost * 100).ToString() + "%\n";
        if (fireRateLowBoost != 0) output += "Low fire rate " + PositiveFloatCheck(fireRateLowBoost) + (fireRateLowBoost * 100).ToString() + "%\n";
        if (addProjectileChance != 0) output += "Additional projectile " + PositiveFloatCheck(addProjectileChance) + (addProjectileChance * 100).ToString() + "%\n";
        if (retaliationDamage != 0) output += "Spike dmg " + PositiveFloatCheck(retaliationDamage) + (retaliationDamage * 100).ToString() + "%\n";
        if (bulletShieldSize != 0) output += "Bullet shield size " + PositiveFloatCheck(bulletShieldSize) + (bulletShieldSize * 100).ToString() + "%\n";
        if (projectileNegateChance != 0) output += "Negate projectile " + PositiveFloatCheck(projectileNegateChance) + (projectileNegateChance * 100).ToString() + "%\n";
        if (lowHealthThreshold != 0) output += "Low treshold " + PositiveFloatCheck(lowHealthThreshold) + (lowHealthThreshold * 100).ToString() + "%\n";
        if (armor != 0) output += "Armor " + PositiveFloatCheck(armor) + armor.ToString() + "\n";
        if (shieldTimer != 0) output += "Dmg shield respawn " + PositiveFloatCheck(shieldTimer) + (shieldTimer * 100).ToString() + "%\n";

        if (orbitalDamage != 0) output += "Orbital dmg " + PositiveFloatCheck(orbitalDamage) + orbitalDamage.ToString() + "\n";
        if (orbitalCount != 0) output += "Orbital count " + PositiveFloatCheck(orbitalCount) + orbitalCount.ToString() + "\n";
        if (orbitalSpeed != 0) output += "Orbital speed " + PositiveFloatCheck(orbitalSpeed) + (orbitalSpeed * 100).ToString() + "%\n";
        if (fireballChance != 0) output += "Fireball chance " + PositiveFloatCheck(fireballChance) + (fireballChance * 100).ToString() + "%\n";
        if (fireballScale != 0) output += "Fireball size " + PositiveFloatCheck(fireballScale) + (fireballScale * 100).ToString() + "%\n";

        return output;
    }

    string PositiveFloatCheck(float number)
    {
        if (number > 0) return "+";
        return "";
    }
}