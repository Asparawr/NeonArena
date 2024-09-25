using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedWithCenterController : MonoBehaviour
{
    public float brakSpeed = 1;
    public Transform parentObject;
    void Start()
    {

    }
    public void Break()
    {
        int count = parentObject.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = parentObject.GetChild(0);

            child.GetComponent<ScriptsEnabler>().EnableScripts();

            Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
            childRigidbody.velocity = child.localPosition * brakSpeed;
            childRigidbody.bodyType = RigidbodyType2D.Dynamic;
            child.parent = null;
        }
        GetComponent<Rigidbody2D>().mass = 1;
    }
}
