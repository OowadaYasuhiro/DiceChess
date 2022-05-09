using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    public float CircleR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // ローカル座標基準で、現在の回転量へ加算する
        myTransform.Rotate(0, 0, CircleR * Time.deltaTime);
    }
}
