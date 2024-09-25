using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "EnemyStats", order = 1)]
public class BaseEnemyStats : ScriptableObject
{
    public float health;
    public float damage;
    public float bulletSpeed;
    public float rateOfFire;
    public float movementSpeed;
    public float range; // in seconds

    public float price;
}
