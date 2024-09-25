using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{

    public GameObject player;
    public BaseEnemyStats baseStats;
    public float health;
    public float difficultyMod = 1;

    // event triggering when destroying gameobject
    public UnityEvent Destroyed;

    public void Start()
    {
        //#TODO move to spawner 
        player = GameObject.Find("Player");
        SetHealth();
    }
    public void SetHealth()
    {
        health = baseStats.health * difficultyMod;
    }
    public void UpdateHealth(float value)
    {
        health += value;
        if (health <= 0)
        {
            DestroySelf();
        }
    }


    public float GetDamage()
    {
        return baseStats.damage * difficultyMod;
    }

    public void DestroySelf()
    {
        GetComponent<ParticleSpawner>().SpawnParticle();
        Destroyed.Invoke();
        Destroy(gameObject);
    }
}
