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
        _maxSp = int.Parse(csvDatas[0][0]);
        Debug.Log("_maxSpは" + csvDatas[0][0]);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void setSP(int i) {
        _sp += i;
        //必殺技ゲージが100以上になったら余分なポイントを減らす
        if(_maxSp < _sp) {
            _sp -= _sp - _maxSp;
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
   
}
