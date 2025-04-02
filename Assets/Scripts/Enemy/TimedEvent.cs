using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    public float maxEventTimer = 2;
    float eventTimer;
    public UnityEvent timedEvent;
    private void Start()
    {
        eventTimer = maxEventTimer;
    }
    void Update()
    {
        eventTimer -= Time.deltaTime;
        if (eventTimer < 0)
        {
            timedEvent.Invoke();
            eventTimer = maxEventTimer;
        }
    }
}
