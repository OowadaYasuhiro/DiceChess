using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonController : MonoBehaviour
{
    public float MovingDistance = 10;
    private float StartPos;

    void Start()
    {
        StartPos = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, StartPos + Mathf.PingPong(Time.time * 4f, MovingDistance), transform.position.z);
    }
}