using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    GameSceneDirector GSD;
    UnitController unitCon;
    GameObject[] player;
    public int recovery = 3;//回復量
    public int hp = 0;
    public int maxHp = 0;

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
}
