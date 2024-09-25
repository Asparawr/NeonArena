using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public void SpawnParticle()
    {
        // spawn particle
        GameObject particleObject = Instantiate(spawnObject, transform.position, Quaternion.identity);

        //set color to parent object color
        var main = particleObject.GetComponent<ParticleSystem>().main;
        Color color = GetComponent<SpriteRenderer>().color;
        main.startColor = color;
    }
}
