using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImage : MonoBehaviour
{
    [SerializeField]private GameObject[] _charaPickImage; //0~3は1Pの画像 4~7は2Pの画像
    [SerializeField]private GameObject[] _itemPickImage;  //0~3は1P一個目 4~7は1P二個目 8~11は2P一個目 12~15は2P二個目

    [SerializeField]private CursorManager _cursorManager;
    [SerializeField]private SelectController _selectController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeImage();
    }

    public void changeImage() {
        //CursorManagerのCharaSelectCountの数値を読み取って画像切り替えの指示を変更する
        if(_cursorManager.CharaSelectCount == 0) {
            changeChara(0, 1, 2, 3, _selectController.nowSelectChara);
        }
        else if(_cursorManager.CharaSelectCount == 1) {
            changeChara(4, 5, 6, 7, _selectController.nowSelectChara);
        }
        //CursorManagerのItemSelectCountの数値を読み取って画像切り替えの指示を変更する
        if(_cursorManager.ItemSelectCount == 0) {
            changeItem(0, 1, 2, 3, _selectController.nowSelectItem);
        }
        else if(_cursorManager.ItemSelectCount == 1) {
            changeItem(4, 5, 6, 7, _selectController.nowSelectItem);
        }
        else if(_cursorManager.ItemSelectCount == 2) {
            changeItem(8, 9, 10, 11, _selectController.nowSelectItem);
        }
        else if(_cursorManager.ItemSelectCount == 3) {
            changeItem(12, 13, 14, 15, _selectController.nowSelectItem);
        }
    }
    /// <summary>
    /// キャラ画像を変更する関数
    /// </summary>
    /// <param name="num1">配列1つ目</param>
    /// <param name="num2">配列2つ目</param>
    /// <param name="num3">配列3つ目</param>
    /// <param name="num4">配列4つ目</param>
    /// <param name="numPlayer">CursorManagerのどの数値を参照するか</param>
    public void changeChara(int num1, int num2, int num3, int num4, int numPlayer) {
        switch(numPlayer) {
            case 0:
                _charaPickImage[num1].SetActive(true);
                _charaPickImage[num2].SetActive(false);
                _charaPickImage[num3].SetActive(false);
                _charaPickImage[num4].SetActive(false);
                break;
            case 1:
                _charaPickImage[num1].SetActive(false);
                _charaPickImage[num2].SetActive(true);
                _charaPickImage[num3].SetActive(false);
                _charaPickImage[num4].SetActive(false);
                break;
            case 2:
                _charaPickImage[num1].SetActive(false);
                _charaPickImage[num2].SetActive(false);
                _charaPickImage[num3].SetActive(true);
                _charaPickImage[num4].SetActive(false);
                break;
            case 3:
                _charaPickImage[num1].SetActive(false);
                _charaPickImage[num2].SetActive(false);
                _charaPickImage[num3].SetActive(false);
                _charaPickImage[num4].SetActive(true);
                break;

        }

        Debug.Log("配列番号1"+num1);
        Debug.Log("配列番号2" + num2);
        Debug.Log("配列番号3" + num3);
        Debug.Log("配列番号4" + num4);
        Debug.Log("numPlayer" + numPlayer);
    }
    /// <summary>
    /// アイテム画像を変更する関数
    /// </summary>
    /// <param name="num1">配列1つ目</param>
    /// <param name="num2">配列2つ目</param>
    /// <param name="num3">配列3つ目</param>
    /// <param name="num4">配列4つ目</param>
    /// <param name="numPlayer">CursorManagerのどの数値を参照するか</param>
    public void changeItem(int num1, int num2, int num3, int num4, int numPlayer) {
        //キャンセルした時に他のピックアイテムが表示されないようにする
        if(_cursorManager.ItemSelectCount < 3) {
            _itemPickImage[num1 + 4].SetActive(false);
            _itemPickImage[num2 + 4].SetActive(false);
            _itemPickImage[num3 + 4].SetActive(false);
            _itemPickImage[num4 + 4].SetActive(false);
        }

        switch(numPlayer) {
            case 0:
                _itemPickImage[num1].SetActive(true);
                _itemPickImage[num2].SetActive(false);
                _itemPickImage[num3].SetActive(false);
                _itemPickImage[num4].SetActive(false);
                break;
            case 1:
                _itemPickImage[num1].SetActive(false);
                _itemPickImage[num2].SetActive(true);
                _itemPickImage[num3].SetActive(false);
                _itemPickImage[num4].SetActive(false);
                break;
            case 2:
                _itemPickImage[num1].SetActive(false);
                _itemPickImage[num2].SetActive(false);
                _itemPickImage[num3].SetActive(true);
                _itemPickImage[num4].SetActive(false);
                break;
            case 3:
                _itemPickImage[num1].SetActive(false);
                _itemPickImage[num2].SetActive(false);
                _itemPickImage[num3].SetActive(false);
                _itemPickImage[num4].SetActive(true);
                break;

        }

        
    }
}
