using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageProjectileParticle : MonoBehaviour
{
    GameObject projectile;
    public bool destroying = false;
    public float scale = 0.4f;
    private void Start()
    {
        SetColor(transform.parent.GetComponent<SpriteRenderer>().color);
        projectile = transform.parent.gameObject;
        scale *= GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>().playerStats.bulletScale;
        transform.rotation = transform.parent.rotation;
        transform.parent = null;
        transform.localScale = new Vector3(scale, scale, scale);
    }
    private void Update()
    {
        if (projectile != null)
            transform.position = projectile.transform.position;
        else if (!destroying)
        {
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
            destroying = true;
        }

    }
    public void SetColor(Color color)
    {
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }
}
