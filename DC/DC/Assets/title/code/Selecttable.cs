using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selecttable : MonoBehaviour
{
    Button チュートリアル;
    Button START;
    Button EXIT;

    // Start is called before the first frame update
    void Start()
    {
        チュートリアル　= GameObject.Find("チュートリアルUI/チュートリアル").GetComponent<Button>();
        
        START = GameObject.Find("STARTUI/START").GetComponent<Button>();

        EXIT = GameObject.Find("EXITUI/EXIT").GetComponent<Button>();
    }

}

