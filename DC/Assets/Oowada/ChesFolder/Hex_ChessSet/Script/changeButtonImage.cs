using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeButtonImage : MonoBehaviour
{
    // 画像を動的に変えたいボタンの宣言
    Image btnImage;

    // inspectorで直接画像のスプライトを張り付ける
    public Sprite Asprite;
    public Sprite Bsprite;

    //フラグ
    private bool flg = true;

    void Start()
    {
        // Imageを所得
        btnImage = this.GetComponent<Image>();
    }

    void Update()
    {
        // フラグによってそれに合った画像に差し替える
        if (flg == true)
        {
            btnImage.sprite = Asprite;
        }
        else if (flg == false)
        {
            btnImage.sprite = Bsprite;
        }
    }

    public void changeFlg()
    {
        flg = false;
        Invoke("flgTrue",2.0f);
    }
    void flgTrue()
    {
        flg = true;
    }
}
