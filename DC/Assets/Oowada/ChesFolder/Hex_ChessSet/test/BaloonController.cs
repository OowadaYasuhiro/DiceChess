using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaloonController : MonoBehaviour
{
    public float MovingDistance = 20;
    private float StartPos;
    RectTransform RT; 

    void Start()
    {
        StartPos = transform.position.y;
        RT = GetComponent<RectTransform>();
    }

    void Update()
    {
        //RT.anchoredPosition = new Vector2(0 , StartPos + 240 + Mathf.PingPong(Time.time * 30f, MovingDistance));
        transform.localPosition = new Vector3(transform.position.x, StartPos + Mathf.PingPong(Time.time * 4f, MovingDistance), transform.position.z);
    }
}