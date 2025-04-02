using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitals : MonoBehaviour
{
    public float damage;
    public float distance;
    public void Reposition()
    {
        int i = 0;
        int angle;
        angle = 360 / transform.childCount + 1;
        foreach (Transform child in transform)
        {
            child.localPosition = new Vector3(Mathf.Sin(i * angle * Mathf.Deg2Rad) * distance, Mathf.Cos(i * angle * Mathf.Deg2Rad) * distance, 0);
            i++;
        }
    }
}
