using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDice : MonoBehaviour
{
    public GameObject prefab;
    public float a;
    public float b;
    public float c;

    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            transform.position = new Vector3(a,b,c);
        }
    }
}