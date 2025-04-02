using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    public float sizeDifficultyMod = 1;
    public bool hasParent = false;
    public GameObject player;
    public BaseEnemyStats baseStats;
    GameObject damagePopupPrefab;
    public float speedMod = 1;
    public float slowTimer = 0;
    public float health;
    public float maxHealth;
    public float difficultyMod = 1;
    public float price = 1;

    // event triggering when destroying gameobject
    public UnityEvent Destroyed;

    public UnityEvent HealthChanged;

    public float borderPosX;
    public float borderPosY;
    private bool isInside = false;
    public List<Transform> setupChildren;
    public List<Transform> deathParticleList;
    public bool PlayDeathSound = true;

    float spawnTimer = 0;
    public float spawnTimerTreshold = 1;

    //immunities
    public bool slowImmunity = false;
    public bool burnImmunity = false;
    public bool poisonImmunity = false;
    public bool stunImmunity = false;
    public bool knockbackImmunity = false;
    public bool damageImmunity = false;
    public bool destroyImmunity = false;

    // Partcle spawning
    public GameObject debuffParticles;

    //damage over time
    public float burnDamage = 0;
    public float burnTimerMax = 0;
    float burnTimer = 0;
    float burnTickMax = 0.4f;
    float burnTickTimer = 0;
    public float poisonDamage = 0;
    public float poisonTimerMax = 0;
    float poisonTimer = 0;
    public float poisonTickMax = 1f;
    float poisonTickTimer = 0;
    float stunTimer = 0;
    bool isStunned = false;
    float slowMod = 0;
    public bool destroying = false;
    void Start()
    {
        damagePopupPrefab = Resources.Load("Prefabs/PlayerDamagePopup") as GameObject;
    }
    private void Update()
    {
        // effect timers
        if (slowTimer < 0)
        {
            if (!isStunned)
                speedMod = 1;
            DisableParticle("Slow");
        }
        else
            slowTimer -= Time.deltaTime;
        if (burnTimer < 0)
        {
            DisableParticle("Burn");
        }
        else
        {
            burnTimer -= Time.deltaTime;
            if (burnTickTimer < 0)
            {
                burnTickTimer = burnTickMax;
                var damageDealt = UpdateHealth(-burnDamage);
                var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
                popup.GetComponent<PopupController>().SetText(damageDealt.ToString());
            }
            else
                burnTickTimer -= Time.deltaTime;
        }
        if (poisonTimer < 0)
        {
            DisableParticle("Poison");
        }
        else
        {
            poisonTimer -= Time.deltaTime;
            if (poisonTickTimer < 0)
            {
                poisonTickTimer = poisonTickMax;
                var damageDealt = UpdateHealth(-poisonDamage);
                var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
                popup.GetComponent<PopupController>().SetText(damageDealt.ToString());
            }
            else
                poisonTickTimer -= Time.deltaTime;
        }
        if (stunTimer < 0)
        {
            speedMod = 1 - slowMod;
            DisableParticle("Stun");
        }
        else
            stunTimer -= Time.deltaTime;
        spawnTimer += Time.deltaTime;
        float posy = Mathf.Abs(transform.position.y);
        if (gameObject.layer != LayerMask.NameToLayer("Boss"))
        {
            if (!isInside && !hasParent && spawnTimer > spawnTimerTreshold && borderPosX > Mathf.Abs(transform.position.x) + 3 && borderPosY > Mathf.Abs(transform.position.y) + 3)
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                isInside = true;
            }
            if (borderPosX < Mathf.Abs(transform.position.x) + 4 && borderPosY < Mathf.Abs(transform.position.y) + 4)
            {
                isInside = false;
                gameObject.layer = LayerMask.NameToLayer("SpawningEnemy");
            }
        }

    }
    public void Setup(float difficulty, float borderPosX, float borderPosY)
    {
        this.borderPosX = borderPosX;
        this.borderPosY = borderPosY;
        difficultyMod = difficulty * sizeDifficultyMod;
        player = GameObject.Find("Player");
        SetHealth();
        foreach (var child in setupChildren)
        {
            var childStats = child.GetComponent<EnemyStats>();
            childStats.Setup(difficulty, borderPosX, borderPosY);
            childStats.hasParent = true;
        }
    }
    public void SetHealth()
    {
        health = baseStats.health * difficultyMod;
        maxHealth = health;
    }
    public float UpdateHealth(float value)
    {
        if (damageImmunity && value < 0)
        {
            return 0;
        }
        health += value;
        if (health <= 0 && !destroyImmunity)
        {
            destroying = true;
            DestroySelf();
        }
        HealthChanged.Invoke();
        return -value;
    }

    public float GetDamage()
    {
        return baseStats.damage * difficultyMod;
    }

    public void DestroySelf()
    {
        Destroyed.Invoke();
        var particleSpawner = GetComponent<ParticleSpawner>();
        if (particleSpawner != null)
            particleSpawner.SpawnParticle();
        if (PlayDeathSound)
        {
            FindObjectOfType<AudioManager>().Play("Pop");
        }
        foreach (var child in deathParticleList)
        {
            if (child != null)
                child.GetComponent<ParticleSpawner>().SpawnParticle();
        }
        Destroy(gameObject);
    }
    public void SetSlow(float slow, float slowDuration)
    {
        if (!slowImmunity)
        {
            if (slowTimer < 0)
            {
                SetParticle("Slow");
            }
            if (!isStunned)
                speedMod = slow;
            slowMod = 1 - slow;
            slowTimer = slowDuration;
        }
    }
    public void SetBurn(float burnDamage, float burnTimerMax, float burnTickMax)
    {
        if (!burnImmunity)
        {
            SetParticle("Burn");
            this.burnDamage = burnDamage;
            this.burnTimerMax = burnTimerMax;
            this.burnTickMax = burnTickMax;
            burnTimer = burnTimerMax;
        }
    }
    public void SetPoison(float poisonDamage, float poisonTimerMax, float poisonTickMax)
    {
        if (!poisonImmunity)
        {
            SetParticle("Poison");
            this.poisonDamage = poisonDamage;
            this.poisonTimerMax = poisonTimerMax;
            this.poisonTickMax = poisonTickMax;
            poisonTimer = poisonTimerMax;
        }
    }
    public void SetStun(float stunDuration)
    {
        if (!stunImmunity)
        {
            SetParticle("Stun");
            isStunned = true;
            speedMod = 0;
            stunTimer = stunDuration;
        }
    }
    public void SetParticle(string name)
    {
        if (debuffParticles == null)
        {
            debuffParticles = Instantiate(Resources.Load<GameObject>("Prefabs/DebuffParticles"), gameObject.transform);
        }
        debuffParticles.GetComponent<DebuffParticles>().SetParticle(name);
    }
    public void DisableParticle(string name)
    {
        if (debuffParticles != null)
            debuffParticles.GetComponent<DebuffParticles>().DisableParticle(name);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAura"))
        {
            if (other.gameObject.tag == "SlowAura")
            {
                var slowAura = other.gameObject.GetComponent<SlowAura>();
                SetSlow(slowAura.slow, slowAura.slowDuration);
            }
            if (other.gameObject.tag == "BurnAura")
            {
                var burnAura = other.gameObject.GetComponent<BurnAura>();
                SetBurn(burnAura.burnDamage, burnAura.burnDuration, burnAura.burnTickRate);
            }
        }
    }
    public void SwitchDamageImmunity()
    {
        damageImmunity = !damageImmunity;
    }
}
