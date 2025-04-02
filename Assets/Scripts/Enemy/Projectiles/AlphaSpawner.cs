using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSpawner : MonoBehaviour
{
    public float startingSpawnerTimer;
    public float spawnerTimer;
    public List<SpriteRenderer> sprites;
    void Start()
    {
        spawnerTimer = startingSpawnerTimer;
    }
    void Update()
    {
        if (spawnerTimer <= 0)
        {
            GetComponent<ScriptsEnabler>().EnableScripts();
            this.enabled = false;
        }
        else
        {
            foreach (var sprite in sprites)
            {
                spawnerTimer -= Time.deltaTime;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (1 - spawnerTimer) / startingSpawnerTimer);
            }
        }

    }
}
