using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProjectile : MonoBehaviour
{
    public void Setup(float xVelocity, float yVelocity, float speed, float range)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity, yVelocity) * speed;
        Destroy(gameObject, range);
    }
    public void Update()
    {

    }
}
