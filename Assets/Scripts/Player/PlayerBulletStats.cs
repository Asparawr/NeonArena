using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletStats : MonoBehaviour
{
    public GameObject popupPrefab;

    public float damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<PopupController>().SetText(damage.ToString());
            collision.gameObject.GetComponent<EnemyStats>().UpdateHealth(-damage);
            Destroy(gameObject);
        }
    }
}
