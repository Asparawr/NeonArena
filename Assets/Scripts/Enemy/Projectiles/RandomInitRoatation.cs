using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInitRoatation : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
    }

}
