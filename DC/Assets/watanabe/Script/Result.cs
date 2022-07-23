using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{   
    //Pawnスコアを表示するUIの取得
    public Text PawnImage9;

    //Rookスコアを表示するUIの取得
    public Text RookImage10;

    //Knightスコアを表示するUIの取得
    public Text KnightImage11;

    //Bishopスコアを表示するUIの取得
    public Text BishopImage12;

    //Queenスコアを表示するUIの取得
    public Text QueenImage13;

    //Kingスコアを表示するUIの取得
    public Text KingImage14;

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
        PawnImage9 .text = score.ToString();
        RookImage10 .text = score.ToString();
        KnightImage11 .text = score.ToString();
        BishopImage12 .text = score.ToString();
        QueenImage13 .text = score.ToString();
        KingImage14 .text = score.ToString();
        totalScoreText .text = score.ToString();
    }

    //Aボタンでタイトルに戻る
    public void ClickStartButton()
    {
        SceneManager.LoadScene("Title");
    }
    

}
