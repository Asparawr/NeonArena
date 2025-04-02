using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnemy : MonoBehaviour
{
    public float invisibilityTimerMax = 2;
    float invisibilityTimer = 0;
    public float colorChangeSpeed = 0.1f;
    bool isGrowing = false;
    public float minVisibility = 0.1f;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (invisibilityTimer < 0)
        {
            isGrowing = false;
        }
        else if (invisibilityTimer > 0)
        {
            invisibilityTimer -= Time.deltaTime;
        }
        else if (isGrowing)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, colorChangeSpeed * Time.deltaTime);
            if (spriteRenderer.color.a >= 1)
            {
                isGrowing = false;
                invisibilityTimer = invisibilityTimerMax;
            }
        }
        else
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.clear, colorChangeSpeed * Time.deltaTime);
            if (spriteRenderer.color.a <= minVisibility)
                isGrowing = true;
        }
    }
}
