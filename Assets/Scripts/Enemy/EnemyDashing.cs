using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDashing : MonoBehaviour
{
    public float dashingPeriod = 1; //in seconds
    private float dashingTimer;
    private Vector2 posDifference;

    private EnemyStats enemyStats;
    private Rigidbody2D rb;
    public bool isEnabled = true;
    public float dashingSpeedMod = 1;
    public float dragMod = 1;
    public UnityEvent Dashed;
    void Start()
    {
        Setup();
    }
    void OnEnable()
    {
        Setup();
    }
    void Setup()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        SetDrag();
    }
    public void SetDrag()
    {
        rb.drag = 1f / dashingPeriod * enemyStats.speedMod * enemyStats.baseStats.movementSpeed * dragMod + 0.1f;
    }

    void Update()
    {
        dashingTimer += Time.deltaTime;
        if (dashingTimer > dashingPeriod && isEnabled)
        {
            Dash();
        }
    }
    public void Dash()
    {

        // calculate distance of side movements
        Vector3 upVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.up;
        Vector3 rightVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.right;
        Vector3 leftVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.left;
        Vector3 downVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector3.down;
        float upDiff = Vector3.Distance(transform.position + upVec, enemyStats.player.transform.position);
        float rightDiff = Vector3.Distance(transform.position + rightVec, enemyStats.player.transform.position);
        float leftDiff = Vector3.Distance(transform.position + leftVec, enemyStats.player.transform.position);
        float downDiff = Vector3.Distance(transform.position + downVec, enemyStats.player.transform.position);

        // choose closest move
        if (upDiff < rightDiff && upDiff < leftDiff && upDiff < downDiff)
            rb.velocity += enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(upVec.x, upVec.y) * dashingSpeedMod;
        else if (downDiff < leftDiff && downDiff < rightDiff)
            rb.velocity += enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(downVec.x, downVec.y) * dashingSpeedMod;
        else if (rightDiff < leftDiff)
            rb.velocity += enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(rightVec.x, rightVec.y) * dashingSpeedMod;
        else
            rb.velocity += enemyStats.speedMod * enemyStats.baseStats.movementSpeed * new Vector2(leftVec.x, leftVec.y) * dashingSpeedMod;

        dashingTimer = 0;
        Dashed.Invoke();

    }
}
