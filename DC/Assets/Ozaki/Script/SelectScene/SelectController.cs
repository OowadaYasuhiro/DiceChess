using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;

    [SerializeField] private GameObject _charaSelectCanvas;
    [SerializeField] private GameObject _itemSelsectCanvas;
    [SerializeField] private GameObject _sc;
    [SerializeField] private EventSystem _eventSystem;

    [SerializeField] private MessageController _messageCon;

    [SerializeField] private GameObject[] _charaFrame;
    [SerializeField] private GameObject[] _itemFrame;
    [SerializeField] private Button[] _itemButton;
    [SerializeField] private BGM _sound;
    

    private int _nowSelectCharaCursor;
    private int _nowSelectItemCursor;
    private int _lastItemSelect;

    private bool _messageFlag = false;

    public enum MODE
    {
        none = -1,
        CharaSet = 1,
        CharaSelect,
        ItemSet,
        ItemSelect,
        NextScene
    }
    MODE _nowMode;
    //private int _nowModeNum;

    private void Start()
    {
        _messageFlag = false;
        _nowMode = MODE.CharaSet;
    }

    private void Update()
    {
        if(_nowMode == MODE.CharaSet)
        {
            CharaSet();
        }
        else if(_nowMode == MODE.CharaSelect)
        {
            
            if (cursorManager.CharaSelectCount == 2)
            {
                CharaNextMode();
            }
            else
            {
                CharaSelect();
            }
            

        }
        else if(_nowMode == MODE.ItemSet)
        {
            ItemSet();

        }
        else if(_nowMode == MODE.ItemSelect)
        {
            
            
            
            if (cursorManager.ItemSelectCount == 4)
            {
                _lastItemSelect = _nowSelectItemCursor;
                cursorManager.getI();
                _eventSystem.enabled = false;
                Invoke("ItemNextMode",0.5f);
            }
            else
            {
                ItemSelect();
            }
        }
        else if(_nowMode == MODE.NextScene)
        {
            NextScene("LoadScene");
        }
    }

    //キャラクターセレクト画面準備
    public void CharaSet()
    {
        _charaSelectCanvas.SetActive(true);
        _itemSelsectCanvas.SetActive(false);

        //_charaFrame[0].Select();
        _eventSystem.SetSelectedGameObject(_charaFrame[0]);

        _nowMode = MODE.CharaSelect;
    }

    //キャラ選択中
    public void CharaSelect()
    {
        nowSelectCursor(1);

        if(Input.GetButtonDown("Y1") && _messageFlag == false) {
            Debug.Log("Yボタン押された");
            _messageFlag = true;
            
            OpenMessage();
        }

        if(Input.GetButtonDown("Cancel") && _messageFlag == false){

            cursorManager.CharaCnacel("Title");
        }
        
    }

    public void OpenMessage() {
        _messageCon.setMessage();


    }

    public void CharaNextMode()
    {
        _nowMode = MODE.ItemSet;
    }
    

    //アイテムセレクト画面準備
    public void ItemSet()
    {
        _charaSelectCanvas.SetActive(false);
        _itemSelsectCanvas.SetActive(true);

        //_itemFrame[0].Select();
        _eventSystem.SetSelectedGameObject(_itemFrame[0]);
        cursorManager.ItemSelectCount = 0;

        _nowMode = MODE.ItemSelect;
    }

    //アイテム選択中
    public void ItemSelect()
    {
        nowSelectCursor(2);

        if (Input.GetButtonDown("Cancel") && _messageFlag == false)
        {

            cursorManager.ItemCnacel();
            if(cursorManager.ItemSelectCount == -1)
            {
                cursorManager.ItemCnacel_count0();
                _nowMode = MODE.CharaSet;
            }
        }

        if(Input.GetButtonDown("Y1")) {
            _messageFlag = true;
            OpenMessage();
        }
    }
    public void ItemNextMode()
    {
        
        _nowMode = MODE.NextScene;
    }

    public void AreYouReady() {
        _sc.SetActive(true);
        var sb = GameObject.Find("SortieButton").GetComponent<Button>();
        sb.Select();
        Ready();
    }

    public void Ready() {
        if(Input.GetButtonDown("Cancel")) {
            cursorManager.ItemCnacel();
            _sc.SetActive(false);
            _itemButton[0].Select();
            


            _nowMode = MODE.ItemSelect;
        }
    }

    /// <summary>
    /// 次のシーンへ
    /// </summary>
    /// <param name="name">Loadしたいシーンの名前</param>
    public void NextScene(string name)
    {
        
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// EventSystemのカーソルセレクトで選択さてているobjectの名前で情報送る
    /// </summary>
    /// <param name="now">now = 1 はキャラセレ 2はアイテム</param>
    public void nowSelectCursor(int now) {
        if(now == 1) {
            switch(_eventSystem.currentSelectedGameObject.name) {
                case "Frame_01":
                    _nowSelectCharaCursor = 0;

                    break;
                case "Frame_02":
                    _nowSelectCharaCursor = 1;
                    break;
                case "Frame_03":
                    _nowSelectCharaCursor = 2;
                    break;
                case "Frame_04":
                    _nowSelectCharaCursor = 3;
                    break;
            }
        } else {
            switch(_eventSystem.currentSelectedGameObject.name) {
                case "Frame_01":
                    _nowSelectItemCursor = 0;

                    break;
                case "Frame_02":
                    _nowSelectItemCursor = 1;
                    break;
                case "Frame_03":
                    _nowSelectItemCursor = 2;
                    break;
                case "Frame_04":
                    _nowSelectItemCursor = 3;
                    break;
                
            }
        }

    }

    public int nowMode {
        get { return (int)_nowMode; }
    }

    public int nowSelectChara {
        get { return _nowSelectCharaCursor;}
    }

    public int nowSelectItem {
        get { return _nowSelectItemCursor;}
    }

    public bool messageFlag {
        get { return _messageFlag; }
        set { _messageFlag = value; }
    }
}
