using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject destroyParticlePrefab;
    public float radius = 2;

    private void OnDestroy()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transform.localScale *= radius;
        var destroyParticle = Instantiate(destroyParticlePrefab, transform.position, Quaternion.identity);
        var newProjectile = Instantiate(gameObject, transform.position, Quaternion.identity);
        destroyParticle.transform.localScale = transform.localScale;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(destroyParticle, 1f);
        Destroy(newProjectile, .2f);
    }
}

