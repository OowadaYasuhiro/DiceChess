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
    EffectController effCon;
    UnitController unitCon;
    GameObject[] player;
    Vector3 enemyVec;
    Animator fadeAnim;

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
        if(sceneDirector.nowPlayer == 0 && player1Chara.Sp == 100) {
            player1Chara.setSP(-100);
            player1Chara.setPlayer1SpBar();

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

                hp = unitCon.GetHP();
                hp -= damage;
                unitCon.SetHP(hp);
                Debug.Log(unit.name);

                if(unitCon.GetHP() <= 0) {
                    effCon.enemyPositionEff(4, enemyVec);
                    Destroy(unit);
                }
            }

            
        }
        //nowPlayerが1の時は相手の番
        else if(sceneDirector.nowPlayer == 1 && player2Chara.Sp == 100) {
            player2Chara.setSP(-100);
            player2Chara.setPlayer2SpBar();

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
                Debug.Log(unit.name);

                if(unitCon.GetHP() <= 0) {
                    effCon.enemyPositionEff(4, enemyVec);
                    Destroy(unit);
                }
            }

            
        }
        Debug.Log(damage);

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
        if(sceneDirector.nowPlayer == 0 && player1Chara.Sp == 100) {
            player1Chara.setSP(-100);
            player1Chara.setPlayer1SpBar();

            player2Chara.setSP(-minSp);
            player2Chara.setPlayer2SpBar();
            
        } else if(sceneDirector.nowPlayer == 1 && player2Chara.Sp == 100) {
            player2Chara.setSP(-100);
            player2Chara.setPlayer2SpBar();

            player1Chara.setSP(-minSp);
            player1Chara.setPlayer1SpBar();
            
        }

        yield break;
    }

    public void getSpMove() {
        StartCoroutine("specialMove1");
    }

    /*必殺技使用方法
     * 別のスクリプトで「もしSpが100％かつプレイヤーアイコンを押されたら」という文を作る
     * 上のif分の中身にgetSpMoveを書く
     * 
     * 
     */
}
