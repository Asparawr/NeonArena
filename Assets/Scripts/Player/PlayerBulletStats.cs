using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBulletStats : MonoBehaviour
{
    public GameObject damagePopupPrefab;
    public PlayerController playerController;
    public float damage = -100;
    public float range;
    public float speed = 1;
    public List<Transform> hitEnemies;
    public int penetration = 1;
    public bool infiniteHits = false;
    float spawnTime = 0;
    public float split = 0;
    public float slow;
    public float slowTime = 2;
    public float burnDamage;
    public float burnTime = 3;
    public float burnTickRate = 0.4f;
    public float poisonDamage;
    public float poisonTime = 5;
    public float poisonTickRate = 1f;
    public float vampirysm;
    public float bounce;
    public float startingSpeed;
    public float startingSpeedDecrease = 6;
    public float critChance;
    public float critDamage;
    public float stun;
    public float knockBack;
    Rigidbody2D rb;
    bool isDestroyed = false;
    void Start()
    {
        spawnTime = Time.time;
        damagePopupPrefab = Resources.Load("Prefabs/PlayerDamagePopup") as GameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //gradually decrease velocity, when above starting speed+speed
        if (rb.velocity.magnitude > startingSpeed + speed)
        {
            rb.velocity -= rb.velocity.normalized * Time.deltaTime * startingSpeedDecrease;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpawningEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (isDestroyed) return;
            var collisionTransform = collision.transform;
            //check if collision sends collision to parent
            var colHealthChange = collision.gameObject.GetComponent<SendHealthChange>();
            if (collision.gameObject.GetComponent<SendHealthChange>() != null)
            {
                collisionTransform = colHealthChange.mainEnemyStats.transform;
            }
            //filter already hit
            if (hitEnemies.Contains(collisionTransform))
                return;
            hitEnemies.Add(collisionTransform);

            //penetration
            if (!infiniteHits && bounce <= 0)
                penetration--;
            if (penetration <= 0 && bounce <= 0)
            {
                Destroy(gameObject);
                isDestroyed = true;
            }

            var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            //add crit
            if (Random.Range(0f, 1f) < critChance)
            {
                damage *= critDamage;
                var popupText = popup.GetComponent<TextMeshPro>();
                popupText.color = Color.white;
                //set popup text scale to 1.2
                popupText.fontSize *= 1.2f;
            }
            //update health
            var enemyStats = collisionTransform.gameObject.GetComponentInParent<EnemyStats>();
            var damageDealt = enemyStats.UpdateHealth(-damage);

            popup.GetComponent<PopupController>().SetText(Mathf.Ceil(damageDealt).ToString());

            //apply slow
            if (slow < 1)
                enemyStats.SetSlow(slow, slowTime);

            //apply burn
            if (burnDamage > 0)
                enemyStats.SetBurn(burnDamage, burnTime, burnTickRate);
            //apply poison
            if (poisonDamage > 0)
                enemyStats.SetPoison(poisonDamage, poisonTime, poisonTickRate);
            //apply vampirysm
            if (vampirysm > 0)
                playerController.UpdateHealth(vampirysm);

            //apply knockback
            if (knockBack > 0)
            {
                var enemyRb = collisionTransform.gameObject.GetComponentInParent<Rigidbody2D>();
                enemyRb.AddForce((collisionTransform.position - transform.position).normalized * knockBack * 10, ForceMode2D.Impulse);
            }
            //apply stun
            if (stun > 0)
                enemyStats.SetStun(stun);

            //on kill buffs
            if (enemyStats.destroying)
                playerController.OnKill();

            //bounce
            if (bounce > 0)
            {
                //randomize direction
                var randomAngle = Random.Range(0, 360);
                var randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
                //rotate towards ditection rotated
                var angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                //change direction
                var rigidbody = GetComponent<Rigidbody2D>();
                rigidbody.velocity = randomDirection * speed;
                bounce -= 1;
            }

            //split
            if (split > 0)
            {
                var splitCount = split;
                split = 0;
                for (int i = 0; i < splitCount; i++)
                {
                    GameObject bullet = Instantiate(gameObject, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    bullet.GetComponent<PlayerBulletStats>().hitEnemies = hitEnemies;
                    bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * speed;
                    Destroy(bullet, range - (Time.time - spawnTime));
                    //set scale to 0.5
                    bullet.transform.localScale *= 0.5f;
                    bullet.GetComponent<PlayerBulletStats>().damage *= 0.5f;
                }
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                GetComponent<Rigidbody2D>().velocity = transform.up * speed;
            }
        }
    }
}
