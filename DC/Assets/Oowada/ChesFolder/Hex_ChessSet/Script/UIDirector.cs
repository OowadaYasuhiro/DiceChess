using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIDirector : MonoBehaviour
{
    // UI関連
    GameObject txtTurnInfo;
    GameObject txtResultInfo;
    GameObject turnEndCursor;
    GameObject turnEndButtonBack;
    private bool hpDisplayl = false;
    [SerializeField] private Text hpText; //HPのテキスト
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image paseImage;
    GameObject player1FrontFrameBack;
    GameObject player2FrontFrameBack;
    GameObject charaTextPanel;
    GameObject charaTextPanel2;
    GameObject itemBackImageBack1;
    GameObject itemBackImageBack2;
    [SerializeField] private GameObject itemText1Panel;
    [SerializeField] private GameObject itemText2Panel;



    //コントローラのためのボタン
    [SerializeField] EventSystem eventSystem;
    GameObject selectedObj;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private Button endButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private GameObject mainCursor;


    void Start()
    {
        // UIオブジェクト取得
        txtTurnInfo = GameObject.Find("TextTurnInfo");
        txtResultInfo = GameObject.Find("TextResultInfo");
        turnEndCursor = GameObject.Find("ImageCanvas_Player1/TurnEndButton/TurnEndCursor");
        turnEndButtonBack = GameObject.Find("ImageCanvas_Player1/TurnEndButtonBack");
        charaTextPanel = GameObject.Find("ImageCanvas_Player1/CharaImage1P/CharaTextPanel");
        charaTextPanel2 = GameObject.Find("ImageCanvas_Player1/CharaImage2P/CharaTextPanel2");
        itemBackImageBack1 = GameObject.Find("ImageCanvas_Player1/ItemBoxPanel/Item1Panel/ItemBackImageBack");
        itemBackImageBack2 = GameObject.Find("ImageCanvas_Player1/ItemBoxPanel/Item2Panel/ItemBackImageBack");
        player1FrontFrameBack = GameObject.Find("ImageCanvas_Player1/CharaImage1P/Player1FrontFrameBack");
        player2FrontFrameBack = GameObject.Find("ImageCanvas_Player1/CharaImage2P/Player2FrontFrameBack");

        //コントローラーの初期化
        itemText1Panel.SetActive(false);
        itemBackImageBack1.SetActive(false);
        itemText2Panel.SetActive(false);
        itemBackImageBack2.SetActive(false);
        turnEndCursor.SetActive(false);
        turnEndButtonBack.SetActive(false);
        player1FrontFrameBack.SetActive(false);
        player2FrontFrameBack.SetActive(false);
        charaTextPanel.SetActive(false);
        charaTextPanel2.SetActive(false);
    }

    void Update()
    {

    }

    public void TurnInfo(int nowPlayer)//ターン数表示
    {
        // Infoの更新
        string nowColor = "";
        if (nowPlayer == 0) { nowColor = "白"; }
        else if (nowPlayer == 1) { nowColor = "黒"; }
        txtTurnInfo.GetComponent<Text>().text = "" + nowColor + "の番です";
    }
    public Text ResultInfo()//勝者表示
    {
        return txtResultInfo.GetComponent<Text>();
    }

    public void SelectButtonPlayer1()
    {
        mainCursor.SetActive(false);
        player1Button.Select();
        charaTextPanel.SetActive(true);
        player1FrontFrameBack.SetActive(true);
    }
    public void NotSelectButtonPlayer1()
    {
        charaTextPanel.SetActive(false);
        player1FrontFrameBack.SetActive(false);
    }
    public void SelectButtonPlayer2()
    {
        player2Button.Select();
        mainCursor.SetActive(false);
        charaTextPanel2.SetActive(true);
        player2FrontFrameBack.SetActive(true);
    }
    public void NotSelectButtonPlayer2()
    {
        charaTextPanel2.SetActive(false);
        player2FrontFrameBack.SetActive(false);
    }

    public void ItemUiFalse()
    {
        itemText1Panel.SetActive(false);
        itemBackImageBack1.SetActive(false);
        itemText2Panel.SetActive(false);
        itemBackImageBack2.SetActive(false);
    }
    public void ItemUiPlayer1True()
    {
        mainCursor.SetActive(false);
        item1Button.Select();
        itemText1Panel.SetActive(true);
        itemBackImageBack1.SetActive(true);
    }
    public void ItemUiPlayer2True()
    {
        mainCursor.SetActive(false);
        item2Button.Select();
        itemText2Panel.SetActive(true);
        itemBackImageBack2.SetActive(true);
    }
    public void SelectCursor()
    {
        selectedObj = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        mainCursor.SetActive(true);
    }

    public void EndButtonTrue()
    {
        mainCursor.SetActive(false);
        endButton.Select();
        turnEndCursor.SetActive(true);
        turnEndButtonBack.SetActive(true);
    }
    public void EndButtonFalse()
    {
        turnEndCursor.SetActive(false);
        turnEndButtonBack.SetActive(false);
    }
    public void TitleButtonTrue()
    {
        titleButton.Select();
    }
    public void TitleButtonFalse()
    {
        titleButton.Select();
        EventSystem.current.SetSelectedGameObject(null);
    }




}
