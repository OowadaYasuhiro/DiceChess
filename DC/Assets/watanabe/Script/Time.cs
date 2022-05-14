using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time : MonoBehaviour
{
    yield return new WaitForSeconds(秒数);

    //コルーチンの実行
    StartCoroutine("Hoge");
}

private IEnumarator Hoge()
{

    text.text = "おはようございます";

    //1秒待つ
    yield return new WaitForSeconds(1.0f);

    //1秒待った後の処理
    text.text = "こんにちは";

    //1秒待つ
    yield return new WaitForSeconds(1.0f);

    //1秒待った後の処理
    text.text = "こんばんは";
}
}
