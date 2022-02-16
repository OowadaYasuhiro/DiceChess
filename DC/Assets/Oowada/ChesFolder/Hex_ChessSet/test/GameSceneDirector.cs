using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameSceneDirector : MonoBehaviour
{
    public bool debug = true;
    float debtimer;

    // ゲーム設定
    public const int TILE_X = 8;
    public const int TILE_Y = 8;
    const int PLAYER_MAX = 2;

    // タイルのプレハブ
    public GameObject[] prefabTile;
    // カーソルのプレハブ
    public GameObject prefabCursor;

    // 内部データ
    GameObject[,] tiles;
    UnitController[,] units;

    // ユニットのプレハブ（色ごと）
    public List<GameObject> prefabWhiteUnits;
    public List<GameObject> prefabBlackUnits;

    // 1 = ポーン 2 = ルーク 3 = ナイト 4 = ビショップ 5 = クイーン 6 = キング
    public int[,] unitType =
    {
        { 2, 1, 0, 0, 0, 0, 11, 12 },
        { 3, 1, 0, 0, 0, 0, 11, 13 },
        { 4, 1, 0, 0, 0, 0, 11, 14 },
        { 5, 1, 0, 0, 0, 0, 11, 15 },
        { 6, 1, 0, 0, 0, 0, 11, 16 },
        { 4, 1, 0, 0, 0, 0, 11, 14 },
        { 3, 1, 0, 0, 0, 0, 11, 13 },
        { 2, 1, 0, 0, 0, 0, 11, 12 },
    };

    // UI関連
    GameObject txtTurnInfo;
    GameObject txtResultInfo;
    GameObject btnApply;
    GameObject btnCancel;

    // 選択ユニット
    UnitController selectUnit;

    // 移動関連
    List<Vector2Int> movableTiles;
    List<GameObject> cursors;

    // モード
    enum MODE
    {
        NONE,
        CHECK_MATE,
        NORMAL,
        BATTLE_SET,
        DICE,
        ATTACK_TIME,
        STATUS_UPDATE,
        TURN_CHANGE,
        RESULT,
        FIRST_TURN
    }

    MODE nowMode, nextMode;
    int nowPlayer;

    // 前回ユニット削除から経過ターン　50以上で引き分け
    int prevDestroyTurn;

    // 前回の盤面
    List<UnitController[,]> prevUnits;

    //戦闘開始合図のアニメ(仮)
    Animator panelAnim;
    Animator textAnim;
    GameObject AttackButton;
    GameObject ATKText;
    GameObject eneUnit;
    Text aText;
    //攻撃ダイスを振ったかどうかのチェック    
    bool diceCheck = false;
    bool pushAButton = false;
    int Hp = 0;
    bool canAtk = true;

    // Start is called before the first frame update
    void Start()
    {
        // UIオブジェクト取得
        txtTurnInfo = GameObject.Find("TextTurnInfo");
        txtResultInfo = GameObject.Find("TextResultInfo");
        btnApply = GameObject.Find("ButtonApply");
        btnCancel = GameObject.Find("ButtonCancel");

        // 戦闘開始UIオブジェクト取得
        panelAnim = GameObject.Find("AttackBackPanel").GetComponent<Animator>();
        textAnim = GameObject.Find("ATTACK PHASE").GetComponent<Animator>();
        AttackButton = GameObject.Find("AttackDiceButton");
        ATKText = GameObject.Find("AttackDameText");
        aText = ATKText.GetComponent<Text>();

        // リザルト関連は非表示
        btnApply.SetActive(false);
        btnCancel.SetActive(false);

        AttackButton.SetActive(false);
        ATKText.SetActive(false);

        // 内部データの初期化
        tiles = new GameObject[TILE_X, TILE_Y];
        units = new UnitController[TILE_X, TILE_Y];
        cursors = new List<GameObject>();
        prevUnits = new List<UnitController[,]>();

        for (int i = 0; i < TILE_X; i++)
        {
            for (int j = 0; j < TILE_Y; j++)
            {
                // タイルとユニットのポジション
                float x = i - TILE_X / 2;
                float y = j - TILE_Y / 2;

                Vector3 pos = new Vector3(x, 0, y);

                // 作成
                int idx = (i + j) % 2;
                GameObject tile = Instantiate(prefabTile[idx], pos, Quaternion.identity);

                tiles[i, j] = tile;

                // ユニットの作成
                int type = unitType[i, j] % 10;
                int player = unitType[i, j] / 10;
                int hp = unitType[i, j] %10 * 3;

                GameObject prefab = getPrefabUnit(player, type);
                GameObject unit = null;
                UnitController ctrl = null;

                if (null == prefab) continue;

                pos.y += 1.5f;
                unit = Instantiate(prefab);

                // 初期化処理
                ctrl = unit.GetComponent<UnitController>();
                ctrl.SetUnit(player, (UnitController.TYPE)type, tile);

                // 内部データセット
                units[i, j] = ctrl;
            }
        }

        nowPlayer = -1;
        nowMode = MODE.NONE;
        //nextMode = MODE.TURN_CHANGE;
        nextMode = MODE.FIRST_TURN;
    }

    // Update is called once per frame
    void Update()
    {
        if(MODE.CHECK_MATE == nowMode)
        {
            checkMateMode();
        }
        else if(MODE.NORMAL == nowMode)
        {
            normalMode();
        }
        else if(MODE.BATTLE_SET == nowMode)
        {
            battleMode();
        }
        else if(MODE.STATUS_UPDATE == nowMode)
        {
            statusUpdateMode();
        }
        else if(MODE.TURN_CHANGE == nowMode)
        {
            turnChangeMode();
        }
        else if(MODE.RESULT == nowMode)
        {
            if (debug)
            {
                debtimer += Time.deltaTime;
                if(2 < debtimer)
                {
                    print("勝敗 "+ txtResultInfo.GetComponent<Text>().text);
                    Retry();
                }
            }
        }
        else if(MODE.FIRST_TURN == nowMode)
        {
            firstTurn();
        }

        // モード変更
        if(MODE.NONE != nextMode)
        {
            nowMode = nextMode;
            nextMode = MODE.NONE;
        }
    }

    // チェックメイトモード
    void checkMateMode()
    {
        // 次のモード
        nextMode = MODE.NORMAL;
        Text info = txtResultInfo.GetComponent<Text>();
        info.text = "";

        // --------------------
        // ドローのチェック（簡易版）
        // --------------------

        // 1 vs 1になったら引き分け
        if ( 3 > getUnits().Count)
        {
            info.text = "チェックメイトできないので\n引き分け";
            nextMode = MODE.RESULT;
        }

        // 50ターンの誰も削除されなければドロー
        if( 50 < prevDestroyTurn)
        {
            info.text = "50ターンルールで\n引き分け";
            nextMode = MODE.RESULT;
        }

        // 3回同じ盤面になったらドロー
        int prevcount = 0;

        foreach (var v in prevUnits)
        {
            bool check = true;

            for (int i = 0; i < v.GetLength(0); i++)
            {
                for (int j = 0; j < v.GetLength(1); j++)
                {
                    if (v[i, j] != units[i, j]) check = false;

                }
            }

            if (check) prevcount++;
        }

        // 3回続いたか
        if( 2 < prevcount)
        {
            info.text = "同じ盤面が続いたので\n引き分け";
            nextMode = MODE.RESULT;
        }

        // --------------------
        // チェックのチェック
        // --------------------
        // 今回のプレイヤーのキング
        UnitController target = getUnit(nowPlayer, UnitController.TYPE.KING);
        // チェックしているユニット
        List<UnitController> checkunits = GetCheckUnits(units, nowPlayer);
        // チェック状態セット
        bool ischeck = (0 < checkunits.Count) ? true : false;

        if( null != target)
        {
            target.SetCheckStatus(ischeck);
        }

        // ゲームが続くならチェックと表示
        if( ischeck && MODE.RESULT != nextMode)
        {
            info.text = "チェック！！";
        }

        // --------------------
        // 移動可能範囲を調べる
        // --------------------
        int tilecount = 0;

        // 移動可能範囲をカウント
        foreach(var v in getUnits(nowPlayer))
        {
            tilecount += getMovableTiles(v).Count;
        }

        // 動かせない
        if( 1 > tilecount )
        {
            info.text = "ステイルメイト\n" + "引き分け";

            if (ischeck)
            {
                info.text = "チェックメイト\n" + (getNextPlayer() + 1) + "Pの勝ち！！";
            }

            nextMode = MODE.RESULT;
        }

        // 今回の盤面をコピー
        UnitController[,] copyunits = GetCopyArray(units);
        prevUnits.Add(copyunits);

        // 次のモードの準備
        if(MODE.RESULT == nextMode)
        {
            btnApply.SetActive(true);
            btnCancel.SetActive(true);
        }
    }

    // ノーマルモード
    void normalMode()
    {
        GameObject tile = null;
        UnitController unit = null;

        //ここで駒の動きを変える
        int foge = daise();

        for(int i = 0; i < unitType.GetLength(0); i++)
        {
            for(int j = 0; j < unitType.GetLength(0); j++)
            {
                if(unitType[i, j]/10 == 0)
                {
                    unitType[i, j] = foge;
                }
                else if(unitType[i, j] / 10 == 1)
                {
                    unitType[i, j] = foge+10;
                }
                else
                {
                    unitType[i, j] = 0;
                }
            }
        }

        // プレイヤーの処理
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ユニットにも当たり判定があるのでヒットした全てのオブジェクト情報を取得
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                if (hit.transform.name.Contains("Tile"))
                {
                    tile = hit.transform.gameObject;
                    break;
                }
            }
        }
        

        // CPUの処理
        /*while( TitleSceneDirector.PlayerCount <= nowPlayer
                && (null == selectUnit || null == tile ) )
        {
            // ユニット選択
            if( null == selectUnit)
            {
                // 今回の全ユニット
                List<UnitController> tmpunits = getUnits(nowPlayer);
                // ランダムで1体選ぶ
                UnitController tmp = tmpunits[Random.Range(0, tmpunits.Count)];
                // ユニットがいるタイルを選択
                tile = tiles[tmp.Pos.x, tmp.Pos.y];

                // 一旦処理へ流す
                break;
            }

            // ここから下はselectUnitが入った状態でくる
            if( 1 > movableTiles.Count)
            {
                setSelectCursors();
                break;
            }

            // 移動可能範囲があればランダムで移動
            int rnd = Random.Range(0, movableTiles.Count);
            tile = tiles[movableTiles[rnd].x, movableTiles[rnd].y];
        }*/

        // タイルが押されていなければ処理しない
        if (null == tile) return;

        // 選んだタイルからユニット取得
        Vector2Int tilepos = new Vector2Int(
            (int)tile.transform.position.x + TILE_X / 2,
            (int)tile.transform.position.z + TILE_Y / 2);

        // ユニット
        unit = units[tilepos.x, tilepos.y];

        // ユニット選択
        if (null != unit
            && selectUnit != unit
            && nowPlayer == unit.Player )
        {
            // 移動可能範囲を取得
            List<Vector2Int> tiles = getMovableTiles(unit);

            // 選択不可
            if (1 > tiles.Count) return;

            movableTiles = tiles;
            setSelectCursors(unit);
        }
        // 移動
        else if (null != selectUnit && movableTiles.Contains(tilepos))
        {
            
            
            moveUnit(selectUnit, tilepos);
            //nextMode = MODE.STATUS_UPDATE;
        }
        // 移動範囲だけ見られる
        else if( null != unit && nowPlayer != unit.Player)
        {
            setSelectCursors(unit, false);
        }
        // 選択解除
        else
        {
            setSelectCursors();
        }
    }

    //セレクトしたタイルにいる相手のユニットを保存する
    public void enemyUnit(UnitController unit, Vector2Int tilepos) {



    }

    //戦闘処理
    void battleMode()
    {

    }

    // 移動後の処理
    void statusUpdateMode()
    {
        /*// キャスリング
        if(selectUnit.Status.Contains(UnitController.STATUS.QSIDE_CASTLING) )
        {
            // 左端のルーク
            UnitController unit = units[0, selectUnit.Pos.y];
            Vector2Int tile = new Vector2Int(selectUnit.Pos.x + 1, selectUnit.Pos.y);

            moveUnit(unit, tile);
        }
        else if (selectUnit.Status.Contains(UnitController.STATUS.KSIDE_CASTLING))
        {
            // 右端のルーク
            UnitController unit = units[TILE_X-1, selectUnit.Pos.y];
            Vector2Int tile = new Vector2Int(selectUnit.Pos.x - 1, selectUnit.Pos.y);

            moveUnit(unit, tile);
        }

        // アンパッサンとプロモーション
        if (UnitController.TYPE.PAWN == selectUnit.Type)
        {
            foreach (var v in getUnits(getNextPlayer()))
            {
                if (!v.Status.Contains(UnitController.STATUS.EN_PASSANT)) continue;

                // 置いた場所がアンパッサン対象か
                if(selectUnit.Pos == v.OldPos)
                {
                    Destroy(v.gameObject);
                }
            }

            // プロモーション
            int py = TILE_Y - 1;
            if (selectUnit.Player == 1) py = 0;

            // 端に到達
            if( py == selectUnit.Pos.y )
            {
                // クイーン固定
                GameObject prefab = getPrefabUnit(nowPlayer, (int)UnitController.TYPE.QUEEN);
                UnitController unit = Instantiate(prefab).GetComponent<UnitController>();
                GameObject tile = tiles[selectUnit.Pos.x, selectUnit.Pos.y];

                unit.SetUnit(selectUnit.Player, UnitController.TYPE.QUEEN, tile);
                moveUnit(unit, new Vector2Int(selectUnit.Pos.x, selectUnit.Pos.y));
            }
        }*/

        // ターン経過
        foreach (var v in getUnits(nowPlayer))
        {
            v.ProgressTurn();
        }
        canAtk = true;

        // カーソル
        setSelectCursors();

        nextMode = MODE.TURN_CHANGE;
    }

    // ターン変更
    void turnChangeMode()
    {
        // ターンの処理
        nowPlayer = getNextPlayer();

        // Infoの更新
        txtTurnInfo.GetComponent<Text>().text = "" + (nowPlayer + 1) + "Pの番です";

        // 経過ターン（１P側にきたら+1）
        if( 0 == nowPlayer)
        {
            prevDestroyTurn++;
        }

        nextMode = MODE.CHECK_MATE;
    }

    int getNextPlayer()
    {
        int next = nowPlayer + 1;
        if (PLAYER_MAX <= next) next = 0;

        return next;
    }

    // 指定のユニットを取得する
    UnitController getUnit(int player, UnitController.TYPE type)
    {
        foreach (var v in getUnits(player))
        {
            if (player != v.Player) continue;
            if(type == v.Type ) return v;
        }

        return null;
    }

    // 指定されたプレイヤー番号のユニットを取得する
    List<UnitController> getUnits(int player = -1)
    {
        List<UnitController> ret = new List<UnitController>();

        foreach (var v in units)
        {
            if (null == v) continue;

            if(player == v.Player)
            {
                ret.Add(v);
            }
            else if( 0 > player)
            {
                ret.Add(v);
            }

        }

        return ret;
    }

    // 指定された配列をコピーして返す
    public static UnitController[,] GetCopyArray(UnitController[,] org)
    {
        UnitController[,] ret = new UnitController[org.GetLength(0), org.GetLength(1)];
        Array.Copy(org, ret, org.Length);
        return ret;
    }

    // 移動可能範囲取得
    List<Vector2Int> getMovableTiles(UnitController unit)
    {
        // そこをどいたらチェックされてしまうか
        UnitController[,] copyunits = GetCopyArray(units);
        copyunits[unit.Pos.x, unit.Pos.y] = null;

        // チェックされるかどうか
        List<UnitController> checkunits = GetCheckUnits(copyunits, unit.Player);

        // チェックを回避できるタイルを返す
        if( 0 < checkunits.Count)
        {
            // 移動可能範囲
            List<Vector2Int> ret = new List<Vector2Int>();

            // 移動可能範囲
            List<Vector2Int> movetiles = unit.GetMovableTiles(units);

            // 移動してみる
            foreach(var v in movetiles)
            {
                // 移動した状態を作る
                UnitController[,] copyunits2 = GetCopyArray(units);
                copyunits2[unit.Pos.x, unit.Pos.y] = null;
                copyunits2[v.x, v.y] = unit;

                int checkcount = GetCheckUnits(copyunits2, unit.Player, false).Count;

                if (1 > checkcount) ret.Add(v);
            }

            return ret;
        }

        // 通常移動可能範囲を返す
        return unit.GetMovableTiles(units);
    }

    // 選択時の関数
    void setSelectCursors(UnitController unit=null, bool setunit = true)
    {
        // カーソル解除
        foreach (var v in cursors)
        {
            Destroy(v);
        }

        // 選択ユニットの非選択状態
        if( null != selectUnit)
        {
            selectUnit.SelectUnit(false);
            selectUnit = null;
        }

        // なにもセットされないなら終了
        if (null == unit) return;

        // カーソル作成
        foreach(var v in getMovableTiles(unit))
        {
            Vector3 pos = tiles[v.x, v.y].transform.position;
            pos.y += 0.51f;

            GameObject obj = Instantiate(prefabCursor, pos, Quaternion.identity);
            cursors.Add(obj);
        }

        // 選択状態
        if(setunit)
        {
            selectUnit = unit;
            selectUnit.SelectUnit();
        }
    }

    // ユニット移動
    public void moveUnit(UnitController unit, Vector2Int tilepos)
    {
        // 現在地
        Vector2Int unitpos = unit.Pos;
        

        // 誰かいたら消す ここが攻撃できるところ
        if(null != units[tilepos.x, tilepos.y])
        {
            canAtk = false;
            
            Hp = units[tilepos.x, tilepos.y].GetHP();
            int PU = 0;
            

            panelAnim.SetTrigger("in");
            textAnim.SetTrigger("in");

            Invoke("battleSetMode", 1f);


            pushAButton = true;
            //戦闘モードに移行
            nextMode = MODE.BATTLE_SET;
            //TODO　攻撃ダイスを振り攻撃値を決める

            //TODO　敵の駒のHPを攻撃値をの値だけ削る
            /*if(pushAButton == true) {

                Debug.Log("ボタン押した");
                diceTime();
                
                

                
            }*/
            //units[tilepos.x, tilepos.y].gameObject.GetComponent<UnitController>();//選択された敵の駒を把握してUnitControllerにあるSetHPをよびHPを減らす
            if(pushAButton == true) {
                PU = diceTime();

                Hp = Hp - PU;
                


                if(Hp <= 0) {
                    nextMode = MODE.STATUS_UPDATE;
                    

                    Destroy(units[tilepos.x, tilepos.y].gameObject);//敵の駒のHPをUnitControllerのGetHPからとりif文で分岐
                    prevDestroyTurn = 0;

                    panelAnim.SetTrigger("out");
                    AttackButton.SetActive(false);
                    ATKText.SetActive(false);
                    pushAButton = false;
                    diceCheck = false;

                    // 新しい場所へ移動
                    unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

                    // 内部データ更新（元の場所）
                    units[unitpos.x, unitpos.y] = null;

                    // 内部データ更新（新しい場所）
                    units[tilepos.x, tilepos.y] = unit;


                    
                } else {
                    nextMode = MODE.STATUS_UPDATE;

                    //自分の攻撃したコマが「ポーン、ナイト、キング」だったら移動しないでその場にとどまる

                    //自分の攻撃したコマが上のコマ以外だったら相手の駒の目の前でとどまる


                    panelAnim.SetTrigger("out");
                    AttackButton.SetActive(false);
                    ATKText.SetActive(false);
                    pushAButton = false;
                    diceCheck = false;

                    // 新しい場所へ移動
                    unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

                    // 内部データ更新（元の場所）
                    units[unitpos.x, unitpos.y] = null;

                    // 内部データ更新（新しい場所）
                    units[tilepos.x, tilepos.y] = unit;

                    
                }
            }
            
            //TODO　駒のHPが0になった時その駒を消す
            
            
            //TODO　場所を乗っ取る
                                                            //カッコ外でやってるので特に付け加えることは無い
            //TODO　選択したマスから１マス前に駒を置く//unitposのＸ，Ｙを見てどうにかしようとしてる
            //unitpos = unitType[tilepos.x, tilepos.y] % 10;
            //TODO　STATUS_UPDATEに移行
                                                            //いらないかも
            //１マス前に置いたら　tilepos をかえ内部データの更新
                                                            //内部データ更新は下でやってくれているのでtileposを帰る
        }
        else{

            nextMode = MODE.STATUS_UPDATE;

            // 新しい場所へ移動
            unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

            // 内部データ更新（元の場所）
            units[unitpos.x, unitpos.y] = null;

            // 内部データ更新（新しい場所）
            units[tilepos.x, tilepos.y] = unit;

            
        }

        
    }

    public void battleSetMode() {
        
        AttackButton.SetActive(true);
        


    }

    //ダイス回すボタンクリック
    public void pushATKButton() {
        pushAButton = true;
        Debug.Log("ボタン押した");
    }

    //試しダイス
    int rnd;
    public int diceTime() {
        rnd = Random.Range(1, 11);
        diceCheck = true;
        
        ATKText.SetActive(true);
        aText.text = rnd.ToString();

        return rnd;
    }




    // ユニットのプレハブを取得
    GameObject getPrefabUnit(int player, int type)
    {
        int idx = type - 1;

        if (0 > idx) return null;

        GameObject prefab = prefabWhiteUnits[idx];
        if( 1 == player ) prefab = prefabBlackUnits[idx];

        return prefab;
    }

    // 指定された配置でチェックされているかチェック
    static public List<UnitController> GetCheckUnits(UnitController[,] units, int player, bool checkking = true)
    {
        List<UnitController> ret = new List<UnitController>();

        foreach (var v in units)
        {
            if (null == v) continue;
            if (player == v.Player) continue;

            // 敵1体の移動可能範囲
            List<Vector2Int> enemytiles = v.GetMovableTiles(units, checkking);

            foreach (var t in enemytiles)
            {
                if (null == units[t.x, t.y]) continue;

                if(UnitController.TYPE.KING == units[t.x, t.y].Type)
                {
                    ret.Add(v);
                }
            }
        }

        return ret;
    }


    void attack(Vector2Int tilepos)
    {
        //TODO　攻撃ダイスを振り攻撃値を決める
        //TODO　敵の駒のHPを攻撃値をの値だけ削る
        //TODO　駒のHPが0になった時その駒を消す
        //TODO　場所を乗っ取
        //TODO　STATUS_UPDATEに移行

    }

    public void firstTurn()
    {
        int rnd = Random.Range(0,2);

        if(rnd == 0)
        {
            nowPlayer = 0;
        }
        else if(rnd == 1)
        {
            nowPlayer = 1;
        }

        nextMode = MODE.TURN_CHANGE;
      }

   

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
    }

     public int daise()
    {
        return 1;
    }
}
