using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbtal : MonoBehaviour
{
    public GameObject damagePopupPrefab;
    Orbitals orbitals;
    private void Start()
    {
        damagePopupPrefab = Resources.Load("Prefabs/PlayerDamagePopup") as GameObject;
        orbitals = transform.parent.GetComponent<Orbitals>();
    }
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpawningEnemy") || collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            var damageDealt = collision.gameObject.GetComponent<EnemyStats>().UpdateHealth(-orbitals.damage);
            //damage popup
            var popup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            popup.GetComponent<PopupController>().SetText(damageDealt.ToString());
        }
    }
}
