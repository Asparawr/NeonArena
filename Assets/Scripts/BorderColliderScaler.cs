using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderColliderScaler : MonoBehaviour
{
    public float ScaleX = 0;
    public float ScaleY = 0;
    public float BorderOffset = 0.3f;

    private SpriteRenderer spriteRenderer;
    private EdgeCollider2D edgeCollider;

    private Vector2[] colliderPoints;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        edgeCollider = gameObject.GetComponent<EdgeCollider2D>();
        ScaleColliders();
    }

    void ScaleColliders()
    {
        // get current scale
        ScaleX = spriteRenderer.size.x/2- BorderOffset;
        ScaleY = spriteRenderer.size.y/2- BorderOffset;

        // set edge collider points to scale
        colliderPoints = edgeCollider.points;
        colliderPoints[0] = new Vector2(-ScaleX, ScaleY);
        colliderPoints[1] = new Vector2(-ScaleX, -ScaleY);
        colliderPoints[2] = new Vector2(ScaleX, -ScaleY);
        colliderPoints[3] = new Vector2(ScaleX, ScaleY);
        colliderPoints[4] = new Vector2(-ScaleX, ScaleY);
        edgeCollider.points = colliderPoints;
    }
}
