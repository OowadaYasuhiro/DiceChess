using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    // 音データの再生装置を格納する変数
    private AudioSource audio;

    // 音データを格納する変数（Inspectorタブからも値を変更できるようにする）
    [SerializeField] private AudioClip[] _bgm;
    [SerializeField] private AudioClip[] _se;



    // Start is called before the first frame update
    void Start()
    {
        // ゲームスタート時にAudioSource（音再生装置）のコンポーネントを加える
        audio = gameObject.AddComponent<AudioSource>();
    }

    
    public void playBGM(int num)
    {
        audio.PlayOneShot(_bgm[num]);
    }

    public void playSE(int num)
    {
        audio.PlayOneShot(_se[num]);
    }
}
