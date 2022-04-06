using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    //必殺技ゲージ
    public int sp = 0;


    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void setSP(int i) {
        sp += i;
        //必殺技ゲージが100以上になったら余分なポイントを減らす
        if(100 < sp) {
            sp -= sp - 100;
        }
    }

    
   
}
