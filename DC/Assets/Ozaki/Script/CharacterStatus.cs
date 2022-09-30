using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class CharacterStatus : MonoBehaviour
{
    //必殺技ゲージ
    [SerializeField]private int _sp = 0;
    private int _maxSp;

    private Image _spPlayer1Image;
    private Image _spPlayer2Image;
    [SerializeField] private GameObject _HissatsuParticle1;
    [SerializeField] private GameObject _HissatsuParticle2;
    GameSceneDirector sceneDirector;


    //csv
    TextAsset csvFile; 
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public Vector3 position;
    public Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        _spPlayer1Image = GameObject.Find("Player1SpGauge").GetComponent<Image>();
        _spPlayer2Image = GameObject.Find("Player2SpGauge").GetComponent<Image>();

        //csv
        csvFile = Resources.Load("CSV/ItemSpecial") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        _maxSp = 50;
        //Debug.Log("_maxSpは" + csvDatas[0][0]);
        _spPlayer1Image.color = new Color32(255, 255, 255, 255);
        _spPlayer2Image.color = new Color32(255, 255, 255, 255);
        _HissatsuParticle1.SetActive(false);
        _HissatsuParticle2.SetActive(false);
        sceneDirector = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void setSP(int i) {
        _sp += i;
        //必殺技ゲージがMAX以上になったら余分なポイントを減らす
        if(_maxSp < _sp){
            _sp -= _sp - _maxSp;
            if (sceneDirector.nowPlayer == 0){
                _spPlayer1Image.color = new Color32(255, 193, 0, 255);
                _HissatsuParticle1.SetActive(true);
            }
            if (sceneDirector.nowPlayer == 1){
                _spPlayer2Image.color = new Color32(255, 193, 0, 255);
                _HissatsuParticle2.SetActive(true);
            }
        }
        if(0 > _sp) {
            _sp = 0;
        }
    }

    public int Sp {
        get { return _sp; }
    }

    public void setPlayer1SpBar() {
        _spPlayer1Image.fillAmount = _sp / 50f;
    }

    public void setPlayer2SpBar() {
        _spPlayer2Image.fillAmount = _sp / 50f;
    }

    public void setPlayer1SpColor(){
        _spPlayer1Image.color = new Color32(255, 255, 255, 255);
        _HissatsuParticle1.SetActive(false);
    }
    public void setPlayer2SpColor()
    {
        _spPlayer2Image.color = new Color32(255, 255, 255, 255);
        _HissatsuParticle2.SetActive(false);
    }
}
