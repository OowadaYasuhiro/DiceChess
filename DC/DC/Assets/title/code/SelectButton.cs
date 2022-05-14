using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    Button チュートリアル;
    Button START;
    Button EXIT;

    // Start is called before the first frame update
    void Start()
    {
        チュートリアル = GameObject.Find("チュートリアルUI/チュートリアル").GetComponent<Button>();

        START = GameObject.Find("START").GetComponent<Button>();

        EXIT = GameObject.Find("EXIT").GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
