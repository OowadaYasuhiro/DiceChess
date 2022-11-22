using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class a : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject Button1;
    [SerializeField] GameObject Button2;
    [SerializeField] GameObject Piece1;
    [SerializeField] GameObject Piece2;

    GameObject selectedObj;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroySingleObject.winner = 0;
        DontDestroySingleObject.p1Character =0;//プレイヤー１のキャラクター 
        DontDestroySingleObject.p2Character = 0;//プレイヤー２のキャラクター 
        DontDestroySingleObject.p1item1 = 0;//プレイヤー１のアイテム１
        DontDestroySingleObject.p1item2 = 0;//プレイヤー１のアイテム２
        DontDestroySingleObject.p2item1 = 0;//プレイヤー２のアイテム１
        DontDestroySingleObject.p2item2 = 0;//プレイヤー２のアイテム２
        DontDestroySingleObject.p1Point = 0;//プレイヤー１のポイント
        DontDestroySingleObject.p2Point = 0;//プレイヤー２のポイント
        DontDestroySingleObject.p1TakePawn = 0;//取ったポーン
        DontDestroySingleObject.p2TakePawn = 0;
        DontDestroySingleObject.p1TakeRook = 0;//取ったルーク
        DontDestroySingleObject.p2TakeRook = 0;
        DontDestroySingleObject.p1TakeKnight = 0;//取ったナイト
        DontDestroySingleObject.p2TakeKnight = 0;
        DontDestroySingleObject.p1TakeBishop = 0;//取ったビショップ
        DontDestroySingleObject.p2TakeBishop = 0;
        DontDestroySingleObject.p1TakeQueen = 0;//取ったクイーン
        DontDestroySingleObject.p2TakeQueen = 0;
        DontDestroySingleObject.p1TakeKing = 0;//取ったキング
        DontDestroySingleObject.p2TakeKing = 0;
    EventSystem.current.SetSelectedGameObject(Button1);
        Piece1.gameObject.SetActive(false);
        Piece2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        selectedObj = EventSystem.current.currentSelectedGameObject;

        if (Button1 == selectedObj)
        {
            Piece1.gameObject.SetActive(true);
            Piece2.gameObject.SetActive(false);
        }
        if (Button2 == selectedObj)
        {
            Piece1.gameObject.SetActive(false);
            Piece2.gameObject.SetActive(true);
        }
    }
}
