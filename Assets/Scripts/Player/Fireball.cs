using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    GameObject damagePopupPrefab;
    public GameObject destroyParticlePrefab;
    public List<Transform> hitEnemies;
    public float damage;
    public float range;
    bool destroying = false;
    float startTime;
    private void Start()
    {
        startTime = Time.time;
        damagePopupPrefab = Resources.Load("Prefabs/PlayerDamagePopup") as GameObject;
    }
    private void Update()
    {
        if (Time.time - startTime > range)
        {
            if (!destroying) InitializeDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpawningEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            //filter already hit
            if (hitEnemies.Contains(transform))
                return;
            hitEnemies.Add(collision.transform);
            var damageDealt = collision.gameObject.GetComponent<EnemyStats>().UpdateHealth(-damage);
            //damage popup
            var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<PopupController>().SetText(Mathf.Ceil(damageDealt).ToString());
            if (!destroying) InitializeDestroy();
        }
    }
    void InitializeDestroy()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        destroying = true;
        transform.localScale *= 2;
        var destroyParticle = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
        destroyParticle.transform.localScale = transform.localScale;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(destroyParticle, 1f);
        Destroy(gameObject, .2f);
    }
}
