using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    private float RepeatTime = 12f;
    private bool animaEnd = true;
    private Animator anima;
    // 1＝キング　2＝クイーン　３＝ルーク　４＝ビショップ　５＝ナイト　６＝ポーン
    [SerializeField]private int ChessPanelNum;
    [SerializeField]private int setNum;
    [SerializeField]private GameObject[] ChessPanelObj;

    // Start is called before the first frame update
    void Start()
    {
        
        anima = GetComponent<Animator>();
        if(setNum == 0)
        {
            anima.Play("CenterPos");
            ChessPanelNum = 1;
        }
        if (setNum == 1)
        {
            anima.Play("RightPos");
            ChessPanelNum = 2;
        }
        if (setNum == 2)
        {
            anima.Play("LeftPos");
            ChessPanelNum = 6;
        }

    }

    // Update is called once per frame
    void Update()
    {
        RepeatTime -= Time.deltaTime;
        if (animaEnd)
        {
            if(Input.GetKeyDown("z")) 
            {
                animaEnd = false;
                anima.SetTrigger("rightAxis"); 
                RepeatTime = 12f;
            }
            if (Input.GetKeyDown("x")||RepeatTime <= 0)
            {
                animaEnd = false;
                anima.SetTrigger("leftAxis");
                RepeatTime = 12f;
            }

        }
        else
        {

        }
    }

    public void animaEndCheck() {
        animaEnd = true;
    }

    public void ChengePanelNum(int num)
    {
        this.ChessPanelNum += num;
        if(this.ChessPanelNum < 1)
        {
            this.ChessPanelNum += 6;
        }
        else if (this.ChessPanelNum > 6)
        {
            this.ChessPanelNum -= 6;
        }
    }

    public int ChessPanelNumGet
    {
        get
        {
            return ChessPanelNum;
        }
    }

    public bool AnimaEnd
    {
        get
        {
            return animaEnd;
        }
    }

}
