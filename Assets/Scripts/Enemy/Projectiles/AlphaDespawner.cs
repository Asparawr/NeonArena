using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaDespawner : MonoBehaviour
{
    public float startingSpawnerTimer;
    public List<SpriteRenderer> sprites;
    float spawnerTimer;
    void Start()
    {
        spawnerTimer = startingSpawnerTimer;
    }
    private void OnEnable()
    {
        GetComponent<ScriptsEnabler>().DisableScripts();
    }
    void Update()
    {
        if (spawnerTimer <= 0)
            Destroy(gameObject);
        else
        {
            foreach (var sprite in sprites)
            {
                spawnerTimer -= Time.deltaTime;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, spawnerTimer / startingSpawnerTimer);
            }
        }

    }
}
