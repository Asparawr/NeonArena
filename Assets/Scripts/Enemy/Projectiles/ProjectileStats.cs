using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileStats : MonoBehaviour
{
    public float damage;
    public bool destroyOnCollison = false;
    public float damageDelay = -1;
    float delayTimer = 0;
    public UnityEvent DestroyEvent;
    public EnemyStats enemyStats;
    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ProjectileShield"))
            Destroy(gameObject);
        if (delayTimer <= 0 && this.enabled && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.damageImmunityTimer > playerController.playerStats.damageImmunity)
            {
                playerController.damageImmunityTimer = 0;
                var popup = Instantiate(playerController.damagePopupPrefab, other.transform.position, Quaternion.identity);
                var roundedDamage = Mathf.Ceil(damage);

                //add armor
                damage = Mathf.Min(0, damage + playerController.playerStats.armor);
                //check projectile negate chance
                if (UnityEngine.Random.Range(0f, 1f) < playerController.playerStats.projectileNegateChance)
                    roundedDamage = 0;
                //check shield
                if (playerController.playerStats.shieldActive && playerController.shieldTimer <= 0)
                    roundedDamage = 0;

                popup.GetComponent<PopupController>().SetText(roundedDamage.ToString());
                playerController.UpdateHealth(-roundedDamage);
                if (destroyOnCollison)
                {
                    DestroyEvent.Invoke();
                    Destroy(gameObject);
                }
                if (damageDelay >= 0)
                {
                    delayTimer = damageDelay;
                    this.enabled = false;
                }
            }
        }
    }
}
