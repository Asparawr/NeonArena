using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public float moveToPositionX;
    public float moveToPositionY;
    public float speed;
    public bool moving = true;
    public float delaySpawn = 0;
    public float delayMin;
    public float delayMax;
    public float delayTimer;
    public bool startAttack = false;
    void Start()
    {
        delayTimer = delaySpawn;
    }
    void Update()
    {
        if (moving)
        {
            var x = 0;
            var y = 0;
            if (moveToPositionX != 0) x = 1;
            if (moveToPositionY != 0) y = 1;
            transform.position -= speed * Time.deltaTime * new Vector3(x, y, 0);
            if (transform.position.y < moveToPositionY)
                moving = false;
        }
        delayTimer -= Time.deltaTime;
        if (!moving && delayTimer < 0)
        {
            delayTimer = Random.Range(delayMin, delayMax);
            startAttack = true;
        }
    }
}
