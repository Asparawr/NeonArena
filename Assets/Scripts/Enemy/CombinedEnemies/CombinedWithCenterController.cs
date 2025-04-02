using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedWithCenterController : MonoBehaviour
{
    public float brakSpeed = 1;
    public Transform parentObject;
    public Transform mainObject;
    public void BreakDisconnect()
    {
        transform.parent = null;
        Break();
        Destroy(gameObject);
    }

    public void Break()
    {
        int count = parentObject.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = parentObject.GetChild(0);
            if (child.gameObject.layer == LayerMask.NameToLayer("Enemy") || child.gameObject.layer == LayerMask.NameToLayer("SpawningEnemy"))
            {
                child.gameObject.layer = LayerMask.NameToLayer("SpawningEnemy");
                child.GetComponent<ScriptsEnabler>().EnableScripts();

                Rigidbody2D childRigidbody = child.GetComponent<Rigidbody2D>();
                childRigidbody.velocity = child.localPosition.normalized * brakSpeed;
                childRigidbody.bodyType = RigidbodyType2D.Dynamic;
                child.parent = mainObject.parent;
                child.GetComponent<EnemyStats>().hasParent = false;
            }
            else Destroy(child.gameObject);
        }
        //       GetComponent<Rigidbody2D>().mass = 1;
    }

    public void Reparent()
    {
        if (!GetComponent<EnemyStats>().hasParent)
        {
            foreach (Transform child in transform)
            {
                child.parent = transform.parent;
            }
        }
        else
            Break();
    }
}
