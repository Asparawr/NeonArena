using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionDetector : MonoBehaviour
{

    public bool colliding = false;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 0)
            colliding = true;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == 0)
            colliding = false;
    }

}