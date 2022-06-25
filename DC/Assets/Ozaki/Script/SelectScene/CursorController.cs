using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    private SelectController selectController;

    //選択しているキャラ番号を送る用 0～3
    private int _focusCharaNum;
    //選択しているアイテム番号を送る用 0～3
    private int _focusItemNum;

    //このカーソルが1Pか2Pか判別する用 1=1P 2=2P
    [SerializeField] private int _cursorNum = 0;
    private string selectCharaObj;

    //アニメーション終了判定
    private bool _animEnd = false;
    private bool _corsorStay = false;

    private Animator anim;
    private Image _image;
    [SerializeField] private float _blinkingSpeed = 0.5f;
    private float _red;
    private float _blue;
    private float _green;
    private float _alpha;
    private bool _isFadeIn = false;
    private bool _isFadeOut = false;

    //決定した時の
    private bool _charaDecision = false;

    // Start is called before the first frame update
    void Start()
    {
        selectController = GameObject.Find("SceneDirector").GetComponent<SelectController>();
        anim = this.gameObject.GetComponent<Animator>();
        _image = this.gameObject.GetComponent<Image>();
        _red = this._image.color.r;
        _blue = this._image.color.b;
        _green = this._image.color.g;
        _isFadeIn = true;

        if(_cursorNum == 1) {
            this.gameObject.transform.position = new Vector3(-372, 352, 0);
            this._focusCharaNum = 0;
            this.anim.SetInteger("charaNum", 0);

        }
        if(_cursorNum == 2) {
            this.gameObject.transform.position = new Vector3(372, 328, 0);
            this._focusCharaNum = 3;
            this.anim.SetInteger("charaNum", 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(selectController.modeNum == 1) {

        }
        else if(selectController.modeNum == 2) {
            //キャラ決定していない時の行動
            if(!this._charaDecision) {
                //カーソル待機boolがfalseだったら
                if(this._corsorStay == false) {
                    //カーソル番号を読み取って移動と決定を行えるようにする
                    if(_cursorNum == 1) {
                        StartCoroutine(charaCursor("L_Stick_H"));
                        StartCoroutine(decision("Decision_1"));
                    }
                    if(_cursorNum == 2) {
                        StartCoroutine(charaCursor("L_Stick_H_2"));
                        StartCoroutine(decision("Decision_2"));
                    }
                }
                //点滅設定
                if(_isFadeIn) {
                    blinkingCursorOff();
                } else if(_isFadeOut) {
                    blinkingCursorOn();
                }
            } else {
                //点滅終了
                blickingCursorEnd();
                //カーソル番号を読み取ってキャンセル押すか確認する
                if(_cursorNum == 1) {
                    StartCoroutine(cancel("Cancel_1"));
                }
                if(_cursorNum == 2) {
                    StartCoroutine(cancel("Cancel_2"));
                }
            }
        }
        else if(selectController.modeNum == 3) {

        }
        else if(selectController.modeNum == 4) {

        }
        else if(selectController.modeNum == 5) {

        }
        

         //cursor1の時に使う
        if(selectController.cursorSame) {
            cursorSameOn();
        } else {
            cursorSameOff();
        }
        
    }

    //カーソル選択処理
    public IEnumerator charaCursor(string axisName) {
        selectCharaObj = transform.parent.name;

        switch(this._focusCharaNum) {
            case 0:
                //右入力したらカーソルをキャラ_02(リアン)へ
                if(0 < Input.GetAxis(axisName)) {
                    this._corsorStay = true;
                    this.anim.SetBool("right",true);
                    
                    yield return null;

                    this._focusCharaNum = 1;
                    this.anim.SetInteger("charaNum", 1);

                    yield return StartCoroutine(animEndStay());
                    

                } 
                
                break;
            case 1:
                //右入力したらカーソルをキャラ_03(ヴィオラ)へ
                if(0 < Input.GetAxis(axisName)) {
                    this._corsorStay = true;
                    this.anim.SetBool("right", true);

                    yield return null;

                    this._focusCharaNum = 2;
                    this.anim.SetInteger("charaNum", 2);

                    yield return StartCoroutine(animEndStay());
                    
                }
                //左入力したらカーソルをキャラ_01(ティカ)へ
                else if(Input.GetAxis(axisName) < 0) {
                    this._corsorStay = true;
                    this.anim.SetBool("left", true);

                    yield return null;

                    this._focusCharaNum = 0;
                    this.anim.SetInteger("charaNum", 0);

                    yield return StartCoroutine(animEndStay());
                    
                }
                break;
            case 2:
                //右入力したらカーソルをキャラ_04(三姉妹)へ
                if(0 < Input.GetAxis(axisName)) {
                    this._corsorStay = true;
                    this.anim.SetBool("right", true);

                    yield return null;

                    this._focusCharaNum = 3;
                    this.anim.SetInteger("charaNum", 3);

                    yield return StartCoroutine(animEndStay());
                    
                }
                //左入力したらカーソルをキャラ_02(リアン)へ
                else if(Input.GetAxis(axisName) < 0) {
                    this._corsorStay = true;
                    this.anim.SetBool("left", true);

                    yield return null;

                    this._focusCharaNum = 1;
                    this.anim.SetInteger("charaNum", 1);

                    yield return StartCoroutine(animEndStay());
                    
                }
                break;
            case 3:
                //左入力したらカーソルをキャラ_03(ヴィオラ)へ
                if(Input.GetAxis(axisName) < 0) {
                    this._corsorStay = true;
                    this.anim.SetBool("left", true);

                    yield return null;

                    this._focusCharaNum = 2;
                    this.anim.SetInteger("charaNum", 2);

                    yield return StartCoroutine(animEndStay());
                    
                }
                break;
        }

        
        yield break;
    }
    //カーソルアニメ待機
    public IEnumerator animEndStay() {
        //_animEndがtrueになったら次の処理へ
        yield return new WaitUntil(() => this._animEnd == true);
        Debug.Log("animEnd falseです");
        this._animEnd = false;
        this.anim.SetBool("right", false);
        this.anim.SetBool("left", false);
        this._corsorStay = false;

        yield break;
    }
    //決定ボタン押した時の処理
    public IEnumerator decision(string decName) {
        if(Input.GetButtonDown(decName)) {
            _charaDecision = true;
        }
        yield break;
    }
    //キャンセルボタンを押したときの処理
    public IEnumerator cancel(string canName) {
        if(Input.GetButtonDown(canName)) {
            _charaDecision = false;
            _isFadeIn = true;
        }
        yield break;
    }

    //点滅設定
    public void blinkingCursorOn() {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if(_alpha >= 1.0f) {
            _isFadeOut = false;
            _alpha = 1.0f;
            _isFadeIn = true;

        }
    }
    public void blinkingCursorOff() {
        _alpha -= Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if(_alpha <= 0.5f) {
            _isFadeIn = false;
            //_alpha = 0.05f; //cursorAnime ならこっち
            _alpha = 0.5f;  //cursorAnime 2ならこっち
            
            _isFadeOut = true;

        }
    }
    public void blickingCursorEnd() {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if(_alpha >= 1.0f) {
            _isFadeOut = false;
            _isFadeIn = false;
            _alpha = 1.0f;

        }
    }

    //カーソル位置ずらし
    public void cursorSameOn() {
        if(_cursorNum == 1) {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_01").transform;
        }
        if(_cursorNum == 2) {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_02").transform;
        }
    }
    public void cursorSameOff() {
        //親子関係削除
        this.gameObject.transform.parent = GameObject.Find("CursorCanvas").transform; ;
    }
    
    //SelectControllerでキャラピック画像変える用
    public int focusCharaNum {
        get { return _focusCharaNum;}
        set { _focusCharaNum = value;}
    }

    public int cursorNum {
        get { return _cursorNum;}
    }

    public void animEnd() {
        this._animEnd = true;
    }

    public bool charaDecision {
        get { return this._charaDecision; }
    }

}
