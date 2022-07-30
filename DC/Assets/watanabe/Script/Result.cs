using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public Sprite point0;
    public Sprite point1;
    public Sprite point2;
    public Sprite point3;
    public Sprite point4;
    public Sprite point5;
    public Sprite point6;
    public Sprite point7;
    public Sprite point8;
    public Sprite point9;

    //Pawnスコアを表示するUIの取得
    public Image pawnPointImage;

    //Rookスコアを表示するUIの取得
    public Image rookPointImage;

    //Knightスコアを表示するUIの取得
    public Image knightPointImage;

    //Bishopスコアを表示するUIの取得
    public Image bishopPointImage;

    //Queenスコアを表示するUIの取得
    public Image queenPointImage;

    //Kingスコアを表示するUIの取得
    public Image kingPointImage;

    //合計スコアを表示するUIの取得

    public Text totalScoreImage;

    //スコアのカウント用
    private int score;

    //合計スコアのカウント用
    private int totalScore;

    public Image totalScoreImage10;
    public Image totalScoreImage100;


    // Start is called before the first frame update
    void Start()
    {
        setcount();
    }

    public void setcount()
    {
        if (DontDestroySingleObject.p1TakePawn == 0) { pawnPointImage.sprite = point0;}
        if (DontDestroySingleObject.p1TakePawn == 1) { pawnPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakePawn == 2) { pawnPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1TakeRook == 1) { rookPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakeRook == 0) { rookPointImage.sprite = point0; }
        if (DontDestroySingleObject.p1TakeRook == 2) { rookPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1TakeKnight == 0) { knightPointImage.sprite = point0; }
        if (DontDestroySingleObject.p1TakeKnight == 1) { knightPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakeKnight == 2) { knightPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1TakeBishop == 0) { bishopPointImage.sprite = point0; }
        if (DontDestroySingleObject.p1TakeBishop == 1) { bishopPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakeBishop == 2) { bishopPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1TakeQueen == 0) { queenPointImage.sprite = point0; }
        if (DontDestroySingleObject.p1TakeQueen == 1) { queenPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakeQueen == 2) { queenPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1TakeKing == 0) { kingPointImage.sprite = point0; }
        if (DontDestroySingleObject.p1TakeKing == 1) { kingPointImage.sprite = point1; }
        if (DontDestroySingleObject.p1TakeKing == 2) { kingPointImage.sprite = point2; }

        if (DontDestroySingleObject.p1Point / 100 == 0) { totalScoreImage100.sprite = point0;}
        if (DontDestroySingleObject.p1Point / 100 == 1) { totalScoreImage100.sprite = point1; }
        if (DontDestroySingleObject.p1Point / 100 == 2) { totalScoreImage100.sprite = point2; }
        if (DontDestroySingleObject.p1Point / 100 == 3) { totalScoreImage100.sprite = point3; }
        if (DontDestroySingleObject.p1Point / 100 == 4) { totalScoreImage100.sprite = point4; }
        if (DontDestroySingleObject.p1Point / 100 == 5) { totalScoreImage100.sprite = point5; }

        if (DontDestroySingleObject.p1Point % 100 / 10 == 0) { totalScoreImage10.sprite = point0; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 1) { totalScoreImage10.sprite = point1; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 2) { totalScoreImage10.sprite = point2; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 3) { totalScoreImage10.sprite = point3; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 4) { totalScoreImage10.sprite = point4; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 5) { totalScoreImage10.sprite = point5; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 6) { totalScoreImage10.sprite = point6; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 7) { totalScoreImage10.sprite = point7; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 8) { totalScoreImage10.sprite = point8; }
        if (DontDestroySingleObject.p1Point % 100 / 10 == 9) { totalScoreImage10.sprite = point9; }
    }

    private void Update()
    {
        if (Input.GetKeyDown("joystick 1 button 0"))
        {
            ClickStartButton();
        }

    }

    //Aボタンでタイトルに戻る
    public void ClickStartButton()
    {
        SceneManager.LoadScene("Title");
    }
    

}
