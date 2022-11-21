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
    public Sprite backGloundWin;
    public Sprite backGloundLose;

    //Pawnスコアを表示するUIの取得
    public Image back1PImage;
    //Pawnスコアを表示するUIの取得
    public Image back2PImage;

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
    public Image totalScoreImage10;
    public Image totalScoreImage100;

    //キャライメージ
    [SerializeField] private GameObject TIKA;
    [SerializeField] private GameObject LIAN; 
    [SerializeField] private GameObject VIOLA;
    [SerializeField] private GameObject LLL;
    [SerializeField] private GameObject TIKAText;
    [SerializeField] private GameObject LIANText;
    [SerializeField] private GameObject VIOLAText;
    [SerializeField] private GameObject LLLText;


    public Image pawnPointImage_p2;
    public Image rookPointImage_p2;
    public Image knightPointImage_p2;
    public Image bishopPointImage_p2;
    public Image queenPointImage_p2;
    public Image kingPointImage_p2;
    [SerializeField] private GameObject TIKA_p2;
    [SerializeField] private GameObject LIAN_p2;
    [SerializeField] private GameObject VIOLA_p2;
    [SerializeField] private GameObject LLL_p2;
    [SerializeField] private GameObject TIKAText_p2;
    [SerializeField] private GameObject LIANText_p2;
    [SerializeField] private GameObject VIOLAText_p2;
    [SerializeField] private GameObject LLLText_p2;
    public Image totalScoreImage10_p2;
    public Image totalScoreImage100_p2;

    RectTransform whiteRect;
    RectTransform rect;
    public GameObject WhitePanel;
    public GameObject Panel;
    bool White = true;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        setcount();
        setCharacter();
        whiteRect = WhitePanel.GetComponent<RectTransform>();
        Panel.GetComponent<FadeController>().isFadeIn = 1;     
    }

    private void Update()
    {
        timer += Time.deltaTime; // 経過時間を計算
        if (Input.GetKeyDown("joystick 1 button 0")|| Input.GetMouseButtonDown(0))
        {
            if (timer >= 2)
            {
                Panel.GetComponent<FadeController>().isFadeOut = 2;
                Invoke("abc", 0.5f);
                timer = 0;
            }
        }
        if (Input.GetKeyDown("joystick 1 button 1") || Input.GetMouseButtonDown(1))
        {
            ClickStartButton();
        }
    }


    //Aボタンでタイトルに戻る
    public void ClickStartButton()
    {
        SceneManager.LoadScene("Title");
    }
    public void abc()
    {
        if (White == true) { whiteRect.localPosition = new Vector3(0, 0, 0); White = false; }
        else if (White == false) { whiteRect.localPosition = new Vector3(-3860, 0, 0); White = true; }
        Panel.GetComponent<FadeController>().isFadeIn = 1;
    }

    public void setcount()
    {
        if (DontDestroySingleObject.p1TakePawn == 0) { pawnPointImage.sprite = point0; }
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

        if (DontDestroySingleObject.p1Point / 100 == 0) { totalScoreImage100.sprite = point0; }
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

        if (DontDestroySingleObject.p2TakePawn == 0) { pawnPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakePawn == 1) { pawnPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakePawn == 2) { pawnPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2TakeRook == 1) { rookPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakeRook == 0) { rookPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakeRook == 2) { rookPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2TakeKnight == 0) { knightPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakeKnight == 1) { knightPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakeKnight == 2) { knightPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2TakeBishop == 0) { bishopPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakeBishop == 1) { bishopPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakeBishop == 2) { bishopPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2TakeQueen == 0) { queenPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakeQueen == 1) { queenPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakeQueen == 2) { queenPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2TakeKing == 0) { kingPointImage_p2.sprite = point0; }
        if (DontDestroySingleObject.p2TakeKing == 1) { kingPointImage_p2.sprite = point1; }
        if (DontDestroySingleObject.p2TakeKing == 2) { kingPointImage_p2.sprite = point2; }

        if (DontDestroySingleObject.p2Point / 100 == 0) { totalScoreImage100_p2.sprite = point0; }
        if (DontDestroySingleObject.p2Point / 100 == 1) { totalScoreImage100_p2.sprite = point1; }
        if (DontDestroySingleObject.p2Point / 100 == 2) { totalScoreImage100_p2.sprite = point2; }
        if (DontDestroySingleObject.p2Point / 100 == 3) { totalScoreImage100_p2.sprite = point3; }
        if (DontDestroySingleObject.p2Point / 100 == 4) { totalScoreImage100_p2.sprite = point4; }
        if (DontDestroySingleObject.p2Point / 100 == 5) { totalScoreImage100_p2.sprite = point5; }

        if (DontDestroySingleObject.p2Point % 100 / 10 == 0) { totalScoreImage10_p2.sprite = point0; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 1) { totalScoreImage10_p2.sprite = point1; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 2) { totalScoreImage10_p2.sprite = point2; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 3) { totalScoreImage10_p2.sprite = point3; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 4) { totalScoreImage10_p2.sprite = point4; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 5) { totalScoreImage10_p2.sprite = point5; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 6) { totalScoreImage10_p2.sprite = point6; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 7) { totalScoreImage10_p2.sprite = point7; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 8) { totalScoreImage10_p2.sprite = point8; }
        if (DontDestroySingleObject.p2Point % 100 / 10 == 9) { totalScoreImage10_p2.sprite = point9; }
    }

    public void setCharacter()
    {
        TIKA.SetActive(false);
        LIAN.SetActive(false);
        VIOLA.SetActive(false);
        LLL.SetActive(false);
        TIKAText.SetActive(false);
        LIANText.SetActive(false);
        VIOLAText.SetActive(false);
        LLLText.SetActive(false);
        TIKA_p2.SetActive(false);
        LIAN_p2.SetActive(false);
        VIOLA_p2.SetActive(false);
        LLL_p2.SetActive(false);
        TIKAText_p2.SetActive(false);
        LIANText_p2.SetActive(false);
        VIOLAText_p2.SetActive(false);
        LLLText_p2.SetActive(false);
        if (DontDestroySingleObject.p1Character == 0) { TIKA.SetActive(true); TIKAText.SetActive(true); }
        if (DontDestroySingleObject.p1Character == 1) { LIAN.SetActive(true); LIANText.SetActive(true); }
        if (DontDestroySingleObject.p1Character == 2) { VIOLA.SetActive(true); VIOLAText.SetActive(true); }
        if (DontDestroySingleObject.p1Character == 3) { LLL.SetActive(true); LLLText.SetActive(true); }
        if (DontDestroySingleObject.p2Character == 0) { TIKA_p2.SetActive(true); TIKAText_p2.SetActive(true); }
        if (DontDestroySingleObject.p2Character == 1) { LIAN_p2.SetActive(true); LIANText_p2.SetActive(true); }
        if (DontDestroySingleObject.p2Character == 2) { VIOLA_p2.SetActive(true); VIOLAText_p2.SetActive(true); }
        if (DontDestroySingleObject.p2Character == 3) { LLL_p2.SetActive(true); LLLText_p2.SetActive(true); }
        if (DontDestroySingleObject.winner == 0) { back1PImage.sprite = backGloundWin; back2PImage.sprite = backGloundLose; }
        if (DontDestroySingleObject.winner == 1) { back2PImage.sprite = backGloundWin; back1PImage.sprite = backGloundLose; }

    }


}
