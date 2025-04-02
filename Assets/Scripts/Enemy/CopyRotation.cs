using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public Transform objectTocopy;

    void Update()
    {
        if (objectTocopy != null)
            transform.rotation = objectTocopy.rotation;
    }
}
