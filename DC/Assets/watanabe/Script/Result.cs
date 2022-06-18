using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    //Pawnスコアを表示するUIの取得
    public Text pawnText;

    //Rookスコアを表示するUIの取得
    public Text rookText;

    //Knightスコアを表示するUIの取得
    public Text knightText;

    //Bishopスコアを表示するUIの取得
    public Text bishopText;

    //Queenスコアを表示するUIの取得
    public Text queenText;

    //Kingスコアを表示するUIの取得
    public Text kingText;

    //合計スコアを表示するUIの取得
    public Text totalScoreText;

    //スコアのカウント用
    private int score;

    //合計スコアのカウント用
    private int totalScore;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    //ゲーム開始前の状態に戻す
    private void Initialize()
    {
        //スコアを０に戻す
        score = 0;
    }

    //ポイントの追加　修飾子public
    public void AddPoint (int point)
    {
        score += point;

        Debug.Log(score);

        //ゲーム画面上のスコアを更新する
        DisplayScore();
    }

    //ゲーム画面上のスコアの表示を更新する
    private void DisplayScore()
    {
        //現在のスコアを画面に表示する
        pawnText .text = score.ToString();
        rookText .text = score.ToString();
        knightText .text = score.ToString();
        bishopText .text = score.ToString();
        queenText .text = score.ToString();
        kingText .text = score.ToString();
        totalScoreText .text = score.ToString();
    }

    
    

}
