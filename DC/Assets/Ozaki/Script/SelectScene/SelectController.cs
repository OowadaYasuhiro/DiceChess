using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    //0=指定なし(テスト用キャラ) 1=ティカ 2=リアン 3=ヴィオラ 4=三姉妹　
    private int _charaNum1;
    private int _charaNum2;
    //0=指定なし 1= 2= 3= 4=
    private int _itemNum;

    private GameObject charaGroup;
    [SerializeField] private GameObject[] pickChara;
    [SerializeField] private GameObject[] selectCharaImage;
    [SerializeField] private GameObject[] blackCover;



    private CursorController cursorCon_01;
    private CursorController cursorCon_02;

    private bool _cursorSame = false;

    private EventSystem ev;

    public enum MODE
    {
        NONE = -1,
        SETCHARACTOR = 1,
        CHARASELECT,
        SETITEM,
        ITEMSELECT,
        NEXTS_SCENE
    }

    MODE _mode, _nowMode;
    private int _modeNum;

    // Start is called before the first frame update
    void Start()
    {
        cursorCon_01 = GameObject.Find("CharaCursor_1P").GetComponent<CursorController>();
        cursorCon_02 = GameObject.Find("CharaCursor_2P").GetComponent<CursorController>();

        _mode = MODE.CHARASELECT;
    }

    // Update is called once per frame
    void Update()
    {
        if(_mode == MODE.SETCHARACTOR) {
            _modeNum = (int)_mode;

        }
        else if(_mode == MODE.CHARASELECT) {
            _modeNum = (int)_mode;
            //ピックキャラ画像変更
            charaPickImage_01();
            charaPickImage_02();

            //キャラセレ枠切り替え
            charaSelectSwith();
            //両者キャラ決定時
            charaDecision();
        }
        else if(_mode == MODE.SETITEM) {
            _modeNum = (int)_mode;

        }
        else if(_mode == MODE.ITEMSELECT) {
            _modeNum = (int)_mode;

        }
        else if(_mode == MODE.NEXTS_SCENE) {

        }

        cursorSumeJudge();
    }

    //プレイヤー1のピック画像を変更する
    public void charaPickImage_01() {
        switch(cursorCon_01.focusCharaNum) {
            case 0:
                _charaNum1 = cursorCon_01.focusCharaNum;
                //ピックキャラ画像OnOff切り替え
                pickChara[0].SetActive(true);
                pickChara[1].SetActive(false);
                pickChara[2].SetActive(false);
                pickChara[3].SetActive(false); 
                
                break;
            case 1:
                _charaNum1 = cursorCon_01.focusCharaNum;
                //ピックキャラ画像OnOff切り替え
                pickChara[0].SetActive(false);
                pickChara[1].SetActive(true);
                pickChara[2].SetActive(false);
                pickChara[3].SetActive(false);
                break;
            case 2:
                _charaNum1 = cursorCon_01.focusCharaNum;
                //ピックキャラ画像OnOff切り替え
                pickChara[0].SetActive(false);
                pickChara[1].SetActive(false);
                pickChara[2].SetActive(true);
                pickChara[3].SetActive(false);
                break;
            case 3:
                _charaNum1 = cursorCon_01.focusCharaNum;
                //ピックキャラ画像OnOff切り替え
                pickChara[0].SetActive(false);
                pickChara[1].SetActive(false);
                pickChara[2].SetActive(false);
                pickChara[3].SetActive(true);
                break;

        }
    }
    //プレイヤー2のピック画像を変更
    public void charaPickImage_02() {
        switch(cursorCon_02.focusCharaNum) {
            case 0:
                _charaNum2 = cursorCon_02.focusCharaNum;

                pickChara[4].SetActive(true);
                pickChara[5].SetActive(false);
                pickChara[6].SetActive(false);
                pickChara[7].SetActive(false);
                break;
            case 1:
                _charaNum2 = cursorCon_02.focusCharaNum;

                pickChara[4].SetActive(false);
                pickChara[5].SetActive(true);
                pickChara[6].SetActive(false);
                pickChara[7].SetActive(false);
                break;
            case 2:
                _charaNum2 = cursorCon_02.focusCharaNum;

                pickChara[4].SetActive(false);
                pickChara[5].SetActive(false);
                pickChara[6].SetActive(true);
                pickChara[7].SetActive(false);
                break;
            case 3:
                _charaNum2 = cursorCon_02.focusCharaNum;

                pickChara[4].SetActive(false);
                pickChara[5].SetActive(false);
                pickChara[6].SetActive(false);
                pickChara[7].SetActive(true);
                break;

        }
    }
    //キャラセレクト画像のOnOffを切り替える
    public void charaSelectSwith() {
        if(cursorCon_01.focusCharaNum == 0 || cursorCon_02.focusCharaNum == 0) {
            selectCharaImage[0].SetActive(true);
            blackCover[0].SetActive(false);
        } else {
            selectCharaImage[0].SetActive(false);
            blackCover[0].SetActive(true);
        }

        if(cursorCon_01.focusCharaNum == 1 || cursorCon_02.focusCharaNum == 1) {
            selectCharaImage[1].SetActive(true);
            blackCover[1].SetActive(false);
        } else {
            selectCharaImage[1].SetActive(false);
            blackCover[1].SetActive(true);
        }

        if(cursorCon_01.focusCharaNum == 2 || cursorCon_02.focusCharaNum == 2) {
            selectCharaImage[2].SetActive(true);
            blackCover[2].SetActive(false);
        } else {
            selectCharaImage[2].SetActive(false);
            blackCover[2].SetActive(true);
        }


        if(cursorCon_01.focusCharaNum == 3 || cursorCon_02.focusCharaNum == 3) {
            selectCharaImage[3].SetActive(true);
            blackCover[3].SetActive(false);
        } else {
            selectCharaImage[3].SetActive(false);
            blackCover[3].SetActive(true);
        }

    }

    public void cursorSumeJudge() {
        if(cursorCon_01.focusCharaNum == cursorCon_02.focusCharaNum) {
            _cursorSame = true;
        } else {
            _cursorSame = false;
        }
    }

    //キャラ決定したら
    private void charaDecision() {
        if(cursorCon_01.charaDecision && cursorCon_02.charaDecision) {
            _mode = MODE.SETITEM;
        }
    }





    public bool cursorSame {
        get { return _cursorSame;}
    }

    public int modeNum {
        get { return _modeNum;}
    }

    
    
}
