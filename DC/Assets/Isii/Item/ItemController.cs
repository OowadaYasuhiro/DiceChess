using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    GameSceneDirector GSD;
    UnitController unitCon;
    CharacterStatus status;
    GameObject[] player;
    [SerializeField]
    private GameObject[] chara;
    public int recovery = 3;//回復量
    public int spRecovery = 30;//sp回復量
    public int hp = 0;
    public int maxHp = 0;
    public int move = 2;

    // Start is called before the first frame update
    void Start()
    {
        GSD = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
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
    }

    //回復する
    public IEnumerator Kaihuku()
    {
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
        //自分の番に使った時damageflagを1増やす
        if(GSD.nowPlayer == 0)
        {
            GSD.damageflag += 1;
        }
        //相手の番に使った時damageflagを2増やす
        else if (GSD.nowPlayer == 1)
        {
            GSD.damageflag += 2;
        }
        Debug.Log(GSD.damageflag);
        yield break;
    }

    public IEnumerator SpItem()
    {
        //nowPlayerが0の時は自分の番
        if (GSD.nowPlayer == 0)
        {
            status = chara[0].GetComponent<CharacterStatus>();
            status.setSP(spRecovery);
        }
        //nowPlayerが1の時は相手の番
        else if (GSD.nowPlayer == 1)
        {
            status = chara[1].GetComponent<CharacterStatus>();
            status.setSP(spRecovery);
        }
        Debug.Log(GSD.nowPlayer + "pに" + spRecovery + "したのでspの合計値が" + status.sp);
        yield break;
    }

    public IEnumerator Twice()
    {
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

    public void UseItem1()
    {
        Debug.Log("アイテム開始");
        StartCoroutine("Kaihuku");
        Debug.Log("アイテム仕様");
    }
    public void UseItem2()
    {
        Debug.Log("アイテム2仕様");
        StartCoroutine("DamageItem");

    }
}
