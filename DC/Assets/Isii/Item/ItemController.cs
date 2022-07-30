using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        eff = GameObject.Find("SceneDirector").GetComponent<EffectController>();
        GSD = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
        canItem1_1P = true;
        canItem1_2P = true;
        canItem2_1P = true;
        canItem2_2P = true;
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
                Debug.Log("アイテム開始");
                StartCoroutine("Kaihuku");
                Debug.Log("アイテム仕様");
                canItem1_1P = false;
            }
        }
        if (GSD.nowPlayer == 1)
        {
            if (canItem1_2P)
            {
                Debug.Log("アイテム開始");
                StartCoroutine("Kaihuku");
                Debug.Log("アイテム仕様");
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
                Debug.Log("アイテム2仕様");
                StartCoroutine("DamageItem");
                canItem2_1P = false;
            }
        }
        if (GSD.nowPlayer == 1)
        {
            if (canItem2_2P)
            {
                Debug.Log("アイテム2仕様");
                StartCoroutine("DamageItem");
                canItem2_2P = false;
            }
        }
    }
}
