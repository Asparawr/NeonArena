using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMoveSwap : MonoBehaviour
{
    public float maxSwitchTimer;
    public float minSwitchTimer;
    public float switchTimer;
    EnemyStats enemyStats;
    EnemyMoveTowards enemyMoveTowards;
    EnemyDashing enemyDashing;
    // Start is called before the first frame update
    void Start()
    {
        enemyMoveTowards = GetComponent<EnemyMoveTowards>();
        enemyDashing = GetComponent<EnemyDashing>();

    }

    // Update is called once per frame
    void Update()
    {
        //switch enabled scripts based on timer
        switchTimer -= Time.deltaTime;
        if (switchTimer <= 0)
        {
            switchTimer = Random.Range(minSwitchTimer, maxSwitchTimer);
            if (enemyMoveTowards.enabled)
            {
                enemyMoveTowards.enabled = false;
                enemyDashing.enabled = true;
            }
            else
            {
                enemyMoveTowards.enabled = true;
                enemyDashing.enabled = false;
            }
        }

    }
}
