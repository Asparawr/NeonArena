using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject spawnObject;
    public void SpawnParticle()
    {
        // spawn particle
        transform.parent = null;
        GameObject particleObject = Instantiate(spawnObject, transform.position, transform.rotation);

        if (spriteRenderer != null)
        {
            particleObject.transform.localScale = spriteRenderer.transform.lossyScale;
        }
        else
        {
            particleObject.transform.localScale = transform.lossyScale;
        }

        //set color to parent object color
        var main = particleObject.GetComponent<ParticleSystem>().main;
        Color color = new Color();
        if (spriteRenderer != null)
        {
            color = spriteRenderer.color;
        }
        else
        {
            color = GetComponent<SpriteRenderer>().color;
        }
        main.startColor = color;

        //set rotaion
        particleObject.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }
}
