using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ItemController : MonoBehaviour
{
    GameSceneDirector GSD;
    UnitController unitCon;
    CharacterStatus status;
    EffectController eff;
    GameObject[] player;
    [SerializeField]
    private GameObject[] chara;
    private int recovery = 3;//回復量
    private int spRecovery = 30;//sp回復量
    private int hp = 0;
    private int maxHp = 0;
    private int move = 2;
    //アイテムを一回で使うためのbool
    private bool canItem1_1P;
    private bool canItem1_2P;
    private bool canItem2_1P;
    private bool canItem2_2P;

    [SerializeField] private GameObject SeObject;//SE

    [SerializeField] private Sprite item1;
    [SerializeField] private Sprite item2;
    [SerializeField] private Sprite item3;
    [SerializeField] private Sprite item4;

    //アイテム背景改編のため
    [SerializeField] private Image item1Box;
    [SerializeField] private Image item2Box;
    [SerializeField] private Image item1Image;
    [SerializeField] private Image item2Image;

    [SerializeField] private Text item1text;
    [SerializeField] private Text item2text;

    [SerializeField] private Sprite chara1;
    [SerializeField] private Sprite chara2;
    [SerializeField] private Sprite chara3;
    [SerializeField] private Sprite chara4;
    [SerializeField] private Image chara1Image;
    [SerializeField] private Image chara2Image;
    [SerializeField] private Text chara1text;
    [SerializeField] private Text chara2text;

    //csv
    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public Vector3 position;
    public Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        eff = GameObject.Find("SceneDirector").GetComponent<EffectController>();
        GSD = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
        canItem1_1P = true;
        canItem1_2P = true;
        canItem2_1P = true;
        canItem2_2P = true;
        if (DontDestroySingleObject.p1Character == 0) { chara1Image.sprite = chara1; chara1text.text = "相手の駒すべてに3ダメージ与える。"; }
        if (DontDestroySingleObject.p1Character == 1) { chara1Image.sprite = chara2; chara1text.text = "相手の駒すべてに3ダメージ与える。"; }
        if (DontDestroySingleObject.p1Character == 2) { chara1Image.sprite = chara3; chara1text.text = "相手の必殺技ゲージを－30する。"; }
        if (DontDestroySingleObject.p1Character == 3) { chara1Image.sprite = chara4; chara1text.text = "相手の必殺技ゲージを－30する。"; }
        if (DontDestroySingleObject.p2Character == 0) { chara2Image.sprite = chara1; chara2text.text = "相手の駒すべてに3ダメージ与える。"; }
        if (DontDestroySingleObject.p2Character == 1) { chara2Image.sprite = chara2; chara2text.text = "相手の駒すべてに3ダメージ与える。"; }
        if (DontDestroySingleObject.p2Character == 2) { chara2Image.sprite = chara3; chara2text.text = "相手の必殺技ゲージを－30する。"; }
        if (DontDestroySingleObject.p2Character == 3) { chara2Image.sprite = chara4; chara2text.text = "相手の必殺技ゲージを－30する。"; }

        //csv
        csvFile = Resources.Load("CSV/ItemSpecial") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        recovery = int.Parse(csvDatas[0][3]);
        //Debug.Log("回復量は"+csvDatas[0][3]);
        spRecovery = int.Parse(csvDatas[0][4]);
        //Debug.Log("必殺増加量は" + csvDatas[0][4]);
    }

    // Update is called once per frame
    void Update()
    {
        //仮置き
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Kaihuku());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(DamageItem());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(SpItem());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(Twice());
        }


        if (GSD.nowPlayer == 0){
            if (DontDestroySingleObject.p1item1 == 0){
                item1Box.sprite = item1;
                item1text.text = "全自駒のHPを3回復"; 
            }
            if (DontDestroySingleObject.p1item1 == 1){
                item1Box.sprite = item2;
                item1text.text = "次の攻撃力2倍";
            }
            if (DontDestroySingleObject.p1item1 == 2){
                item1Box.sprite = item3;
                item1text.text = "必殺技ゲージ30増加";
            }
            if (DontDestroySingleObject.p1item1 == 3){
                item1Box.sprite = item4;
                item1text.text = "移動距離2倍";
            }
            if (DontDestroySingleObject.p1item2 == 0){
                item2Box.sprite = item1;
                item2text.text = "全自駒のHPを3回復";
            }
            if (DontDestroySingleObject.p1item2 == 1){
                item2Box.sprite = item2;
                item2text.text = "次の攻撃力2倍";
            }
            if (DontDestroySingleObject.p1item2 == 2){
                item2Box.sprite = item3;
                item2text.text = "必殺技ゲージ30増加";
            }
            if (DontDestroySingleObject.p1item2 == 3){
                item2Box.sprite = item4;
                item2text.text = "移動距離2倍";
            }
            if (canItem1_1P == false){
                item1Box.color = new Color32(105, 105, 105, 255);
                item1Image.color = new Color32(105, 105, 105, 255);
            }
            else{
                item1Box.color = new Color32(255, 255, 255, 255);
                item1Image.color = new Color32(255, 255, 255, 255);
            }
        }
        if (GSD.nowPlayer == 1){
            if (DontDestroySingleObject.p2item1 == 0){
                item1Box.sprite = item1;
                item1text.text = "全自駒のHPを3回復";
            }
            if (DontDestroySingleObject.p2item1 == 1){
                item1Box.sprite = item2;
                item1text.text = "次の攻撃力2倍";
            }
            if (DontDestroySingleObject.p2item1 == 2){
                item1Box.sprite = item3;
                item1text.text = "必殺技ゲージ30増加";
            }
            if (DontDestroySingleObject.p2item1 == 3){
                item1Box.sprite = item4;
                item1text.text = "移動距離2倍";
            }
            if (DontDestroySingleObject.p2item2 == 0){
                item2Box.sprite = item1;
                item2text.text = "全自駒のHPを3回復";
            }
            if (DontDestroySingleObject.p2item2 == 1){
                item2Box.sprite = item2;
                item2text.text = "次の攻撃力2倍";
            }
            if (DontDestroySingleObject.p2item2 == 2){
                item2Box.sprite = item3;
                item2text.text = "必殺技ゲージ30増加";
            }
            if (DontDestroySingleObject.p2item2 == 3){
                item2Box.sprite = item4;
                item2text.text = "移動距離2倍";
            }
            if (canItem1_2P == false){
                item1Box.color = new Color32(105, 105, 105, 255);
                item1Image.color = new Color32(105, 105, 105, 255);
            }
            else
            {
                item1Box.color = new Color32(255, 255, 255, 255);
                item1Image.color = new Color32(255, 255, 255, 255);
            }
        }
        if (GSD.nowPlayer == 0){
            if (canItem2_1P == false){
                item2Box.color = new Color32(105, 105, 105, 255);
                item2Image.color = new Color32(105, 105, 105, 255);
            }
            else
            {
                item2Box.color = new Color32(255, 255, 255, 255);
                item2Image.color = new Color32(255, 255, 255, 255);
            }
        }
        if (GSD.nowPlayer == 1){
            if (canItem2_2P == false){
                item2Box.color = new Color32(105, 105, 105, 255);
                item2Image.color = new Color32(105, 105, 105, 255);
            }
            else
            {
                item2Box.color = new Color32(255, 255, 255, 255);
                item2Image.color = new Color32(255, 255, 255, 255);
            }
        }

    }

    //回復する
    public IEnumerator Kaihuku()
    {

        eff.setEff(2);
        SE sePlayer = SeObject.GetComponent<SE>();
        sePlayer.moveSound9();

        yield return new WaitForSeconds(0.4f);

        //nowPlayerが0の時は自分の番
        if(GSD.nowPlayer == 0)
        {
            //ヒエラルキー上にあるPlayer1タグがついたオブジェクトをすべて取得する(今回は自分の駒すべて)
            player = GameObject.FindGameObjectsWithTag("Player1");
            //取得したオブジェクトを一つづつ回復処理を行う(全て終わるまでループする)
            foreach(GameObject unit in player)
            {
                unitCon = unit.GetComponent<UnitController>();
                hp = unitCon.GetHP();
                maxHp = unitCon.GetMaxHp();
                hp += recovery;
                //もし回復した時,最大体力を越したとき越した分減らす文章
                if (maxHp < hp)
                {
                    hp -= hp - maxHp;
                }
                unitCon.SetHP(hp);
                Debug.Log(unit.name);
            }
        }
        //nowPlayerが1の時は相手の番
        else if (GSD.nowPlayer == 1)
        {
            //ヒエラルキー上にあるPlayer2タグがついたオブジェクトをすべて取得する(今回は相手の駒すべて)
            player = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject unit in player)
            {
                unitCon = unit.GetComponent<UnitController>();
                hp = unitCon.GetHP();
                maxHp = unitCon.GetMaxHp();
                hp += recovery;
                //もし回復した時,最大体力を越したとき越した分減らす文章
                if (maxHp < hp)
                {
                    hp -= hp - maxHp;
                }
                unitCon.SetHP(hp);
                Debug.Log(unit.name);
            }
        }
        Debug.Log(player);
        yield break;
    }

    //攻撃力を二倍にする
    public IEnumerator DamageItem()
    {
        eff.setEff(2);
        SE sePlayer = SeObject.GetComponent<SE>();
        sePlayer.moveSound9();
        yield return new WaitForSeconds(0.4f);

        //自分の番に使った時damageflagを1増やす
        if (GSD.nowPlayer == 0)
        {
            GSD.damageFlag += 1;
            Debug.Log("1Pa");
        }
        //相手の番に使った時damageflagを2増やす
        else if (GSD.nowPlayer == 1)
        {
            GSD.damageFlag += 2;
            Debug.Log("2Pa");
        }
        Debug.Log(GSD.damageFlag);
        yield break;
    }

    public IEnumerator SpItem()
    {
        eff.setEff(2);
        SE sePlayer = SeObject.GetComponent<SE>();
        sePlayer.moveSound9();
        yield return new WaitForSeconds(0.4f);

        //nowPlayerが0の時は自分の番
        if (GSD.nowPlayer == 0)
        {
            status = chara[0].GetComponent<CharacterStatus>();
            status.setSP(spRecovery);
            status.setPlayer1SpBar();
        }
        //nowPlayerが1の時は相手の番
        else if (GSD.nowPlayer == 1)
        {
            status = chara[1].GetComponent<CharacterStatus>();
            status.setSP(spRecovery);
            status.setPlayer2SpBar();
        }
        Debug.Log(GSD.nowPlayer + "pに" + spRecovery + "したのでspの合計値が" + status.Sp);
        yield break;
    }

    public IEnumerator Twice()
    {
        eff.setEff(2);
        SE sePlayer = SeObject.GetComponent<SE>();
        sePlayer.moveSound9();
        yield return new WaitForSeconds(0.4f);

        //nowPlayerが0の時は自分の番
        if (GSD.nowPlayer == 0)
        {
            //ヒエラルキー上にあるPlayer1タグがついたオブジェクトをすべて取得する(今回は自分の駒すべて)
            player = GameObject.FindGameObjectsWithTag("Player1");
            //取得したオブジェクトを一つづつ回復処理を行う(全て終わるまでループする)
            foreach (GameObject unit in player)
            {
                unitCon = unit.GetComponent<UnitController>();
                unitCon.moveTwice = move;
                Debug.Log(unitCon.moveTwice);
            }
        }
        //nowPlayerが1の時は相手の番
        else if (GSD.nowPlayer == 1)
        {
            //ヒエラルキー上にあるPlayer2タグがついたオブジェクトをすべて取得する(今回は相手の駒すべて)
            player = GameObject.FindGameObjectsWithTag("Player2");
            foreach (GameObject unit in player)
            {
                unitCon = unit.GetComponent<UnitController>();
                unitCon.moveTwice = move;
                Debug.Log(unitCon.moveTwice);
            }
        }
        yield break;
    }

    public void UseItem1()
    {
        //アイテムを一回だけ使わせるため
        if(GSD.nowPlayer == 0)
        {
            if (canItem1_1P)
            {
                if (DontDestroySingleObject.p1item1 == 0){
                    StartCoroutine("Kaihuku");
                }else if(DontDestroySingleObject.p1item1 == 1){
                    StartCoroutine("DamageItem");
                }
                else if (DontDestroySingleObject.p1item1 == 2){
                    StartCoroutine("SpItem");
                }
                else if (DontDestroySingleObject.p1item1 == 3){
                    StartCoroutine("Twice");
                }
                canItem1_1P = false;
            }
        }
        if (GSD.nowPlayer == 1)
        {
            if (canItem1_2P)
            {
                if (DontDestroySingleObject.p2item1 == 0){
                    StartCoroutine("Kaihuku");
                }
                else if (DontDestroySingleObject.p2item1 == 1){
                    StartCoroutine("DamageItem");
                }
                else if (DontDestroySingleObject.p2item1 == 2){
                    StartCoroutine("SpItem");
                }
                else if (DontDestroySingleObject.p2item1 == 3){
                    StartCoroutine("Twice");
                }
                canItem1_2P = false;
            }
        }
    }
    public void UseItem2()
    {
        if (GSD.nowPlayer == 0)
        {
            if (canItem2_1P)
            {
                if (DontDestroySingleObject.p1item2 == 0){
                    StartCoroutine("Kaihuku");
                }
                else if (DontDestroySingleObject.p1item2 == 1){
                    StartCoroutine("DamageItem");
                }
                else if (DontDestroySingleObject.p1item2 == 2){
                    StartCoroutine("SpItem");
                }
                else if (DontDestroySingleObject.p1item2 == 3){
                    StartCoroutine("Twice");
                }
                canItem2_1P = false;
            }
        }
        if (GSD.nowPlayer == 1)
        {
            if (canItem2_2P)
            {
                if (DontDestroySingleObject.p2item2 == 0){
                    StartCoroutine("Kaihuku");
                }
                else if (DontDestroySingleObject.p2item2 == 1){
                    StartCoroutine("DamageItem");
                }
                else if (DontDestroySingleObject.p2item2 == 2){
                    StartCoroutine("SpItem");
                }
                else if (DontDestroySingleObject.p2item2 == 3){
                    StartCoroutine("Twice");
                }
                canItem2_2P = false;
            }
        }
    }
}
