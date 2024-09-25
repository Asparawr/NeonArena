using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyCollisionDetector : MonoBehaviour
{

    public bool colliding = false;
    void OnCollisionEnter2D(Collision2D col)
    {
        colliding = true;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        colliding = false;
    }
}
