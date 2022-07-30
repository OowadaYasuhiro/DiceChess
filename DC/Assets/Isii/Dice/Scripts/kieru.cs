using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kieru : MonoBehaviour
{
    public float deleteTime;
    public int result = 0;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("down")) {
            Vector3 check_1 = transform.TransformDirection(Vector3.forward);
            Vector3 check_4 = transform.TransformDirection(Vector3.right);
            Vector3 check_5 = transform.TransformDirection(Vector3.up);


            if (Mathf.Abs(Mathf.Round(check_1.y)) != 1) {
                if (Mathf.Abs(Mathf.Round(check_4.y)) != 1) {
                    if (Mathf.Round(check_5.y) == 1) {
                        result = 5;
                    } else {
                        result = 2;
                    }
                } else {
                    if (Mathf.Round(check_4.y) == 1) {
                        result = 4;
                    } else {
                        result = 3;
                    }
                }
            } else {
                if (Mathf.Round(check_1.y) == 1) {
                    result = 1;
                } else {
                    result = 6;
                }
            }

            Debug.Log("出た目は " + result + " です");
            Destroy(this.gameObject, deleteTime);
        }
    }
    public int GetResult()
    {
        return result;
    }
    
    public void ButtonKieru()
    {
        Vector3 check_1 = transform.TransformDirection(Vector3.forward);
        Vector3 check_4 = transform.TransformDirection(Vector3.right);
        Vector3 check_5 = transform.TransformDirection(Vector3.up);


        if (Mathf.Abs(Mathf.Round(check_1.y)) != 1){
            if (Mathf.Abs(Mathf.Round(check_4.y)) != 1){
                if (Mathf.Round(check_5.y) == 1){
                    result = 5;
                }else{
                    result = 2;
                }
            }else{
                if (Mathf.Round(check_4.y) == 1){
                    result = 4;
                }else{
                    result = 3;
                }
            }
        }else{
            if (Mathf.Round(check_1.y) == 1){
                result = 1;
            }else{
                result = 6;
            }
        }

        Destroy(this.gameObject, deleteTime);
    }
}

