using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    //このユニットのプレイヤー番号
    public int Player;
    //ユニットの種類
    public TYPE Type;
    //おいてからの経過ターン
    public int ProgressTurnCount;
    //おいてる場所
    public Vector2Int Pos, OldPos;//Vector2Int とはXとYのINT型を扱う
    //移動状態
    public List<STATUS> Status;


    // 1=ポーン　2＝ルーク 3=ナイト 4=ビショップ 5=クイーン 6=キング
    public enum TYPE
    {
        NONE = -1,
        PAWN = 1,
        ROOK,
        KNIGHT,
        BISHOP,
        QUEEN,
        KING,
    }
    
    public enum STATUS
    {
        NONE = -1,
        QSIDE_CASTLING=1,
        KSIDE_CASTLING,
        EN_PASSANT,
        CHECK,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初期設定
    public void SetUnit(int player, TYPE type, GameObject tile)
    {
        this.Player = player;
        Type = type;
        MoveUnit(tile);
        ProgressTurnCount = -1;//初期状態に戻す
    }

    //移動可能範囲取得
    public List<Vector2Int> GetMovableTiles(UnitController[,] units,bool checkking = true)
    {
        List<Vector2Int> ret = new List<Vector2Int>();

        //クイーン
        if(TYPE.QUEEN == Type)
        {
            ret = getMovableTiles(units,TYPE.ROOK);
            ret = getMovableTiles(units, TYPE.BISHOP);//クイーンだけの処理にする
        }
        else if(TYPE.KING == Type)
        {
            ret = getMovableTiles(units,TYPE.KING);

            //敵の移動可能範囲にいけないようにする
        }
        else
        {
            ret = getMovableTiles(units,Type);
        }
        return ret;
    }

    //移動可能範囲を返す（プレーンな状態）
    List<Vector2Int> getMovableTiles(UnitController[,]units,TYPE type)
    {
        List<Vector2Int> ret = new List<Vector2Int>();

        //ポーン
        if (TYPE.PAWN == type)
        {
            int dir = 1;
            if(1 == Player) dir = -1;
            //前方２ます
            List<Vector2Int> vec = new List<Vector2Int>()
            {
                new Vector2Int(0,1*dir),
                new Vector2Int(0,2*dir),
            };

            //2回目以降はいちますしか進めない
            if(-1 < ProgressTurnCount) vec.RemoveAt(vec.Count -1);

            foreach(var v in vec)
            {
                Vector2Int checkpos = Pos + v;
                if(!isCheckable(units,checkpos)) continue;
                if(null != units[checkpos.x , checkpos.y])break;

                ret.Add(checkpos);
            }

            //取れる時は斜めに進める
            vec = new List<Vector2Int>()
            {
                new Vector2Int(-1 ,1*dir),
                new Vector2Int(+1, 1*dir),
            };

            foreach (var v in vec)
            {
                Vector2Int checkpos = Pos + v;
                if (!isCheckable(units, checkpos)) continue;

                //アンパッサン
                if(null != getEnPassanUnit(units, checkpos))
                {
                    ret.Add(checkpos);
                    continue;
                }

                //何もない状態
                if(null == units[checkpos.x , checkpos.y]) continue;

                //自軍のユニットは無視
                if( Player == units[checkpos.x, checkpos.y].Player) continue;

                //追加
                ret.Add(checkpos);
            }
        }
        //ルーク
        if(TYPE.ROOK == type)
        {
            //上下左右ユニットにぶつかるまでどこまでも進める
            List<Vector2Int> vec = new List<Vector2Int>()
            {
                new Vector2Int(0, 1),
                new Vector2Int(0,-1),
                new Vector2Int(1, 0),
                new Vector2Int(-1,0),
            };

            foreach(var v in vec)
            {
                Vector2Int checkpos = Pos + v;
                while (isCheckable(units,checkpos))
                {
                    //誰かいたら終了
                    if( null != units[checkpos.x, checkpos.y])
                    {
                        if (Player != units[checkpos.x,checkpos.y].Player)
                        {
                            ret.Add(checkpos);//敵の駒がいたらそこも追加
                        }
                        break;
                    }

                    ret.Add(checkpos);
                    checkpos += v;
                }
            }
        }

        return ret;
    }

    //配列内かどうか調べる　外ならfalseを返す
    bool isCheckable(UnitController[,] ary, Vector2Int idx)
    {
        if(    idx.x < 0 || ary.GetLength(0) <= idx.x
            || idx.y < 0 || ary.GetLength(0) <= idx.y)
        {
            return false;
        }
        return true;
    }
   
    //選択時の処理
    public void SelectUnit(bool select = true)
    {
        //選択されたとき上にあげて重力で落ちないようにする
        Vector3 pos = transform.position;
        pos.y += 2;
        GetComponent<Rigidbody>().isKinematic = true;//重力なし

        //選択解除
        if (!select)
        {
            pos.y = 1.35f;
            GetComponent<Rigidbody>().isKinematic = false;//重力あり
        }
        //ポジションをここで変化させてる
        transform.position = pos;
    }

    //移動処理
    public void MoveUnit(GameObject tile)
    {
        //移動時は非選択にする
        SelectUnit(false);

        //タイルのポジションから配列番号に戻す
        Vector2Int idx = new Vector2Int(
            (int)tile.transform.position.x + GameSceneDirector.TILE_X / 2,
            (int)tile.transform.position.z + GameSceneDirector.TILE_Y / 2);

        //新しい場所へ移動
        Vector3 pos = tile.transform.position;
        pos.y = 1.35f;
        transform.position = pos;//ユニットの場所

        //移動状態のリセット
        Status.Clear();

        //アンパッサン等処理
        if(TYPE.PAWN == Type)
        {
            //縦に2タイル進んだ時
            if (1 < Mathf.Abs(idx.y - Pos.y))//Mathf.Absとは絶対値を扱う idx.yこれから行こうとしてる場所　pos.y今の自分の値　１Pと２Pで進む方向が違うので
            {
                Status.Add(STATUS.EN_PASSANT);
                //移動した一歩手前に残像が残る
                int dir = -1;
                if (1 == Player) dir = 1;

                Pos.y = idx.y + dir;
            }

            //キャスリング
            if (TYPE.KING == Type)
            {
                //横に２タイル進んだら
                if (1 < idx.x - Pos.x)
                {
                    Status.Add(STATUS.KSIDE_CASTLING);
                }
                if (-1 > idx.x - Pos.x)
                {
                    Status.Add(STATUS.QSIDE_CASTLING);
                }
            }

            //インデックスの更新
            OldPos = Pos;
            Pos = idx;

            //おいてからのターンをリセット
            ProgressTurnCount = 0;
        }

    }

    //前回移動してからターンをカウント
    public void ProgressTurn()
    {
        //初動は無視
        if(0 > ProgressTurnCount) return;

        ProgressTurnCount++;

        //アンパッサンフラグチェック
        if (TYPE.PAWN == Type)
        {
            if (1 < ProgressTurnCount)
            {
                Status.Remove(STATUS.EN_PASSANT);
            }
        }
    }

    //相手のアンパッサン状態のユニットを返す
    UnitController getEnPassanUnit(UnitController[,] units,Vector2Int pos)//全体のUnitを調べてOldPosがposと一致するかどうかを調べる一致したらUnitControllerを返す
    {
        foreach(var v in units)
        {
            if (null == v) continue;//データが入って無かったら次へ
            if (Player == v.Player) continue;//自分と同じプレイヤーなら次へ
            if (!v.Status.Contains(STATUS.EN_PASSANT)) continue;//アンパッサンフラグが含まれてたら何もしない

            if(v.OldPos == pos) return v;
        }

        return null;
    }

    //今回のターンのチェック状態をセット
    public void SetCheckStatus(bool flag = true)
    {
        Status.Remove(STATUS.CHECK);//リセット
        if(flag) Status.Add(STATUS.CHECK);//もしフラグが立っていたらステータスにチェックを追加
    }
}
