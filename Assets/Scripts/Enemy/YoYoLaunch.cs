using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoYoLaunch : MonoBehaviour
{
    public float launchSpeed;
    public float comebackSpeed;
    public float rotation;
    public Transform StartingPos;
    float comebackTimer;
    public float maxComebackTimer = 2;
    Rigidbody2D rb;
    public bool isMovable = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (StartingPos != null)
        {
            comebackTimer -= Time.deltaTime;
            if (comebackTimer < 0 && StartingPos.position != transform.position)
            {
                // move towards starting position
                if (isMovable)
                    rb.velocity = Vector2.MoveTowards(rb.velocity, comebackSpeed * new Vector2(StartingPos.position.x - transform.position.x, StartingPos.position.y - transform.position.y).normalized, comebackSpeed * Time.deltaTime);
                else
                    transform.position = Vector2.MoveTowards(transform.position, StartingPos.position, comebackSpeed * Time.deltaTime);
            }
            //stop if close to the position
            if (comebackTimer < 0 && Vector2.Distance(transform.position, StartingPos.position) < 0.2f)
            {
                rb.velocity = Vector2.zero;
                transform.position = StartingPos.position;
            }
        }
    }

    public void Launch()
    {
        if (Vector2.Distance(transform.position, StartingPos.position) < 0.1f)
        {
            comebackTimer = maxComebackTimer;
            //set velocity towards rotation variable +  global rotation
            rb.velocity += launchSpeed * new Vector2(Mathf.Cos((rotation + transform.rotation.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Sin((rotation + transform.rotation.eulerAngles.z) * Mathf.Deg2Rad));

            //rb.velocity = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad) * launchSpeed, Mathf.Sin(rotation * Mathf.Deg2Rad) * launchSpeed);

        }
    }
}