using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ClickStartButton()
    {
        SceneManager.LoadScene("SelectScene");
    }
    public void EndGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        Application.Quit();//ゲームプレイ終了
    }
}