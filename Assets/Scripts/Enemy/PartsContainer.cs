using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsContainer : MonoBehaviour
{
    public void CheckChildren()
    {
        // destroys when 0 children
        if (transform.childCount <= 1)
        {
            Destroy(gameObject);
        }
    }
}
