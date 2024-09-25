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
    public int price = 0;
    public Rarity rarity;

    //wheteher it can be bought several times
    public bool stackable = true;

    //stat changes
    public float movementSpeed = 0;
    public float maxHealth = 0;
    public float bulletSpeed = 0;
    public float damage=0;
    public float fireRate = 0;
    public float size=0;
    public float range = 0; //in seconds
    public float bulletScale = 0;
    public float moneyMod = 0;

    public List<string> bulletModList;

    //rarity shop chances
    public float rareChance = 0;
    public float epicChance = 0;
    public float legendaryChance = 0;

    // Update is called once per frame
    public string GetEffects()
    {
        string output = "";
        if (movementSpeed > 0) output += "Speed " + PositiveFloatCheck(movementSpeed) + (movementSpeed * 100).ToString() + "%\n";
        if (maxHealth != 0) output += "Health " + PositiveFloatCheck(maxHealth) + (maxHealth * 100).ToString() + "%\n";
        if (bulletSpeed != 0) output += "Bullet speed " + PositiveFloatCheck(bulletSpeed) + (bulletSpeed * 100).ToString() + "%\n";
        if (damage != 0) output += "Damage " + PositiveFloatCheck(damage) + (damage * 100).ToString() + "%\n";
        if (fireRate != 0) output += "Fire Rate " + PositiveFloatCheck(fireRate) + (fireRate*100).ToString() + "%\n";
        if (size != 0) output += "Size " + PositiveFloatCheck(size) + (size * 100).ToString() + "%\n";
        if (range != 0) output += "Range " + PositiveFloatCheck(range) + (range * 100).ToString() + "%\n";
        if (moneyMod != 0) output += "Money gain " + PositiveFloatCheck(moneyMod) + (moneyMod * 100).ToString() + "%\n";
        if (bulletScale != 0) output += "Bullet size " + PositiveFloatCheck(bulletScale) + (bulletScale * 100).ToString() + "%\n";
        if (rareChance != 0) output += "Rare chance " + PositiveFloatCheck(rareChance) + (rareChance).ToString() + "%\n";
        if (epicChance != 0) output += "Epic chance " + PositiveFloatCheck(epicChance) + (epicChance).ToString() + "%\n";
        if (legendaryChance != 0) output += "Legendary chance" + PositiveFloatCheck(legendaryChance) + (legendaryChance).ToString() + "%\n";

        foreach (string modName in bulletModList)
        {
            if (modName == "BouncingEffect") output += "Bouncing Effect\n";
        }

        return output;
    }

    string PositiveFloatCheck(float number)
    {
        if (number > 0) return "+";
        return "";
    }
}
