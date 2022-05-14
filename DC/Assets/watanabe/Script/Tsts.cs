using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsts : MonoBehaviour
{
    // Start is called before the first frame update
    public SE sE;
    void Start()
    {
       
        sE.moveSound();

    }

    // Update is called once per frame
    void Update()
    {
        GameObject se = GameObject.Find("seObject");
        se.GetComponent<SE>().moveSound();
    }
}
