using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [SerializeField] private CursorManager _cursorManager;
    [SerializeField] private SelectController _selectController;
    [SerializeField] private EventSystem _eventSystem;
    

    [SerializeField] private Sprite[] _cursorImage;         //カーソルの画像1Pと2P分


    private Animator _anim;                                 //カーソルのアニメーター
    private bool _animEnd = false;

    private GameObject _pickItemCanvas;
    
    [SerializeField] private int _cursorNum;                //自分が1Pか2Pを判別するint型
    private Image _image;                                   //GetComponent<Image>
    [SerializeField] private float _blinkingSpeed = 0.5f;   //点滅速度
    private float _red;
    private float _blue;
    private float _green;
    private float _alpha;
    private bool _isFadeIn = false;                         //フェードイン開始bool    ※点滅用
    private bool _isFadeOut = false;                        //フェードアウト開始bool　※点滅用

    void Start() {
        _anim = this.gameObject.GetComponent<Animator>();
        _image = this.gameObject.GetComponent<Image>();
        _red = this._image.color.r;
        _blue = this._image.color.b;
        _green = this._image.color.g;
        _isFadeIn = true;

        _pickItemCanvas = GameObject.Find("PickItemCanvas");
    }

    private void Update()
    {
        playerSprite();

        if(_selectController.nowMode == 1) {
            charaCursorSet();
        }
        else if(_selectController.nowMode == 2) {
            //charaCursor();
            //StartCoroutine(positionCurosr(1));
            positionCurosr(1);
        } 
        else if(_selectController.nowMode == 3) {
            itemCursorSet();
        } 
        else if(_selectController.nowMode == 4) {
            //itemCursor();
            //StartCoroutine(positionCurosr(2));
            positionCurosr(2);
        }


        //点滅設定
        if(_isFadeIn) {
            blinkingCursorOff();
        } else if(_isFadeOut) {
            blinkingCursorOn();
        }
    }

    /// <summary>
    /// 1P用の画像と2P用の画像を変える
    /// </summary>
    public void playerSprite() {
        //モードがキャラセレ画面選択中だったら
        if(_selectController.nowMode == 2) {
            //キャラを選んでいなかったら
            if(_cursorManager.CharaSelectCount == 0) {
                _image.sprite = _cursorImage[0];
            } 
            else {
                _image.sprite = _cursorImage[1];
            }
        }
        //モードがアイテムセレ画面選択中だったら
        else if(_selectController.nowMode == 4) {
            //アイテムを選んでいない、もしくは一つ選択している場合
            if(_cursorManager.ItemSelectCount == 0 || _cursorManager.ItemSelectCount == 1) {
                _image.sprite = _cursorImage[0];
            } 
            else {
                _image.sprite = _cursorImage[1];
            }
        }
    }

    /// <summary>
    /// キャラセット
    /// </summary>
    public void charaCursorSet() {
        _pickItemCanvas.SetActive(false);
        _anim.Play("Chara_1");
        _anim.SetInteger("charaNum", 0);
    }

    /// <summary>
    /// アイテムセット
    /// </summary>
    public void itemCursorSet() {
        _pickItemCanvas.SetActive(true);
        _anim.Play("Item_1");
        _anim.SetInteger("itemNum", 0);
    }

    /*
    //カーソル選択処理
    public void charaCursor() {

        


        
    }

    //カーソル選択処理
    public void itemCursor() {
        switch(_eventSystem.currentSelectedGameObject.name) {
            case "Frame_01":
                //下入力したらカーソルをアイテム02へ
                if(0 < Input.GetAxis("Vertical")) {
                    
                    this._anim.SetBool("down", true);

                    this._anim.SetInteger("itemNum", 1);

                }
                //右入力したらカーソルをアイテム03へ
                else if(0 < Input.GetAxis("Horizontal")) {
                    
                    this._anim.SetBool("right", true);

                    this._anim.SetInteger("itemNum", 2);

                }

                break;
            case "Frame_02":
                //上入力したらカーソルをアイテム01へ
                if(Input.GetAxis("Vertical") < 0) {
                    
                    this._anim.SetBool("up", true);

                    this._anim.SetInteger("itemNum", 0);
                    
                }
                //右入力したらカーソルをアイテム03へ
                else if(0 < Input.GetAxis("Horizontal")) {
                    
                    this._anim.SetBool("right", true);

                    this._anim.SetInteger("itemNum", 3);

                }
                break;
            case "Frame_03":
                //下入力したらカーソルをアイテム04へ
                if(0 < Input.GetAxis("Vertical")) {
                    
                    this._anim.SetBool("down", true);

                    this._anim.SetInteger("itemNum", 3);


                }
                //左入力したらカーソルをアイテム01へ
                else if(Input.GetAxis("Horizontal") < 0) {
                    
                    this._anim.SetBool("left", true);

                    this._anim.SetInteger("itemNum", 0);

                }
                break;
            case "Frame_04":
                //上入力したらカーソルをアイテム03へ
                if(Input.GetAxis("Vertical") < 0) {
                    
                    this._anim.SetBool("up", true);

                    this._anim.SetInteger("itemNum", 2);

                }
                //左入力したらカーソルをアイテム02へ
                else if(Input.GetAxis("Horizontal") < 0) {
                    
                    this._anim.SetBool("left", true);

                    this._anim.SetInteger("itemNum", 1);

                }
                break;
        }


        
    }
    */
    
    /*
    //自分のポジション
    public IEnumerator positionCurosr(int now) {
        if(now == 1) {
            if(_selectController.nowSelectChara == 0) {
                //右入力
                if(0 < Input.GetAxis("Horizontal")) {
                    _anim.SetBool("right", true);

                    yield return StartCoroutine(animEndStay());

                }
            }
            else if(_selectController.nowSelectChara == 1 || _selectController.nowSelectChara == 2) {
                //右入力
                if(0 < Input.GetAxis("Horizontal")) {
                    _anim.SetBool("right", true);

                    yield return StartCoroutine(animEndStay());
                }
                //左入力
                else if(Input.GetAxis("Horizontal") < 0) {
                    _anim.SetBool("left", true);

                    yield return StartCoroutine(animEndStay());

                }
            }
            else if(_selectController.nowSelectChara == 3) {
                //左入力
                if(Input.GetAxis("Horizontal") < 0) {
                    _anim.SetBool("left", true);

                    yield return StartCoroutine(animEndStay());

                }
            }


        }
        else if(now == 2)
        {
            if(_selectController.nowSelectItem == 0) {
                //右入力
                if(0 < Input.GetAxis("Horizontal")) {
                    _anim.SetTrigger("rightDir");
                }
                //下入力
                else if(0 < Input.GetAxis("Vertical")) {
                    _anim.SetTrigger("downDir");

                }
            }
            else if(_selectController.nowSelectItem == 1) {
                //右入力
                if(0 < Input.GetAxis("Horizontal")) {
                    _anim.SetTrigger("rightDir");
                }
                //上入力
                else if(Input.GetAxis("Vertical") < 0) {
                    _anim.SetTrigger("upDir");
                }
            } 
            else if(_selectController.nowSelectItem == 2) {
                //左入力
                if(Input.GetAxis("Horizontal") < 0) {
                    _anim.SetTrigger("leftDir");

                }
                //下入力
                else if(0 < Input.GetAxis("Vertical")) {
                    _anim.SetTrigger("downDir");

                }
            } 
            else if(_selectController.nowSelectItem == 3) {
                //左入力
                if(Input.GetAxis("Horizontal") < 0) {
                    _anim.SetTrigger("leftDir");

                }
                //上入力
                else if(Input.GetAxis("Vertical") < 0) {
                    _anim.SetTrigger("upDir");
                }
            }
            

        }
        
    }
    */

    /// <summary>
    /// カーソルを選択中の場所にそろえる
    /// </summary>
    /// <param name="now">1 = キャラセレ画面　2 = アイテムセレ画面 </param>
    public void positionCurosr(int now) {
        if(now == 1) {
            switch(_selectController.nowSelectChara) {
                case 0:
                    _anim.Play("Chara_1");
                    break;
                case 1:
                    _anim.Play("Chara_2");
                    break;
                case 2:
                    _anim.Play("Chara_3");
                    break;
                case 3:
                    _anim.Play("Chara_4");
                    break;
            }
        }
        else if(now == 2) {
            switch(_selectController.nowSelectItem) {
                case 0:
                    _anim.Play("Item_1");
                    break;
                case 1:
                    _anim.Play("Item_2");
                    break;
                case 2:
                    _anim.Play("Item_3");
                    break;
                case 3:
                    _anim.Play("Item_4");
                    break;
            }
        }
    }

    //カーソルアニメ待機
    public IEnumerator animEndStay() {
        //_animEndがtrueになったら次の処理へ
        yield return new WaitUntil(() => _animEnd == true);
        Debug.Log("animEnd falseです");
        _animEnd = false;
        _anim.SetBool("right", false);
        _anim.SetBool("left", false);
        _anim.SetBool("up", false);
        _anim.SetBool("down", false);

        yield break;
    }

    public void animEnd() {
        _animEnd = true;
    }

    //点滅設定
    public void blinkingCursorOn()
    {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha >= 1.0f)
        {
            _isFadeOut = false;
            _alpha = 1.0f;
            _isFadeIn = true;

        }
    }
    public void blinkingCursorOff()
    {
        _alpha -= Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha <= 0.5f)
        {
            _isFadeIn = false;
            _alpha = 0.5f;

            _isFadeOut = true;

        }
    }
    public void blickingCursorEnd()
    {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha >= 1.0f)
        {
            _isFadeOut = false;
            _isFadeIn = false;
            _alpha = 1.0f;

        }
    }

    //カーソル位置ずらし
    public void cursorSameOn()
    {
        if (_cursorNum == 1)
        {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_01").transform;
        }
        if (_cursorNum == 2)
        {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_02").transform;
        }
    }
    public void cursorSameOff()
    {
        //親子関係削除
        this.gameObject.transform.parent = GameObject.Find("CursorCanvas").transform;
    }
}
