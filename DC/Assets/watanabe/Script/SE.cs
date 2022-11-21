using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    // 音データの再生装置を格納する変数
    private AudioSource audio;

    // 音データを格納する変数（Inspectorタブからも値を変更できるようにする）
    [SerializeField] private AudioClip komaugoku;
    [SerializeField] private AudioClip komasentou;
    [SerializeField] private AudioClip daisufuru;
    [SerializeField] private AudioClip daisunome;
    [SerializeField] private AudioClip ketteion;
    [SerializeField] private AudioClip hissatuwaza;
    [SerializeField] private AudioClip sentakuka_soru;
    [SerializeField] private AudioClip ge_musyuuryouji;
    [SerializeField] private AudioClip senkoukoukouhyouji;
    [SerializeField] private AudioClip aitemusiyouji;
    [SerializeField] private AudioClip me_ta_fueru;
    [SerializeField] private AudioClip sousapureiya_hyouji;
    [SerializeField] private AudioClip ta_nsyuuryouji;
    [SerializeField] private AudioClip kingtaosita;
    [SerializeField] private AudioClip suujihyouji;
    [SerializeField] private AudioClip pe_jiskip;
    [SerializeField] private AudioClip syutujinbotan;
    [SerializeField] private AudioClip winSE;


    // Start is called before the first frame update
    void Start()
    {
        // ゲームスタート時にAudioSource（音再生装置）のコンポーネントを加える
        audio = gameObject.AddComponent<AudioSource>();
    }

    //駒動く音
    public void moveSound()
    {
        audio.PlayOneShot(komaugoku);
    }

    //駒戦闘音
    public void moveSound1()
    {
        audio.PlayOneShot(komasentou);
    }

    //ダイス振る音
    public void moveSound2()
    {
        audio.PlayOneShot(daisufuru);
    }

    //ダイスの目出た音
    public void moveSound3()
    {
        audio.PlayOneShot(daisunome);
    }

    //決定音
    public void moveSound4()
    {
        audio.PlayOneShot(ketteion);
    }

    //必殺技使用音
    public void moveSound5()
    {
        audio.PlayOneShot(hissatuwaza);
    }

    //選択カーソル音
    public void moveSound6()
    {
        audio.PlayOneShot(sentakuka_soru);
    }

    //ゲーム終了時音
    public void moveSound7()
    {
        audio.PlayOneShot(ge_musyuuryouji);
    }

    //先攻後攻表示音
    public void moveSound8()
    {
        audio.PlayOneShot(senkoukoukouhyouji);
    }

    //アイテム使用時音
    public void moveSound9()
    {
        audio.PlayOneShot(aitemusiyouji);
    }

    //メーター増える音
    public void moveSound10()
    {
        audio.PlayOneShot(me_ta_fueru);
    }

    //操作プレイヤー表示音
    public void moveSound11()
    {
        audio.PlayOneShot(sousapureiya_hyouji);
    }

    //ターン終了時音
    public void moveSound12()
    {
        audio.PlayOneShot(ta_nsyuuryouji);
    }

    //キング倒した音
    public void moveSound13()
    {
        audio.PlayOneShot(kingtaosita);
    }

    //数字表示音
    public void moveSound14()
    {
        audio.PlayOneShot(suujihyouji);
    }

    //ページスキップ音
    public void moveSound15()
    {
        audio.PlayOneShot(pe_jiskip);
    }

    //出陣ボタン音
    public void moveSound16()
    {
        audio.PlayOneShot(syutujinbotan);
    }
    //出陣ボタン音
    public void winSound()
    {
        audio.PlayOneShot(winSE);
    }
}
