using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    private int _charaSelectCount = 0;
    private int _chara1Num;
    private int _chara2Num;
    private int _itemSelectCount = 0;
    private int _item1_1Num;
    private int _item1_2Num;
    private int _item2_1Num;
    private int _item2_2Num;

    public void Update()
    {
        Debug.Log("キャラは現在"+_charaSelectCount);
        Debug.Log("アイテムは現在" + _itemSelectCount);
    }

    //キャラ決定
    public void CharaSelect(int num)
    {
        if(_charaSelectCount == 0)
        {
            _charaSelectCount ++;
            _chara1Num = num;
            Debug.Log("1Pキャラは" + _chara1Num);
        }
        else if(_charaSelectCount == 1){
            _charaSelectCount ++;
            _chara2Num = num;
            Debug.Log("2Pキャラは" + _chara2Num);
        }

    } 

    //キャラキャンセル
    public void CharaCnacel(string sceneName)
    {
        if (_charaSelectCount == 0)
        {
            SceneManager.LoadScene(sceneName);
        }
        
        else if (_charaSelectCount == 1)
        {
            _charaSelectCount --;
            _chara1Num = 0;
        }
    }

    //アイテム選択
    public void ItemSelect(int num)
    {
        if (_itemSelectCount == 0)
        {
            _itemSelectCount++;
            _item1_1Num = num;

            Debug.Log("1Pアイテム1個目は" + _item1_1Num);
        }
        else if (_itemSelectCount == 1)
        {
            _itemSelectCount++;
            _item1_2Num = num;

            Debug.Log("1Pアイテム2個目は" + _item1_2Num);
        }
        else if (_itemSelectCount == 2)
        {
            _itemSelectCount++;
            _item2_1Num = num;

            Debug.Log("2Pアイテム1個目は" + _item2_1Num);
        }
        else if (_itemSelectCount == 3)
        {
            _itemSelectCount++;
            _item2_2Num = num;

            Debug.Log("2Pアイテム2個目は" + _item2_2Num);
        }

    }

    //アイテムキャンセル
    public void ItemCnacel()
    {
        if (_itemSelectCount == 0)
        {
            _charaSelectCount--;
            _chara2Num = 0;
        }
        else if (_itemSelectCount == 1)
        {
            _itemSelectCount--;
            _item1_1Num = 0;
        }
        else if (_itemSelectCount == 2)
        {
            _itemSelectCount--;
            _item1_2Num = 0;
        }
        else if (_itemSelectCount == 3)
        {
            _itemSelectCount--;
            _item2_1Num = 0;
        }
        else if (_itemSelectCount == 4)
        {
            _itemSelectCount--;
            _item2_2Num = 0;
        }
    }

    public void getI()
    {
        DontDestroySingleObject.p1Character = _chara1Num;
        DontDestroySingleObject.p1Character = _chara2Num;
        DontDestroySingleObject.p1Character = _item1_1Num;
        DontDestroySingleObject.p1Character = _item1_2Num;
        DontDestroySingleObject.p1Character = _item2_1Num;
        DontDestroySingleObject.p1Character = _item2_2Num;
    }

    public int CharaSelectCount {
        get { return _charaSelectCount; }
        set { _charaSelectCount = value; }
    }
    public int ItemSelectCount {
        get { return _itemSelectCount; }
        set { _itemSelectCount = value; }
    }
}
