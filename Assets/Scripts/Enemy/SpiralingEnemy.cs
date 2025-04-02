using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralingEnemy : MonoBehaviour
{
    public Transform enemy1;
    public Transform enemy2;
    Vector3 enemy1Pos;
    Vector3 enemy2Pos;
    public float changeSpeed = 1;

    private void Start()
    {
        enemy1Pos = enemy1.localPosition;
        enemy2Pos = enemy2.localPosition;

    }
    void Update()
    {
        //gradually exchange positions of the two enemies, y position slower and x faster
        enemy1.localPosition = Vector3.MoveTowards(enemy1.localPosition, enemy2Pos, changeSpeed * Time.deltaTime);
        enemy2.localPosition = Vector3.MoveTowards(enemy2.localPosition, enemy1Pos, changeSpeed * Time.deltaTime);

        //swap target positions if reached
        if (Vector3.Distance(enemy1.localPosition, enemy2Pos) < 0.1f)
        {
            var temp = enemy1;
            enemy1 = enemy2;
            enemy2 = temp;
        }
    }
}
