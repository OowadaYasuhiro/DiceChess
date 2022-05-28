using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    //必殺技ゲージ
    [SerializeField]private int _sp = 0;

    private Image _spPlayer1Image;
    private Image _spPlayer2Image;

    // Start is called before the first frame update
    void Start()
    {
        _spPlayer1Image = GameObject.Find("Player1SpGauge").GetComponent<Image>();
        _spPlayer2Image = GameObject.Find("Player2SpGauge").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void setSP(int i) {
        _sp += i;
        //必殺技ゲージが100以上になったら余分なポイントを減らす
        if(100 < _sp) {
            _sp -= _sp - 100;
        }
        if(0 > _sp) {
            _sp = 0;
        }
    }

    public int Sp {
        get { return _sp; }
    }

    public void setPlayer1SpBar() {
        _spPlayer1Image.fillAmount = _sp / 100f;
    }

    public void setPlayer2SpBar() {
        _spPlayer2Image.fillAmount = _sp / 100f;
    }
   
}
