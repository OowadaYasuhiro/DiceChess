using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVProcessing : MonoBehaviour
{
    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public Vector3 position;
    public Quaternion rotation;

    private void Start()
    {
        csvFile = Resources.Load("CSV/CSVUnitData") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        //行、列で指定する。 csvDatas[0][0]
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
