using UnityEngine;
using System.Collections;

public class DiceControl : MonoBehaviour
{
    public int value;
    private Die_d6 die;
    public float Num;
    private Quaternion RandomQ;
    public float deleteTime;

    void Start()
    {
        Num = Random.Range(-180, 180);
        RandomQ = Quaternion.Euler(0, 0, Num);
        //最初にランダムで回転させる
        transform.rotation = Random.rotation * RandomQ;
        die = gameObject.GetComponent<Die_d6>();
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.forward * 10;
        transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 15, 0) * 10;
    }
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            Vector3 check_1 = transform.TransformDirection(Vector3.forward);
            Vector3 check_4 = transform.TransformDirection(Vector3.right);
            Vector3 check_5 = transform.TransformDirection(Vector3.up);
            int result = 0;

            if (Mathf.Abs(Mathf.Round(check_1.y)) != 1)
            {
                if (Mathf.Abs(Mathf.Round(check_4.y)) != 1)
                {
                    if (Mathf.Round(check_5.y) == 1)
                    {
                        result = 5;
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    if (Mathf.Round(check_4.y) == 1)
                    {
                        result = 4;
                    }
                    else
                    {
                        result = 3;
                    }
                }
            }
            else
            {
                if (Mathf.Round(check_1.y) == 1)
                {
                    result = 1;
                }
                else
                {
                    result = 6;
                }
            }

            Debug.Log("出た目は " + result + " です");
            Destroy(this.gameObject, deleteTime);
        }
        
    }
    /*int[] value = {50,500,3};
    // X方向にｉ回だけ回転させる関数。
    void RotationX(int i)
    {
        if (i == 0) { return; }

        // 値を一時的に保管。
        int A = value[0];
        int B = value[1];

        if (i > 0)
        {
            // 回転後の値を、それぞれ代入。
            value[0] = 7 - B;
            value[1] = A;

            // 値を－１して同じ関数を呼び出し、処理を繰り返す。
            RotationX(i - 1);
        }
        else
        {
            // 回転後の値を、それぞれ代入。
            value[0] = B;
            value[1] = 7 - A;

            // 値を＋１して同じ関数を呼び出し、処理を繰り返す。
            RotationX(i + 1);
        }
    }*/
}