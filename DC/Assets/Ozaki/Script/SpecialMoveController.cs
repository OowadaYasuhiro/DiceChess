using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMoveController : MonoBehaviour
{
    //必殺技ゲージ
    public int damage = 3;
    public int minSp = 30;
    int hp;

    //相手のステータスを読み取るためのヤツ    
    CharacterStatus player1Chara; //プレイヤー1キャラのスクリプトを読み込む(読み込み対象変わるかも)
    CharacterStatus player2Chara; //同じく変わるかも
    GameSceneDirector sceneDirector;
    UnitController unitCon;
    GameObject[] player;

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
        if(sceneDirector.nowPlayer == 0 && player1Chara.sp == 100) {
            player = GameObject.FindGameObjectsWithTag("Player2");
            foreach(GameObject unit in player) {
                unitCon = unit.GetComponent<UnitController>();
                hp = unitCon.GetHP();
                hp -= damage;
                unitCon.SetHP(hp);
                Debug.Log(unit.name);

                if(unitCon.GetHP() <= 0) {
                    Destroy(unit);
                }
            }

            player1Chara.sp = 0;
        }
        //nowPlayerが1の時は相手の番
        else if(sceneDirector.nowPlayer == 1 && player2Chara.sp == 100) {
            player = GameObject.FindGameObjectsWithTag("Player1");

            foreach(GameObject unit in player) {
                unitCon = unit.GetComponent<UnitController>();
                hp = unitCon.GetHP();
                hp -= damage;
                unitCon.SetHP(hp);
                Debug.Log(unit.name);

                if(unitCon.GetHP() <= 0) {
                    Destroy(unit);
                }
            }

            player2Chara.sp = 0;
        }
        Debug.Log(damage);

        yield break;
    }

    public IEnumerator specialMove2() {
        yield break;
    }

    public IEnumerator specialMove3() {
        yield break;
    }

    public IEnumerator specialMove4() {
        if(sceneDirector.nowPlayer == 0 && player1Chara.sp == 100) {
            player2Chara.sp -= minSp;
            player1Chara.sp = 0;
        } else if(sceneDirector.nowPlayer == 1 && player2Chara.sp == 100) {
            player1Chara.sp -= minSp;
            player2Chara.sp = 0;
        }

        yield break;
    }

    public void getSpMove() {

    }

    /*必殺技使用方法
     * 別のスクリプトで「もしSpが100％かつプレイヤーアイコンを押されたら」という文を作る
     * 上のif分の中身にgetSpMoveを書く
     * 
     * 
     */
}
