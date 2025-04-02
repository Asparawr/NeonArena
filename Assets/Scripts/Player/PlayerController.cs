using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject moveJoystickObject;
    public GameObject shootJoystickObject;
    public GameObject playerModel;
    public GameObject gameManager;
    public BorderColliderScaler borderColliderScaler;

    private FixedJoystick moveJoystick;
    private FixedJoystick shootJoystick;
    private Rigidbody2D playerRigidbody;
    public PlayerStats playerStats;
    public float slowSpeedMod = 1;

    private PauseController pauseController;

    private Quaternion newRotation;
    public float rotationSpeed = 1;

    public GameObject bulletSpawner;
    public GameObject playerProjectile;
    private float shootCooldown;

    // ui stats updates
    public UIHealthController healthController;

    // damage taking timers
    public float damageImmunityTimer;
    public GameObject damagePopupPrefab;
    public GameObject healPopupPrefab;

    // auras
    public GameObject slowAura;
    public GameObject burnAura;
    SlowAura slowAuraComponent;
    public GameObject orbitals;
    public GameObject orbitalPrefab;
    public GameObject fireballPrefab;
    public GameObject shield;
    public float shieldTimer;
    public PlayerDebuffHandler debuffHandler;
    public bool invurnerable = false;
    public float onKillTimerMax = 5;
    public float onKillTimer = 0;
    public GameObject addProjectilePrefab;
    public GameObject projectileShield;
    GameObject playerDamagePopupPrefab;
    private void Start()
    {
        playerDamagePopupPrefab = Resources.Load("Prefabs/PlayerDamagePopup") as GameObject;
        moveJoystick = moveJoystickObject.GetComponent<FixedJoystick>();
        shootJoystick = shootJoystickObject.GetComponent<FixedJoystick>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        pauseController = gameManager.GetComponent<PauseController>();
        slowAuraComponent = slowAura.GetComponent<SlowAura>();

        //set item chances
        ItemHelper itemHelper = new ItemHelper();
        playerStats.rareChance = itemHelper.itemChances["Rare"];
        playerStats.epicChance = itemHelper.itemChances["Epic"];
        playerStats.legendaryChance = itemHelper.itemChances["Legendary"];

        // update stats
        UpdateAuras();
        UpdateOrbitals();
        UpdatePlayerScale();
        if (playerStats.currentHealth > playerStats.maxHealth)
            playerStats.currentHealth = playerStats.maxHealth;
    }

    void Update()
    {
        damageImmunityTimer += Time.deltaTime;

        //pause game when no joysticks are being used
        if (!moveJoystick.touchStart && !shootJoystick.touchStart)
        {
            pauseController.Pause();
        }
        else
        {
            pauseController.Unpause();
        }

        //open pause menu on back button press
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseController.OpenPauseMenu();

        //movement
        if (moveJoystick.direction != Vector2.zero)
            playerRigidbody.velocity += moveJoystick.direction * slowSpeedMod * playerStats.movementSpeed * Time.deltaTime;

        //check if player is outside of the border and tp back
        if (transform.position.x > borderColliderScaler.ScaleX)
            transform.position = new Vector3(borderColliderScaler.ScaleX, transform.position.y, transform.position.z);
        if (transform.position.x < -borderColliderScaler.ScaleX)
            transform.position = new Vector3(-borderColliderScaler.ScaleX, transform.position.y, transform.position.z);
        if (transform.position.y > borderColliderScaler.ScaleY)
            transform.position = new Vector3(transform.position.x, borderColliderScaler.ScaleY, transform.position.z);
        if (transform.position.y < -borderColliderScaler.ScaleY)
            transform.position = new Vector3(transform.position.x, -borderColliderScaler.ScaleY, transform.position.z);

        //slerp rotation
        if (playerModel)
        {
            newRotation = Quaternion.FromToRotation(Vector3.up, shootJoystick.direction);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            playerModel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, playerModel.transform.rotation.eulerAngles.z));
        }
        shootCooldown -= Time.deltaTime;
        //onKillTimer
        if (onKillTimer > 0)
        {
            onKillTimer -= Time.deltaTime;
            if (onKillTimer <= 0)
            {
                onKillTimer = 0;
            }
        }
        //shield timer
        if (playerStats.shieldActive && shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldTimer = 0;
                shield.SetActive(true);
            }
        }
        if (shootJoystick.touchStart && shootCooldown <= 0f)
        {
            //shoot cooldown
            shootCooldown = playerStats.fireRate;
            //shoot coldown Kill mod
            if (onKillTimer > 0)
                shootCooldown *= playerStats.fireRateKillBoost;
            //shoot cooldown low health mod
            if (playerStats.currentHealth / playerStats.maxHealth < playerStats.lowHealthThreshold)
                shootCooldown *= playerStats.fireRateLowBoost;

            if (UnityEngine.Random.Range(0f, 1f) < playerStats.backShotChance)
                Fire(180);
            if (UnityEngine.Random.Range(0f, 1f) < playerStats.threeShotChance)
            {
                //three shot
                var angle = -30;
                for (int i = 0; i < 3; i++)
                {
                    Fire(angle);
                    angle += 30;
                }
            }
            else if (UnityEngine.Random.Range(0f, 1f) < playerStats.fiveShotChance)
            {
                //five shot
                for (int i = 0; i < 5; i++)
                {
                    //random angle from -45 to 45
                    Fire(UnityEngine.Random.Range(-45, 45));
                }
            }
            else if (UnityEngine.Random.Range(0f, 1f) < playerStats.eightShotChance)
            {
                //eight shot
                var angle = transform.rotation.z;
                for (int i = 0; i < 8; i++)
                {
                    Fire(angle);
                    angle += 45;
                }
            }
            else
            {
                //normal shot
                Fire(0);
            }

            //additional projectile
            if (UnityEngine.Random.Range(0f, 1f) < playerStats.addProjectileChance)
            {
                var projectile = Instantiate(addProjectilePrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
                //setup stats
                PlayerBulletStats bulletStats = projectile.GetComponent<PlayerBulletStats>();
                projectile.GetComponent<Rigidbody2D>().velocity = playerModel.transform.up * playerStats.bulletSpeed;
                bulletStats.speed = playerStats.bulletSpeed;
                bulletStats.damage = playerStats.damage;
                Destroy(projectile, playerStats.range * 3);
            }
        }
    }
    private void Fire(float angle)
    {
        //randomly fire fireball based on freballChance
        if (UnityEngine.Random.Range(0f, 1) < playerStats.fireballChance)
        {
            GameObject fireball = Instantiate(fireballPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
            //set  stats
            fireball.transform.localScale *= playerStats.bulletScale * (1 + playerStats.fireballScale);
            fireball.GetComponent<Rigidbody2D>().velocity = playerModel.transform.up * playerStats.bulletSpeed;

            fireball.GetComponent<Fireball>().damage = playerStats.damage * 2;
            fireball.GetComponent<Fireball>().range = playerStats.range;
        }
        else
        {
            var bullet = Instantiate(playerProjectile, bulletSpawner.transform.position, playerModel.transform.rotation);

            //set bullet stats
            bullet.transform.localScale *= playerStats.bulletScale;

            //change rotation randomly based on spread
            bullet.transform.Rotate(0, 0, transform.rotation.z + angle + UnityEngine.Random.Range(-playerStats.spread, playerStats.spread));

            //set velocity
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * (playerStats.bulletSpeed + playerStats.startingSpeed * playerStats.bulletSpeed);

            //setup stats
            PlayerBulletStats bulletStats = bullet.GetComponent<PlayerBulletStats>();
            bulletStats.speed = playerStats.bulletSpeed;
            bulletStats.damage = playerStats.damage;
            //if onKill mod damage
            if (onKillTimer > 0)
                bulletStats.damage += playerStats.damage * playerStats.attackKillBoost;
            //if low health mod damage
            if (playerStats.currentHealth / playerStats.maxHealth < playerStats.lowHealthThreshold)
                bulletStats.damage += playerStats.damage * playerStats.attackLowBoost;

            //activate bullet effects
            bulletStats.playerController = this;
            bulletStats.penetration += playerStats.bulletPenetration;
            bulletStats.split = playerStats.split;
            bulletStats.slow = playerStats.slow;
            bulletStats.critChance = playerStats.critChance;
            bulletStats.critDamage = playerStats.critDamage;
            bulletStats.burnDamage = playerStats.burnDamage;
            bulletStats.poisonDamage = playerStats.poisonDamage;
            bulletStats.vampirysm = playerStats.vampirysm;
            bulletStats.bounce = playerStats.bounce;
            bulletStats.startingSpeedDecrease *= playerStats.startingSpeed;
            bulletStats.knockBack = playerStats.knockBack;

            //add stun
            if (UnityEngine.Random.Range(0f, 1f) < playerStats.stunChance)
                bulletStats.stun = playerStats.stun;

            //handle mine
            if (UnityEngine.Random.Range(0f, 1f) < playerStats.mineChance)
            {
                bulletStats.range = playerStats.range * 4;
                bullet.GetComponent<Rigidbody2D>().drag = 2.5f;
                Destroy(bullet, playerStats.range * 3);
            }
            else
            {
                bulletStats.range = playerStats.range;
                Destroy(bullet, playerStats.range);
            }
        }
    }

    public void UpdatePlayerScale()
    {
        transform.localScale = new Vector3(1, 1, 1) * playerStats.playerScale;
    }
    public void UpdateAuras()
    {
        if (playerStats.slowAura < 1)
        {
            slowAura.SetActive(true);
            slowAuraComponent.slow = playerStats.slowAura;
        }

        if (playerStats.burnAura > 0)
        {
            burnAura.SetActive(true);
            burnAura.GetComponent<BurnAura>().burnDamage = playerStats.burnAura;
        }
    }
    public void UpdateOrbitals()
    {
        if (playerStats.orbitalDamage > 0)
        {
            var orbitalsComponent = orbitals.GetComponent<Orbitals>();
            orbitals.SetActive(true);
            if (orbitals.transform.childCount < playerStats.orbitalCount)
            {
                for (int i = orbitals.transform.childCount; i < playerStats.orbitalCount; i++)
                {
                    GameObject orbital = Instantiate(orbitalPrefab, orbitals.transform);
                    orbitalsComponent.damage = playerStats.orbitalDamage;
                    orbitals.GetComponent<RotateAtSpeed>().speed = playerStats.orbitalSpeed;
                }
                // reposition
                orbitalsComponent.Reposition();
            }

            orbitalsComponent.damage = playerStats.orbitalDamage;
            orbitals.GetComponent<RotateAtSpeed>().speed = playerStats.orbitalSpeed;
        }
    }
    public void OnKill()
    {
        onKillTimer = onKillTimerMax;
    }
    public void DisableJoysticks()
    {
        moveJoystick.isEnabled = false;
        shootJoystick.isEnabled = false;
        moveJoystick.ResetJoystick();
        shootJoystick.ResetJoystick();
    }

    public void EnableJoysticks()
    {
        moveJoystick.isEnabled = true;
        shootJoystick.isEnabled = true;
    }

    public void AddItem(ShopItem item)
    {
        playerStats.moneyMod += item.moneyMod;
        playerStats.movementSpeed *= 1 + item.movementSpeed;
        playerStats.maxHealth *= 1 + item.maxHealth;
        playerStats.bulletSpeedMod *= 1 + item.bulletSpeed;
        playerStats.damage *= 1 + item.damage;
        playerStats.fireRate /= 1 + item.fireRate;
        playerStats.range *= 1 + item.range;
        playerStats.bulletScale *= 1 + item.bulletScale;
        playerStats.playerScale *= 1 + item.playerScale;
        playerStats.damageImmunity += item.damageImmunity;
        playerStats.respawnCount += item.respawnCount;
        playerStats.critChance += item.critChance;
        playerStats.critDamage *= 1 + item.critDam;

        playerStats.rareChance += item.rareChance / 100;
        playerStats.epicChance += item.epicChance / 100;
        playerStats.legendaryChance += item.legendaryChance / 100;

        playerStats.bulletPenetration += item.bulletPenetration;
        playerStats.split += item.split;
        playerStats.slowAura *= 1 - item.slowAura;
        playerStats.slow *= 1 - item.slow;
        playerStats.burnAura += item.burnAura;
        playerStats.burnDamage += item.burn;
        playerStats.poisonDamage += item.poison;
        playerStats.homing += item.homing;

        playerStats.vampirysm += item.vampirysm;
        playerStats.bounce += item.bounce;
        playerStats.spread += item.spread;
        playerStats.threeShotChance += item.threeShotChance;
        playerStats.fiveShotChance += item.fiveShotChance;
        playerStats.mineChance += item.mineChance;
        playerStats.eightShotChance += item.eightShotChance;
        playerStats.backShotChance += item.backShotChance;
        playerStats.startingSpeed += item.startingSpeed;

        playerStats.stunChance += item.stunChance;
        playerStats.stun += item.stun;
        playerStats.knockBack += item.knockBack;
        playerStats.attackKillBoost += item.attackKillBoost;
        playerStats.fireRateKillBoost *= 1 - item.fireRateKillBoost;
        playerStats.attackLowBoost += item.attackLowBoost;
        playerStats.fireRateLowBoost *= 1 - item.fireRateLowBoost;
        playerStats.addProjectileChance += item.addProjectileChance;
        playerStats.retaliationDamage += item.retaliationDamage;
        playerStats.projectileShieldSize += item.bulletShieldSize;
        playerStats.projectileNegateChance += item.projectileNegateChance;
        playerStats.armor += item.armor;
        playerStats.lowHealthThreshold *= 1 + item.lowHealthThreshold;
        playerStats.shieldTimerMax *= 1 + item.shieldTimer;

        playerStats.orbitalDamage += item.orbitalDamage;
        playerStats.orbitalSpeed *= 1 + item.orbitalSpeed;
        playerStats.orbitalCount += item.orbitalCount;

        playerStats.fireballChance += item.fireballChance;
        playerStats.fireballScale *= 1 + item.fireballScale;

        UpdateAuras();
        UpdateOrbitals();
        UpdatePlayerScale();
        if (item.shieldTimer != 0 && !playerStats.shieldActive)
        {
            playerStats.shieldActive = true;
            shieldTimer = playerStats.shieldTimerMax;
        }
        if (item.bulletShieldSize != 0)
        {
            projectileShield.SetActive(true);
            //change scale to match size
            projectileShield.transform.localScale = new Vector3(1, 1 + playerStats.projectileShieldSize, 1);
        }

    }

    public void UpdateHealth(float value)
    {
        if (invurnerable && value < 0)
            return;
        //handle shield
        if (playerStats.shieldActive && shieldTimer <= 0)
        {
            shieldTimer = playerStats.shieldTimerMax;
            shield.SetActive(false);
            damageImmunityTimer = playerStats.damageImmunity - 0.1f;
            return;
        }

        if (value < 0)
            damageImmunityTimer = 0;
        playerStats.currentHealth += value;

        if (value > 0 && playerStats.currentHealth < playerStats.maxHealth)
        {
            var popup = Instantiate(healPopupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<PopupController>().SetText(value.ToString());
        }

        if (playerStats.currentHealth > playerStats.maxHealth)
            playerStats.currentHealth = playerStats.maxHealth;

        healthController.UpdateHealth(playerStats.currentHealth / playerStats.maxHealth);
        if (playerStats.currentHealth <= 0)
            gameManager.GetComponent<TurnController>().HandleDeath();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpawningEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (damageImmunityTimer > playerStats.damageImmunity)
            {

                var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
                var damage = Mathf.Ceil(collision.gameObject.GetComponent<EnemyStats>().GetDamage());

                //add armor
                damage = Mathf.Max(0, damage - playerStats.armor);
                popup.GetComponent<PopupController>().SetText(damage.ToString());
                UpdateHealth(-damage);
                //retaliation
                if (playerStats.retaliationDamage > 0)
                {
                    var damageDealt = collision.transform.GetComponent<EnemyStats>().UpdateHealth(-playerStats.retaliationDamage * playerStats.damage);
                    var retPopup = Instantiate(playerDamagePopupPrefab, transform.position, Quaternion.identity);
                    retPopup.GetComponent<PopupController>().SetText(damageDealt.ToString());
                }
            }
        }
    }


}
