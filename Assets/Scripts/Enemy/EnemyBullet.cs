using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{ 
    
    void Start()
    {
        GameObject.Destroy(this.gameObject,2.0f);
    }

}
