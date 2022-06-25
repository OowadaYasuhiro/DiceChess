using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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

    //SEのためのオブジェクト
    [SerializeField] private GameObject SeObject;
    

    // 内部データ
    GameObject[,] tiles;
    UnitController[,] units;

    // ユニットのプレハブ（色ごと）
    public List<GameObject> prefabWhiteUnits;
    public List<GameObject> prefabBlackUnits;

    //バトルfalse
    bool battleEnd = false;

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
    GameObject ui_Player1;        //もしかしたらプレイヤーUI系をプレハブにするかもしれないのでそれ用のモノ
    GameObject ui_BackGroundP1;
    GameObject ui_Player2;
    GameObject ui_BackGroundP2;
    CharacterStatus player1Chara; //プレイヤー1キャラのスクリプトを読み込む(読み込み対象変わるかも)
    CharacterStatus player2Chara; //同じく変わるかも
    [SerializeField] private Text hpText; //HPのテキスト
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image paseImage;
    GameObject turnEndCursor;
    GameObject charaTextPanel;

    //コントローラーのため
    [SerializeField] EventSystem eventSystem;
    GameObject selectedObj;
    private float controlTimer = 0.0f;
    [SerializeField] private float DelayTime = 0.15f;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private GameObject itemText1Panel;
    [SerializeField] private GameObject itemText2Panel;
    [SerializeField] private Button endButton;
    [SerializeField] private GameObject mainCursor;

    // 選択ユニット
    UnitController selectUnit;

    //エフェクトコントローラー
    EffectController effCon;

    // 移動関連
    List<Vector2Int> movableTiles;
    List<GameObject> cursors;
    bool canMoveCounter = true; 

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
    public int nowPlayer;

    public int damageFlag = 0;

    //ターンエンドフラグ
    public bool moved = false;

    // 前回ユニット削除から経過ターン　50以上で引き分け
    int prevDestroyTurn;

    // 前回の盤面
    List<UnitController[,]> prevUnits;

    //戦闘開始合図のアニメ(仮)
    Animator panelAnim;
    Animator textAnim;
    GameObject AttackButton;
    [SerializeField]private Button attackDiceButton;
    GameObject ATKText;
    GameObject eneUnit;
    Text aText;


    //攻撃ダイスを振ったかどうかのチェック    
    public bool diceCheck = false;
    public bool pushAButton = false;
    int Hp = 0;
    //タイルを触れるようにするためのbool
    bool invalidTile = true;

    int turnNum = 0;
    int turnCount = 0;
    Text turnText;

    //先攻後攻に使うランダム
    int turnRnd;
    //turnChangeを最初に行わないためのbool
    bool firstTurnFlag;

    // Start is called before the first frame update
    void Start()
    {
        turnRnd = Random.Range(0, 2);
        firstTurnFlag = true;
        turnNum = 1;

        // UIオブジェクト取得
        txtTurnInfo = GameObject.Find("TextTurnInfo");
        txtResultInfo = GameObject.Find("TextResultInfo");
        btnApply = GameObject.Find("ButtonApply");
        btnCancel = GameObject.Find("ButtonCancel");
        turnEndCursor = GameObject.Find("ImageCanvas_Player1/TurnEndButton/TurnEndCursor");
        charaTextPanel = GameObject.Find("ImageCanvas_Player1/CharaImage1P/CharaTextPanel");

        // 戦闘開始UIオブジェクト取得
        panelAnim = GameObject.Find("AttackBackPanel").GetComponent<Animator>();
        textAnim = GameObject.Find("ATTACK PHASE").GetComponent<Animator>();
        AttackButton = GameObject.Find("AttackDiceButton");
        ATKText = GameObject.Find("AttackDameText");
        aText = ATKText.GetComponent<Text>();
        turnText = GameObject.Find("TurnText").GetComponent<Text>();
        // エフェクトコントローラー
        effCon = GameObject.Find("SceneDirector").GetComponent<EffectController>();

        //プレイヤー1取得
        player1Chara = GameObject.Find("Player1Chara").GetComponent<CharacterStatus>();
        player2Chara = GameObject.Find("Player2Chara").GetComponent<CharacterStatus>();

        //コントロールについての初期化
        itemText1Panel.SetActive(false);
        itemText2Panel.SetActive(false);
        turnEndCursor.SetActive(false);
        charaTextPanel.SetActive(false);

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
            firstTurnFlag = false;
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
        //最初のターンのみ実行するenem
        else if(MODE.FIRST_TURN == nowMode)
        {
            StartCoroutine("firstTurn");
            firstTurnFlag = true;
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
        if( 20 < prevcount)
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

        if(invalidTile == true) {
            // プレイヤーの処理
            if(Input.GetMouseButtonUp(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ユニットにも当たり判定があるのでヒットした全てのオブジェクト情報を取得
                foreach(RaycastHit hit in Physics.RaycastAll(ray)) {
                    if(hit.transform.name.Contains("Tile")) {
                        tile = hit.transform.gameObject;
                        break;
                    }
                }
            }
        }


        //コントローラーでのプレイヤー処理
        selectedObj = EventSystem.current.currentSelectedGameObject;
        // transformを取得
        Transform myTransform = this.transform;
        // 座標を取得
        Vector3 pos = myTransform.position;

        float lsh = Input.GetAxis("L_Stick_H");
        float lsv = Input.GetAxis("L_Stick_V");

        if (controlTimer < Time.time){
            if (lsh < 0) {
                if (pos.x > -4){//左に移動
                    pos.x -= 1; myTransform.position = pos; controlTimer = Time.time + DelayTime;
                    EventSystem.current.SetSelectedGameObject(null);
                    mainCursor.SetActive(true);
                    turnEndCursor.SetActive(false);
                }
                else{
                    player1Button.Select();
                    mainCursor.SetActive(false);
                    charaTextPanel.SetActive(true);
                }
            }
            if (lsh > 0){
                if (pos.x < 3){//右に移動
                    pos.x += 1; myTransform.position = pos; controlTimer = Time.time + DelayTime;
                    EventSystem.current.SetSelectedGameObject(null);
                    mainCursor.SetActive(true);
                    charaTextPanel.SetActive(false);
                }
                else{
                    endButton.Select();
                    mainCursor.SetActive(false);
                    turnEndCursor.SetActive(true);
                }
            } 
        }
        if (controlTimer < Time.time){
            if (lsv < 0) { 
                if(pos.z < 3){//上に移動
                    pos.z += 1; myTransform.position = pos; controlTimer = Time.time + DelayTime;
                    EventSystem.current.SetSelectedGameObject(null);
                    itemText1Panel.SetActive(false);
                    itemText2Panel.SetActive(false);
                    mainCursor.SetActive(true);
                }
            }
            if (lsv > 0) { 
                if(pos.z > -4){//下に移動
                    pos.z -= 1; myTransform.position = pos; controlTimer = Time.time + DelayTime; 
                }
                else if(pos.z < -3 && pos.x < 0){
                    item1Button.Select();
                    itemText1Panel.SetActive(true);
                    mainCursor.SetActive(false);
                }
                else if(pos.z < -3 && pos.x >= 0){
                    item2Button.Select();
                    itemText2Panel.SetActive(true);
                    mainCursor.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("joystick 2 button 0") || Input.GetKey(KeyCode.Space))
        {
            // ユニットにも当たり判定があるのでヒットした全てのオブジェクト情報を取得
            foreach (RaycastHit hit in Physics.RaycastAll(transform.position, new Vector3(myTransform.position.x, -6, myTransform.position.y)))
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
        if(unit != null)
        {
            hpText.text = (unit.GetHP() + "/" + unit.GetMaxHp());
            hpSlider.value = (float)unit.GetHP() / unit.GetMaxHp();
            PaseImageController pI;
            pI = paseImage.GetComponent<PaseImageController>();
            if (unit.GetTYPE() == 1)//ポーンのとき
            {
                if(unit.GetPlayer() == 0){ pI.PaceCanChanger(1, 0); }
                if(unit.GetPlayer() == 1){ pI.PaceCanChanger(1, 1); }
            }
            else if (unit.GetTYPE() == 2)//ルークのとき
            {
                if (unit.GetPlayer() == 0) { pI.PaceCanChanger(2, 0); }
                if (unit.GetPlayer() == 1) { pI.PaceCanChanger(2, 1); }
            }
            if (unit.GetTYPE() == 3)//ナイトのとき
            {
                if (unit.GetPlayer() == 0) { pI.PaceCanChanger(3, 0); }
                if (unit.GetPlayer() == 1) { pI.PaceCanChanger(3, 1); }
            }
            if (unit.GetTYPE() == 4)//ビショップのとき
            {
                if (unit.GetPlayer() == 0) { pI.PaceCanChanger(4, 0); }
                if (unit.GetPlayer() == 1) { pI.PaceCanChanger(4, 1); }
            }
            if (unit.GetTYPE() == 5)//クイーンのとき
            {
                if (unit.GetPlayer() == 0) { pI.PaceCanChanger(5, 0); }
                if (unit.GetPlayer() == 1) { pI.PaceCanChanger(5, 1); }
            }
            if (unit.GetTYPE() == 6)//キングのとき
            {
                if (unit.GetPlayer() == 0) { pI.PaceCanChanger(6, 0); }
                if (unit.GetPlayer() == 1) { pI.PaceCanChanger(6, 1); }
            }
        }

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
            if(canMoveCounter == true)
            {
                StartCoroutine(moveUnit(selectUnit, tilepos));
            }
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


    //戦闘処理
    void battleMode()
    {
        //battleEndがtrueになったらstatusUpdateMode()に移行する
        if(battleEnd == true) {
            nextMode = MODE.STATUS_UPDATE;
        }

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
        //タイルを触れるようにする
        invalidTile = true;

        // カーソル
        setSelectCursors();

        battleEnd = false;

        nextMode = MODE.TURN_CHANGE;
    }

    // ターン変更
    void turnChangeMode()
    {
        //最初のターンじゃない時実行する
        if(firstTurnFlag == false) {
            //先攻後攻のアニメ表示
            StartCoroutine("turnChangeAnim");
            //ターンカウントを+1する
            turnCount++;
        }
        //駒の移動処理の制御
        canMoveCounter = true;

        // ターンの処理
        nowPlayer = getNextPlayer();

        // Infoの更新
        txtTurnInfo.GetComponent<Text>().text = "" + (nowPlayer + 1) + "Pの番です";

        // 経過ターン（１P側にきたら+1）
        if( 0 == nowPlayer)
        {
            prevDestroyTurn++;
        }
        //ターン数表示(カウントが2になったら表示を更新する)
        if(turnCount == 2) {
            turnCount = 0;
            turnNum++;
        }
        turnText.text = turnNum.ToString();
        
        

        nextMode = MODE.CHECK_MATE;
    }

    int getNextPlayer()
    {
        int next = nowPlayer + 1;
        if (PLAYER_MAX <= next) next = 0;

        return next;
    }

    IEnumerator turnChangeAnim() {
        //ターンチェンジアニメ取得
        Animator turnAnim = GameObject.Find("TurnChangeImage").GetComponent<Animator>();

        invalidTile = false;
        if(nowPlayer == 1) {
            turnAnim.SetTrigger("EnemyOn");
            
        } else if(nowPlayer == 0) {
            turnAnim.SetTrigger("YourOn");
        }

        yield return new WaitForSeconds(2.5f);

        invalidTile = true;

        yield break;
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
    public IEnumerator moveUnit(UnitController unit, Vector2Int tilepos)
    {
        // 現在地
        Vector2Int unitpos = unit.Pos;

        //移動したかどうかのフラグ
        moved = true;

        //moveSoundを流す
        SE sePlayer = SeObject.GetComponent<SE>();
        sePlayer.moveSound();

        // 誰かいたら消す ここが攻撃できるところ
        if (null != units[tilepos.x, tilepos.y])
        {
            //選択したタイルのコマのスクリプトを読み取り、GetHP()をHpにいれる
            Hp = units[tilepos.x, tilepos.y].GetHP();

            //タイルをクリックできなくする　※モード移行を追加したのでいらないかも
            invalidTile = false;
            
            //攻撃ダメージを保存する変数
            int PU = 0;
            
            //戦闘時の背景暗転とバトルテキストを表示
            panelAnim.SetTrigger("in");
            textAnim.SetTrigger("in");

            //ダイスをまわすボタンを少し遅らせて表示させる
            Invoke("battleSetMode", 1f);

            //pushAButtonがtrueになるまでここで待機
            yield return new WaitUntil(() => pushAButton == true);
            sePlayer.moveSound2();//ダイスを振る音

            //ダイスで出た数値をPUに入れる→Hp
            PU = diceTime();
            Hp = Hp - PU;

            //SetHpを行う(相手コマに体力を保存させる)
            units[tilepos.x, tilepos.y].SetHP(Hp);

            //少し待つ
            yield return new WaitForSeconds(1f);

            
            if(Hp <= 0) {
                battleEnd = true;

                //バトル演出系をoffする
                panelAnim.SetTrigger("spFadeOut");
                AttackButton.SetActive(false);
                ATKText.SetActive(false);
                pushAButton = false;
                diceCheck = false;

                effCon.enemyPositionEff(0, units[tilepos.x, tilepos.y].unitVec());
                sePlayer.moveSound1();//戦闘音
                yield return new WaitForSeconds(0.3f);

                effCon.enemyPositionEff(4, units[tilepos.x, tilepos.y].unitVec());

                if (units[tilepos.x, tilepos.y].GetTYPE() == 1 && nowPlayer == 0) { DontDestroySingleObject.p1TakePawn++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 1 && nowPlayer == 1) { DontDestroySingleObject.p2TakePawn++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 2 && nowPlayer == 0) { DontDestroySingleObject.p1TakeRook++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 2 && nowPlayer == 1) { DontDestroySingleObject.p2TakeRook++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 3 && nowPlayer == 0) { DontDestroySingleObject.p1TakeKnight++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 3 && nowPlayer == 1) { DontDestroySingleObject.p2TakeKnight++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 4 && nowPlayer == 0) { DontDestroySingleObject.p1TakeBishop++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 4 && nowPlayer == 1) { DontDestroySingleObject.p2TakeBishop++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 5 && nowPlayer == 0) { DontDestroySingleObject.p1TakeQueen++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 5 && nowPlayer == 1) { DontDestroySingleObject.p2TakeQueen++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 6 && nowPlayer == 0) { DontDestroySingleObject.p1TakeKing++; }
                if (units[tilepos.x, tilepos.y].GetTYPE() == 6 && nowPlayer == 1) { DontDestroySingleObject.p2TakeKing++; }
                //キングのHPが0になった時の処理
                //まずキングを取得して１ｐと２ｐ
                UnitController sinu1 = getUnit(0, UnitController.TYPE.KING);
                UnitController sinu2 = getUnit(1, UnitController.TYPE.KING);
                Text info = txtResultInfo.GetComponent<Text>();
                if (sinu1.GetHP() <= 0) { info.text = "2Pの勝ち！！"; Invoke("Result", 3.0f); Debug.Log(sinu1.GetHP() + "通った"); DontDestroySingleObject.winner = 1; }
                if (sinu2.GetHP() <= 0) { info.text = "1Pの勝ち！！"; Invoke("Result", 3.0f); Debug.Log(sinu2.GetHP() + "通ったよ"); DontDestroySingleObject.winner = 0; }
                //ここでキングのＨＰが0なら勝者を３秒表示してリザルトに

                yield return new WaitForSeconds(0.1f);

                Destroy(units[tilepos.x, tilepos.y].gameObject);//敵の駒のHPをUnitControllerのGetHPからとりif文で分岐
                prevDestroyTurn = 0;

                yield return new WaitForSeconds(1f);

                //攻撃した側のターンを読み取って1P側か2P側の攻撃かを判断する(SP取得用)
                if(nowPlayer == 0) {
                    player1Chara.setSP(units[tilepos.x, tilepos.y].GetPOINT());
                    player1Chara.setPlayer1SpBar();
                }
                else if(nowPlayer == 1) {
                    player2Chara.setSP(units[tilepos.x, tilepos.y].GetPOINT());
                    player2Chara.setPlayer2SpBar();
                }

                

                // 新しい場所へ移動
                unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

                // 内部データ更新（元の場所）
                units[unitpos.x, unitpos.y] = null;

                // 内部データ更新（新しい場所）
                units[tilepos.x, tilepos.y] = unit;

                //動けないようにする
                canMoveCounter = false;

                yield break;

            } else {
                battleEnd = true;

                panelAnim.SetTrigger("spFadeOut");
                AttackButton.SetActive(false);
                ATKText.SetActive(false);
                pushAButton = false;
                diceCheck = false;

                effCon.enemyPositionEff(0, units[tilepos.x, tilepos.y].unitVec());
                sePlayer.moveSound1();//戦闘音
                yield return new WaitForSeconds(1.0f);

                //自分の攻撃したコマが「ポーン、ナイト、キング」だったら移動しないでその場にとどまる
                if(unit.Type == UnitController.TYPE.PAWN || unit.Type == UnitController.TYPE.KNIGHT || unit.Type == UnitController.TYPE.KING) {
                    tilepos.x = selectUnit.Pos.x;
                    tilepos.y = selectUnit.Pos.y;
                }
                //自分の攻撃したコマが上のコマ以外だったら相手の駒の目の前でとどまる
                else if(unit.Type == UnitController.TYPE.BISHOP || unit.Type == UnitController.TYPE.QUEEN || unit.Type == UnitController.TYPE.ROOK) {

                    //右に移動
                    if(tilepos.x < selectUnit.Pos.x) {
                        tilepos.x = tilepos.x + 1;
                    }//左に移動
                    else if(tilepos.x > selectUnit.Pos.x) {
                        tilepos.x = tilepos.x - 1;
                    }//上に移動
                    if(tilepos.y > selectUnit.Pos.y) {
                        tilepos.y = tilepos.y - 1;
                    }//下に移動
                    else if(tilepos.y < selectUnit.Pos.y) {
                        tilepos.y = tilepos.y + 1;
                    }

                }

                

                // 新しい場所へ移動
                unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

                // 内部データ更新（元の場所）
                units[unitpos.x, unitpos.y] = null;

                // 内部データ更新（新しい場所）
                units[tilepos.x, tilepos.y] = unit;

                //動けないようにする
                canMoveCounter = false;

                yield break;
            }
        }
        else{

            //nextMode = MODE.STATUS_UPDATE;

            // 新しい場所へ移動
            unit.MoveUnit(tiles[tilepos.x, tilepos.y]);

            // 内部データ更新（元の場所）
            units[unitpos.x, unitpos.y] = null;

            // 内部データ更新（新しい場所）
            units[tilepos.x, tilepos.y] = unit;

            //動けないようにする
            canMoveCounter = false;

            yield break;
        }
    }

    //ボタン表示
    public void battleSetMode() {
        AttackButton.SetActive(true);
        GameObject Dice = GameObject.Find("1PDice");
        Transform DiceTrn = Dice.transform;
        DiceTrn.Translate(0, -98, 0);//画面に映る値
        Invoke("AttackDiceButton",0.1f);
    }

    public void AttackDiceButton()
    {
        attackDiceButton.Select();
    }

    //ダイス回すボタンクリック
    public void pushATKButton() {
        pushAButton = true;
        endButton.Select();
    }


    //試しダイス
    int rnd;
    public int diceTime()
    {
        GameObject d6 = GameObject.Find("1PDice/d6 2");
        rnd = d6.GetComponent<nanika>().GetNumber();
        //rnd = Random.Range(1, 7);

        if(nowPlayer == 0)
        {
            if (damageFlag == 1)
            {
                rnd *= 2;
                damageFlag -= 1;
                Debug.Log("1P");
            }
        }
        if(nowPlayer == 1)
        {
            if (damageFlag == 2)
            {
                rnd *= 2;
                damageFlag -= 2;
                Debug.Log("2P");
            }
        }
        if (damageFlag == 3)
        {
            if (nowPlayer == 0)
            {
                rnd *= 2;
                damageFlag -= 1;
            }
            else if (nowPlayer == 1)
            {
                rnd *= 2;
                damageFlag -= 2;
            }
        }

        ATKText.SetActive(true);
        aText.text = rnd.ToString();

        diceCheck = true;
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

        /*foreach (var v in units)
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
        }*/

        return ret;
    }

    //先攻後攻をきめる
    public IEnumerator firstTurn()
    {
        

        Animator turnOrder = GameObject.Find("FirstTurnImage").GetComponent<Animator>();

        if(turnRnd == 0)
        {
            nowPlayer = 0;
            turnOrder.SetTrigger("Second");

        }
        else if(turnRnd == 1)
        {
            nowPlayer = 1;
            
            turnOrder.SetTrigger("First");

        }
        
        
        nextMode = MODE.TURN_CHANGE;

        yield break;
    }

   

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void Result()
    {
        SceneManager.LoadScene("Result");
    }

    public int daise()
    {
        return 1;
    }

    public void TrnEnd()
    {
        if(moved == true) {nextMode = MODE.STATUS_UPDATE; moved = false;}
    }

    //呼び出し用のinvalidTile反転メソッド
    public void tileBoolInversion() {
        invalidTile = !invalidTile;
    }
}
