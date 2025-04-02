using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBoss : MonoBehaviour
{
    public int phase = 0;
    EnemyStats enemyStats;
    public List<Transform> adds;
    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckHealth()
    {
        // switch case phases from 0 to 2
        switch (phase)
        {
            case 0:
                if (enemyStats.health <= enemyStats.maxHealth * 3 / 4)
                {
                    LaunchAdds();
                    phase = 1;
                }
                break;
            case 1:
                if (enemyStats.health <= enemyStats.maxHealth / 2)
                {
                    LaunchAdds();
                    phase = 2;
                }
                break;
            case 2:
                if (enemyStats.health <= enemyStats.maxHealth / 4)
                {
                    LaunchAdds();
                }
                break;
            default:
                break;
        }
    }
    public void LaunchAdds()
    {
        adds[phase * 2].GetComponent<ScriptsEnabler>().EnableScripts();
        adds[phase * 2 + 1].GetComponent<ScriptsEnabler>().EnableScripts();
        adds[phase * 2].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1));
        adds[phase * 2 + 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1));
        adds[phase * 2].GetComponent<SendHealthChange>().enabled = false;
        adds[phase * 2 + 1].GetComponent<SendHealthChange>().enabled = false;
        adds[phase * 2].GetComponent<YoYoLaunch>().enabled = false;
        adds[phase * 2 + 1].GetComponent<YoYoLaunch>().enabled = false;
        var enemyStats1 = adds[phase * 2].GetComponent<EnemyStats>();
        enemyStats1.health = enemyStats1.maxHealth;
        enemyStats1.destroyImmunity = false;
        var enemyStats2 = adds[phase * 2 + 1].GetComponent<EnemyStats>();
        enemyStats2.health = enemyStats2.maxHealth;
        enemyStats2.destroyImmunity = false;
        adds[phase * 2].parent = transform.parent;
        adds[phase * 2 + 1].parent = transform.parent;
    }
}
