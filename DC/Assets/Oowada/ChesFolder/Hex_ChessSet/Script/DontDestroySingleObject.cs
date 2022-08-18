using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySingleObject : MonoBehaviour
{
    public static DontDestroySingleObject Instance {get; private set;}

    public static int winner;//勝敗
    public static int p1Character;//プレイヤー１のキャラクター 
    public static int p2Character;//プレイヤー２のキャラクター 
    public static int p1item1;//プレイヤー１のアイテム１
    public static int p1item2;//プレイヤー１のアイテム２
    public static int p2item1;//プレイヤー２のアイテム１
    public static int p2item2;//プレイヤー２のアイテム２
    public static int p1Point;//プレイヤー１のポイント
    public static int p2Point;//プレイヤー２のポイント
    public static int p1TakePawn;//取ったポーン
    public static int p2TakePawn;
    public static int p1TakeRook;//取ったルーク
    public static int p2TakeRook;
    public static int p1TakeKnight;//取ったナイト
    public static int p2TakeKnight;
    public static int p1TakeBishop;//取ったビショップ
    public static int p2TakeBishop;
    public static int p1TakeQueen;//取ったクイーン
    public static int p2TakeQueen;
    public static int p1TakeKing;//取ったキング
    public static int p2TakeKing;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
