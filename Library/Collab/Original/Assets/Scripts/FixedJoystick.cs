using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : MonoBehaviour
{
    public bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;
    public Vector2 direction;
    public bool resetDirection = false;

    public Transform circle;
    public Transform outerCircle;

    //input borders
    private float leftBorder;
    private float rightBorder;
    public bool leftSide = false;

    // touching finger id
    public int finger = -1;
    private void Start()
    {
        if (leftSide)
        {
            leftBorder = -1;
            rightBorder = Screen.width / 2;
        }
        else
        {
            leftBorder = Screen.width / 2;
            rightBorder = Screen.width + 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            // reset joystick if no fingers touching 
            touchStart = false;
            finger = -1;
            circle.transform.position = transform.position;
            if (resetDirection) direction = new Vector2(0, 0);
        }
        else
        {
            if (finger == -1)
            {
                // check if there is new finger
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch newestTouch = Input.GetTouch(i);
                    if (newestTouch.phase == TouchPhase.Began)
                    {
                        // set to current finger if found
                        if (leftBorder <= newestTouch.position.x && rightBorder > newestTouch.position.x)
                        {
                            finger = newestTouch.fingerId;
                            touchStart = true;
                        }
                    }
                }
            }
            else if (finger != -1)
            {
                // check if finger is still touching
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).fingerId == finger)
                    {
                        pointA = transform.position;
                        pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, Camera.main.transform.position.z));
                        // return if still touching
                        return;
                    }
                }
                // reset joystick if finger is no longer touching
                touchStart = false;
                finger = -1;
                circle.transform.position = transform.position;
                if (resetDirection) direction = new Vector2(0, 0);
            }
        }

    }
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            direction = Vector2.ClampMagnitude(offset, 1.0f);

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);
        }

    }
}
