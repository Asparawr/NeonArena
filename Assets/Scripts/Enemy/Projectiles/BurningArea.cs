using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningArea : MonoBehaviour
{
    public ParticleSystem particle;
    float timer = 0;
    public float duration = 2;
    SpriteRenderer sprite;
    public float baseScale = 4;
    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
        sprite = GetComponent<SpriteRenderer>();
        //set particle scale to match parent
        particle.transform.localScale = new Vector3(particle.transform.localScale.x * transform.localScale.x / baseScale, particle.transform.localScale.y * transform.localScale.y / baseScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //set color alpha to zero over time and destroy when alpha is zero
        Color color = sprite.color;
        color.a = timer / duration;
        sprite.color = color;
        if (timer <= 0)
        {
            // set particle to stop emmiting new particles
            particle.Stop();
            var ProjectileStats = GetComponent<ProjectileStats>();
            ProjectileStats.enabled = false;
            Destroy(gameObject, 8);
            enabled = false;
        }
    }
}
