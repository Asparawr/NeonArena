using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnekHeadDebuff : MonoBehaviour
{
    EnemyFollowing enemyFollowing;
    void Start()
    {
        enemyFollowing = GetComponent<EnemyFollowing>();
    }

    public void Debuff()
    {
        enemyFollowing.accelerationSpeed /= 2;
    }
}
