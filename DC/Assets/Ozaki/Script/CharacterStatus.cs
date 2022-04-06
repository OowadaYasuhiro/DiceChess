using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    //必殺技ゲージ
    public int sp = 0;
    //キャラタイプ。これで必殺技内容を変える。 1=ティカ　2=リアン　3=ヴィオラ　4=ララ＆リリ＆ロロ
    public enum TYPE
    {
        NONE = -1,
        TIKA = 1,
        LIAN,
        VIOLA,
        LLL
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getSP(int i) {
        sp += i;
    }
}
