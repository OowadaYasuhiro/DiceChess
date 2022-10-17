using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;


public class SpecialMoveController : MonoBehaviour
{
    //必殺技ゲージ
    private int damage = 3;
    private int minSp = 15;
    int hp;

    //csv
    TextAsset csvFile; 
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public Vector3 position;
    public Quaternion rotation;

    //相手のステータスを読み取るためのヤツ    
    CharacterStatus player1Chara; //プレイヤー1キャラのスクリプトを読み込む(読み込み対象変わるかも)
    CharacterStatus player2Chara; //同じく変わるかも
    GameSceneDirector sceneDirector;
    EffectController effCon;
    UnitController unitCon;
    GameObject[] player;
    Vector3 enemyVec;
    Animator fadeAnim;
    GameObject txtResultInfo;

    //キャラタイプ。これで必殺技内容を変える。 1=ティカ　2=リアン　3=ヴィオラ　4=ララ＆リリ＆ロロ
    public enum TYPE
    {
        NONE = -1,
        TIKA = 1,
        LIAN,
        VIOLA,
        LLL
    }

    // Start is called before the first frame update
    void Start()
    {
        player1Chara = GameObject.Find("Player1Chara").GetComponent<CharacterStatus>();
        player2Chara = GameObject.Find("Player2Chara").GetComponent<CharacterStatus>();
        sceneDirector = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
        effCon = GameObject.Find("SceneDirector").GetComponent<EffectController>();
        fadeAnim = GameObject.Find("AttackBackPanel").GetComponent<Animator>();
        txtResultInfo = GameObject.Find("TextResultInfo");

        //csv
        csvFile = Resources.Load("CSV/ItemSpecial") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        damage = int.Parse(csvDatas[0][1]);
        //Debug.Log("必殺1 ダメージ" + csvDatas[0][1]);
        minSp = int.Parse(csvDatas[0][2]);
        //Debug.Log("必殺4 SP吸収" + csvDatas[0][2]);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine("specialMove1");
        }
        if(Input.GetKeyDown(KeyCode.X)) {
            StartCoroutine("specialMove4");
        }
    }

    public IEnumerator specialMove1() {
        

        //nowPlayerが0の時は自分の番
        if(sceneDirector.nowPlayer == 0 && player1Chara.Sp == int.Parse(csvDatas[0][0])) {
            player1Chara.setSP(-int.Parse(csvDatas[0][0]));
            player1Chara.setPlayer1SpBar();
            player1Chara.setPlayer1SpColor();

            effCon.setPositionEff(1,-8.25f,0f,-5f ,-35f,0f,0f);
            sceneDirector.tileBoolInversion();
            yield return new WaitForSeconds(0.3f);
            
            fadeAnim.SetTrigger("spFadeIn");
            yield return new WaitForSeconds(0.5f);

            effCon.setEff(3);

            yield return new WaitForSeconds(0.4f);

            fadeAnim.SetTrigger("spFadeOut");

            yield return new WaitForSeconds(0.3f);

            player = GameObject.FindGameObjectsWithTag("Player2");
            foreach(GameObject unit in player) {
                unitCon = unit.GetComponent<UnitController>();
                //敵全体(一体一体)にヒットエフェクトを発生させる
                enemyVec = unit.transform.position;
                effCon.enemyPositionEff(0,enemyVec);
                Debug.Log("名前");
                hp = unitCon.GetHP();
                hp -= damage;
                unitCon.SetHP(hp);
                unitCon.HpDisplay();
                Debug.Log(unit.name);

                int a = unitCon.GetTYPE();
                if (a == 1 && hp <= 0) { DontDestroySingleObject.p1TakePawn++; }
                if (a == 1 && hp <= 0) { DontDestroySingleObject.p2TakePawn++; }
                if (a == 2 && hp <= 0) { DontDestroySingleObject.p1TakeRook++; }
                if (a == 2 && hp <= 0) { DontDestroySingleObject.p2TakeRook++; }
                if (a == 3 && hp <= 0) { DontDestroySingleObject.p1TakeKnight++; }
                if (a == 3 && hp <= 0) { DontDestroySingleObject.p2TakeKnight++; }
                if (a == 4 && hp <= 0) { DontDestroySingleObject.p1TakeBishop++; }
                if (a == 4 && hp <= 0) { DontDestroySingleObject.p2TakeBishop++; }
                if (a == 5 && hp <= 0) { DontDestroySingleObject.p1TakeQueen++; }
                if (a == 5 && hp <= 0) { DontDestroySingleObject.p2TakeQueen++; }
                if (a == 6 && hp <= 0) { DontDestroySingleObject.p1TakeKing++; }
                if (a == 6 && hp <= 0) { DontDestroySingleObject.p2TakeKing++; }
                if (a == 6)
                {
                    if(hp <= 0)
                    {
                        effCon.enemyPositionEff(4, enemyVec);
                        Text info = txtResultInfo.GetComponent<Text>();
                        //ちゃんとした勝利エフェクトを作れ
                        DontDestroySingleObject.winner = 0;
                        info.text = "1Pの勝ち";
                        Invoke("goResult", 3.0f);
                        Destroy(unit);
                    }
                }

                if(unitCon.GetHP() <= 0) {
                    effCon.enemyPositionEff(4, enemyVec);
                    Destroy(unit);
                }
            }

            
        }
        //nowPlayerが1の時は相手の番
        else if(sceneDirector.nowPlayer == 1 && player2Chara.Sp == int.Parse(csvDatas[0][0])) {
            player2Chara.setSP(-int.Parse(csvDatas[0][0]));
            player2Chara.setPlayer2SpBar();
            player2Chara.setPlayer2SpColor();

            effCon.setPositionEff(1, 6.25f, 5f, -1.5f, -35f, 0f, 0f);
            sceneDirector.tileBoolInversion();
            yield return new WaitForSeconds(0.3f);

            fadeAnim.SetTrigger("spFadeIn");
            yield return new WaitForSeconds(0.3f);

            effCon.setEff(3);

            yield return new WaitForSeconds(0.4f);

            fadeAnim.SetTrigger("spFadeOut");

            yield return new WaitForSeconds(0.3f);

            player = GameObject.FindGameObjectsWithTag("Player1");
            foreach(GameObject unit in player) {
                unitCon = unit.GetComponent<UnitController>();

                enemyVec = unit.transform.position;
                effCon.enemyPositionEff(0, enemyVec);

                hp = unitCon.GetHP();
                hp -= damage;
                unitCon.SetHP(hp);
                unitCon.HpDisplay();
                Debug.Log("この駒は"+unit.name);

                int a = unitCon.GetTYPE();
                int b = unitCon.GetPOINT();
                if (a == 1 && hp <= 0) { DontDestroySingleObject.p1TakePawn++; DontDestroySingleObject.p1Point += b;}
                if (a == 1 && hp <= 0) { DontDestroySingleObject.p2TakePawn++; DontDestroySingleObject.p2Point += b; }
                if (a == 2 && hp <= 0) { DontDestroySingleObject.p1TakeRook++; DontDestroySingleObject.p1Point += b; }
                if (a == 2 && hp <= 0) { DontDestroySingleObject.p2TakeRook++; DontDestroySingleObject.p2Point += b; }
                if (a == 3 && hp <= 0) { DontDestroySingleObject.p1TakeKnight++; DontDestroySingleObject.p1Point += b; }
                if (a == 3 && hp <= 0) { DontDestroySingleObject.p2TakeKnight++; DontDestroySingleObject.p2Point += b; }
                if (a == 4 && hp <= 0) { DontDestroySingleObject.p1TakeBishop++; DontDestroySingleObject.p1Point += b; }
                if (a == 4 && hp <= 0) { DontDestroySingleObject.p2TakeBishop++; DontDestroySingleObject.p2Point += b; }
                if (a == 5 && hp <= 0) { DontDestroySingleObject.p1TakeQueen++; DontDestroySingleObject.p1Point += b; }
                if (a == 5 && hp <= 0) { DontDestroySingleObject.p2TakeQueen++; DontDestroySingleObject.p2Point += b; }
                if (a == 6 && hp <= 0) { DontDestroySingleObject.p1TakeKing++; DontDestroySingleObject.p1Point += b; }
                if (a == 6 && hp <= 0) { DontDestroySingleObject.p2TakeKing++; DontDestroySingleObject.p2Point += b; }
                if (a == 6)
                {
                    if (hp <= 0)
                    {
                        effCon.enemyPositionEff(4, enemyVec);
                        Text info = txtResultInfo.GetComponent<Text>();
                        DontDestroySingleObject.winner = 1;
                        //ちゃんとした勝利エフェクトを作れ
                        info.text = "2Pの勝ち";
                        Invoke("goResult", 3.0f);
                        Destroy(unit);
                    }
                }


                if (unitCon.GetHP() <= 0) {
                    effCon.enemyPositionEff(4, enemyVec);
                    Destroy(unit);
                }
            }

            
        }
        Debug.Log("ダメージは"+damage);

        yield return new WaitForSeconds(1f);

        sceneDirector.tileBoolInversion();

        yield break;
    }

    public IEnumerator specialMove2() {
        yield break;
    }

    public IEnumerator specialMove3() {
        yield break;
    }

    public IEnumerator specialMove4() {
        if(sceneDirector.nowPlayer == 0 && player1Chara.Sp == int.Parse(csvDatas[0][0])) {
            player1Chara.setSP(-int.Parse(csvDatas[0][0]));
            player1Chara.setPlayer1SpBar();
            player1Chara.setPlayer1SpColor();

            player2Chara.setSP(-minSp);
            player2Chara.setPlayer2SpBar();
            player2Chara.setPlayer2SpColor();

        } else if(sceneDirector.nowPlayer == 1 && player2Chara.Sp == int.Parse(csvDatas[0][0])) {
            player2Chara.setSP(-int.Parse(csvDatas[0][0]));
            player2Chara.setPlayer2SpBar();
            player2Chara.setPlayer2SpColor();

            player1Chara.setSP(-minSp);
            player1Chara.setPlayer1SpBar();
            player1Chara.setPlayer1SpColor();

        }

        yield break;
    }

    public void getSpMove() {
        if (sceneDirector.nowPlayer == 0){
            if (DontDestroySingleObject.p1Character == 0 || DontDestroySingleObject.p1Character == 1){
                StartCoroutine("specialMove1");
            }
            if (DontDestroySingleObject.p1Character == 2 || DontDestroySingleObject.p1Character == 3){
                StartCoroutine("specialMove4");
            }
        }
        if (sceneDirector.nowPlayer == 1 ){
            if (DontDestroySingleObject.p2Character == 0 || DontDestroySingleObject.p2Character == 1){
                StartCoroutine("specialMove1");
            }
            if (DontDestroySingleObject.p2Character == 2 || DontDestroySingleObject.p2Character == 3){
                StartCoroutine("specialMove4");
            }
        }
    }

    public void goResult(){
        SceneManager.LoadScene("Result3");
    }
    /*必殺技使用方法
     * 別のスクリプトで「もしSpが100％かつプレイヤーアイコンを押されたら」という文を作る
     * 上のif分の中身にgetSpMoveを書く
     * 
     * 
     */
}
