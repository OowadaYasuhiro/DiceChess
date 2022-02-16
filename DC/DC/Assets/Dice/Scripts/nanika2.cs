using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nanika2 : MonoBehaviour
{
    public float Num;
    private Quaternion RandomQ;
    public GameObject[] CubePrefabs; //オブジェクトを格納する配列変数
    private int number; //ランダム情報を入れるための変数
    public bool dice;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dice == true)
        {
            Num = Random.Range(-180, 180);
            RandomQ = Quaternion.Euler(0, 0, Num);
            //最初にランダムで回転させる
            transform.rotation = Random.rotation * RandomQ;
            if (Input.GetKeyDown("up"))
            {
                number = Random.Range(0, CubePrefabs.Length);
                transform.parent.position = new Vector3(0, 100, 0);
                Instantiate(CubePrefabs[number]); //ランダム出現
            }
        }
    }
}
