using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageSceneName : MonoBehaviour
{
    /// 現在のゲームの状態が勝利演出中の時の更新を行う
    void WinUpdate()
    {
        // TODO:勝利演出を行う

        if (Input.anyKeyDown)
        {   // 何かキーが押されたら
            // 次ステージのシーンをロードする
            SceneManager.LoadScene(nextStageSceneName);
        }
    }

    // 現在のゲームの状態が敗北演出中の時の更新を行う
    void LoseUpdate()
    {
        // TODO:敗北演出を行う

        if (Input.anyKeyDown)
        {   // 何かキーが押されたら
            // タイトルシーンに戻る
            SceneManager.LoadScene("Result3");
        }
    }
}
