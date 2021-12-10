using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneDirector : MonoBehaviour
{
    //ゲーム設定
    //タイルの数
    public const int TILE_X = 8;
    public const int TILE_Y = 8;
    //プレイヤーの数
    const int PLAYER_MAX = 2;

    //タイルのプレハブ
    public GameObject[] prefabTile;
    //カーソルのプレハブ
    public GameObject prefabCursor;

    //内部データ
    GameObject[,] tiles;
    UnitController[,] units;

    //ユニットのプレハブ（色ごと）
    public List<GameObject> prefabWhiteUnits;
    public List<GameObject> prefabBlackUnits;

    // 1=ポーン　2＝ルーク 3=ナイト 4=ビショップ 5=クイーン 6=キング
    public int[,] unitType =
    {
        {2,1,0,0,0,0,11,12},
        {3,1,0,0,0,0,11,13},
        {4,1,0,0,0,0,11,14},
        {5,1,0,0,0,0,11,15},
        {6,1,0,0,0,0,11,16},
        {4,1,0,0,0,0,11,14},
        {3,1,0,0,0,0,11,13},
        {2,1,0,0,0,0,11,12},
    };

    //UI関係
    GameObject txtTurnInfo;
    GameObject txtResultInfo;
    GameObject btnApply;
    GameObject btnCancel;

    //選択ユニット
    UnitController selectUnit;

    //移動関連
    List<Vector2Int> movableTiles;
    List<GameObject> cursors;

    //モード
    enum MODE
    {
        NONE,
        CHECK_MATE,
        NORMAL,
        STATUS_UPDATE,
        TURN_CHANGE,
        RESULT
    }

    MODE nowMode,nextMode;
    int nowPlayer;


    // Start is called before the first frame update
    void Start()
    {
        //UIオブジェクト取得
        txtTurnInfo = GameObject.Find("TextTurnInfo");
        txtResultInfo = GameObject.Find("TextResultInfo");
        btnApply = GameObject.Find("ButtonApply");
        btnCancel = GameObject.Find("ButtonCancel");

        //リザルト関係は非表示
        btnApply.SetActive(false);
        btnCancel.SetActive(false);

        //内部データの初期化
        tiles = new GameObject[TILE_X,TILE_Y];
        units = new UnitController[TILE_X, TILE_Y];

        cursors = new List<GameObject>();

        //タイルの生成
        for (int i = 0; i < TILE_X;i++)
        {
            for(int j = 0; j < TILE_Y; j++)
            {
                //タイルとユニットのポジション
                float x = i - TILE_X / 2;
                float y = j - TILE_Y / 2;

                Vector3 pos = new Vector3(x,0,y);

                //作成
                int idx = (i + j) % 2 ;
                GameObject tile = Instantiate(prefabTile[idx],pos,Quaternion.identity);

                tiles[i,j] = tile;

                //ユニットの作成
                int type = unitType[i,j] % 10;//余りがユニットのタイプを表す
                int player = unitType[i,j] / 10;//商がプレイヤーの番号を表す

                //プレハブを取得
                GameObject prefab = getPrefabUnit(player,type);
                GameObject unit = null;
                UnitController ctrl = null;

                if(null == prefab) continue;//ありえない数が来た時処理を終わる

                pos.y += 1.5f;
                unit = Instantiate(prefab);

                //初期化処理
                ctrl = unit.GetComponent<UnitController>();
                ctrl.SetUnit(player, (UnitController.TYPE)type,tile);

                //内部データセット
                units[i,j] = ctrl;
            }
        }
        //モードの初期化
        nowPlayer = -1;
        nowMode = MODE.NONE;
        nextMode = MODE.TURN_CHANGE;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (MODE.CHECK_MATE == nowMode)
        {
            checkMateMode();
        }
        else if (MODE.NORMAL == nowMode)
        {
            normalMode();
        }
        else if (MODE.STATUS_UPDATE == nowMode)
        {
            statusUpdateMode();
        }
        else if (MODE.TURN_CHANGE == nowMode)
        {
            turnChangeMode();
        }
       
        //モード変更
        if (MODE.NONE != nextMode)
        {
            nowMode = nextMode;
            nextMode = MODE.NONE;
        }
    }
    //チェックメイトモード
    void checkMateMode()
    {

        nextMode = MODE.NORMAL;
    }
   
    //ノーマルモード
    void normalMode()
    {
        GameObject tile = null;
        UnitController unit = null;

        //プレイヤーの処理
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //ユニットにも当たり判定があるのでヒットしたすべてのオブジェクト情報を取得
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                //RAYでヒット下オブジェクトでTileと名のついたものを探す
                if (hit.transform.name.Contains("Tile"))
                {
                    tile = hit.transform.gameObject;
                    break;
                }
            }
        }

        //タイルが押されなければ処理しない
        if (null == tile) return;

        //選んだタイルからユニット取得
        Vector2Int tilepos = new Vector2Int(
            (int)tile.transform.position.x + TILE_X / 2,
            (int)tile.transform.position.z + TILE_Y / 2);

        //ユニット
        unit = units[tilepos.x, tilepos.y];

        //ユニット選択
        if (null != unit
            && selectUnit != unit
            && nowPlayer == unit.Player)//ユニットが乗っているそして同じユニットが選択されていないときまた自分のユニットの時
        {
            //移動可能範囲取得
            List<Vector2Int> tiles = getMovableTiles(unit);

            //選択不可
            if (1 > tiles.Count) return;

            movableTiles = tiles;
            setSelectCursors(unit);
        }
        //移動
        else if (null != selectUnit && movableTiles.Contains(tilepos))//ユニットが選択されていた場合&&移動可能範囲に含まれているか
        {
            moveUnit(selectUnit, tilepos);//選択したユニットと移動先
            nextMode = MODE.STATUS_UPDATE;
        }
    }

    //移動後の処理
    void statusUpdateMode()
    {
        //TODO キャスリング

        //TODO アンパッサン

        //TODO プロモーション

        //ターン経過
        foreach(var v in getUnits(nowPlayer))
        {
            v.ProgressTurn();
        }

        //カーソルを消す
        setSelectCursors();

        nextMode = MODE.TURN_CHANGE;
    }

    //ターン変更
    void turnChangeMode()
    {
        //ターンの処理
        nowPlayer = getNextPlayer();

        //Infoの更新
        //txtTurnInfo.GetComponent<Text>().text = "" + (nowPlayer + 1) + "Pの番です";

        nextMode = MODE.CHECK_MATE;
    }

    int getNextPlayer()
    {
        int next = nowPlayer + 1;
        if(PLAYER_MAX <= next)next = 0;

        return next;
    }

    //指定されたプレイヤー番号のユニットを取得する
    List<UnitController>getUnits(int player = -1)
    {
        List<UnitController> ret = new List<UnitController>();

        foreach(var v in units)
        {
            if(null == v)continue;

            if(player == v.Player)
            {
                ret.Add(v);
            }
            else if(0 > player)
            {
                ret.Add(v);
            }


        }

        return ret;
    }

    //移動可能範囲取得
    List<Vector2Int> getMovableTiles(UnitController unit)
    {
        //通常移動可能範囲を返す
        return unit.GetMovableTiles(units);
    }

    //選択時の関数
    void setSelectCursors(UnitController unit=null,bool setunit = true)//選ばれたユニット&&選ばれたユニットを選択状態にするか
    {
        //カーソルの解除
        foreach(var v in cursors)
        {
            Destroy(v);
        }

        //選択ユニットの非選択状態
        if( null != selectUnit)
        {
            selectUnit.SelectUnit(false);//選択されていない状態に
            selectUnit = null;//中身も消す
        }

        //何もセットされないなら終了
        if(null == unit) return;

        //カーソル作成
        foreach(var v in getMovableTiles(unit))
        {
            Vector3 pos = tiles[v.x, v.y].transform.position;
            pos.y += 0.51f; 

            GameObject obj = Instantiate(prefabCursor,pos,Quaternion.identity);
            cursors.Add(obj);
        }


        //選択状態
        if (setunit)
        {
            selectUnit = unit;//中身にユニットを入れる
            selectUnit.SelectUnit();//フラグを立てる
        }
    }

    void moveUnit(UnitController unit, Vector2Int tilepos)
    {
        //現在地
        Vector2Int unitpos = unit.Pos;

        //誰かいたら消す
        if (null != units[tilepos.x,tilepos.y])
        {
            Destroy(units[tilepos.x, tilepos.y].gameObject);
        }

        //新しい場所に移動
        unit.MoveUnit(tiles[tilepos.x,tilepos.y]);

        //内部データ更新（元の場所）
        units[unitpos.x, unitpos.y] = null;

        //内部データ更新（新しい場所）
        units[tilepos.x, tilepos.y] = unit;
    }
    //ユニットのプレハブを取得
    GameObject getPrefabUnit(int player,int type)
    {
        int idx = type -1;

        if(0 > idx) return null;

        GameObject prefab = prefabWhiteUnits[idx];
        if(1 == player) prefab = prefabBlackUnits[idx];

        return prefab;
    }
}
