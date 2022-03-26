using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nanika: MonoBehaviour
{
    public float Num;
    private Quaternion RandomQ;
    public float deleteTime; 
    public GameObject[] CubePrefabs; //オブジェクトを格納する配列変数
    private int number; //ランダム情報を入れるための変数
    public int[] hairetu = new int[6];
    public int count;
    public bool dice;

    void Start()
    {
        tomato();
    }

    void tomato()
    {
        count = 0;
        hairetu = new int[6] {-1,-1,-1,-1,-1,-1};
    }

    // Update is called once per frame
    void Update()
    {
        if(dice == true)
        {
            Num = Random.Range(-180, 180);
            RandomQ = Quaternion.Euler(0, 0, Num);
            //最初にランダムで回転させる
            transform.rotation = Random.rotation * RandomQ;

            if (Input.GetKeyDown("up"))
            {
                if (count == 5)
                {
                    tomato();
                }
                Randomreturn:
                number = Random.Range(0, 6);
                for (int i = 0; i < hairetu.Length; i++)
                {
                    if (number == hairetu[i])
                    {
                        goto Randomreturn;
                    }
                }
                hairetu[count] = number;
                transform.parent.position = new Vector3(0, 100, 0);
                Instantiate(CubePrefabs[number]); //ランダム出現
                count++;
            }
        }
    }
}
