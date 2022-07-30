using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDirector : MonoBehaviour
{
    private int SlotNum = 0;
    [SerializeField]private animation LoadPanel;
    [SerializeField]private GameObject[] Slot;
    [SerializeField]private GameObject AButton;
    // Start is called before the first frame update
    private GameObject Panel;
    FadeController Fade;

    void Start()
    {
        Panel = GameObject.Find("Panel");
        Fade = Panel.GetComponent<FadeController>();
        AButton.SetActive(false);
        StartCoroutine(SetWait());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SixWait());
        SlotNumber();
        SetSlot();
    }

    public void SlotNumber()
    {
        if (LoadPanel.AnimaEnd)
        {
            if (Input.GetKeyDown("x"))
            {
                SlotNum += 1;
                if (SlotNum > 5)
                {
                    SlotNum -= 6;
                }

            }
            else if (Input.GetKeyDown("z"))
            {
                SlotNum -= 1;
                if (SlotNum < 0)
                {
                    SlotNum += 6;
                }
            }
        }
        
    }

    public void SetSlot()
    {
        var SN = ArrayList.FixedSize(Slot);
        /*foreach(GameObject Obj in Slot)
        {
            if(SN == SlotNum)
            {
                Slot[SN].SetActive(true);
            }
            else if(SN != SlotNum)
            {
                Slot[SN].SetActive(false);
            }
        }*/
    }

    public void ChangeScene()
    {
        if (Input.GetKeyDown("a"))
        {
            Fade.isFadeOut = 2;
            StartCoroutine(Change());
        }
    }

    public IEnumerator Change()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Chess 1");
        yield break;
    }

    public IEnumerator SixWait()
    {
        yield return new WaitForSeconds(6.3f);
        ChangeScene();
        yield break;
    }

    public void SetButton()
    {
        GameObject.Find("Basic 3").gameObject.SetActive(false);
        AButton.SetActive(true);
    }

    public IEnumerator SetWait()
    {
        yield return new WaitForSeconds(6.3f);
        SetButton();
        yield break;
    }
}
