using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    [SerializeField] private CursorManager cursorManager;

    [SerializeField] private GameObject charaSelectCanvas;
    [SerializeField] private GameObject itemSelsectCanvas;
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private Button[] charaFrame;
    [SerializeField] private Button[] itemFrame;

    public enum MODE
    {
        none = -1,
        CharaSet = 1,
        CharaSelect,
        ItemSet,
        ItemSelect,
        NextScene
    }
    MODE _nowMode;

    private void Start()
    {
        _nowMode = MODE.CharaSet;
    }

    private void Update()
    {
        if(_nowMode == MODE.CharaSet)
        {
            CharaSet();
        }
        else if(_nowMode == MODE.CharaSelect)
        {
            
            if (cursorManager.CharaSelectCount == 2)
            {
                CharaNextMode();
            }
            else
            {
                CharaSelect();
            }
            

        }
        else if(_nowMode == MODE.ItemSet)
        {
            ItemSet();

        }
        else if(_nowMode == MODE.ItemSelect)
        {
            
            
            
            if (cursorManager.ItemSelectCount == 4)
            {
                cursorManager.getI();
                Invoke("ItemNextMode",1f);
            }
            else
            {
                ItemSelect();
            }
        }
        else if(_nowMode == MODE.NextScene)
        {
            SceneManager.LoadScene("LoadScene");
        }
    }

    //1
    public void CharaSet()
    {
        charaSelectCanvas.SetActive(true);
        itemSelsectCanvas.SetActive(false);

        charaFrame[0].Select();

        _nowMode = MODE.CharaSelect;
    }

    //2
    public void CharaSelect()
    {
        if(Input.GetButtonDown("Cancel")){

            cursorManager.CharaCnacel("Title");
        }
    }

    public void CharaNextMode()
    {
        _nowMode = MODE.ItemSet;
    }
    //応急処置
    public void FrameColor()
    {
        if(cursorManager.CharaSelectCount == 0)
        {
            //eventSystem.firstSelectedGameObject.GetComponent<Button>();

        }
    }

    //3
    public void ItemSet()
    {
        charaSelectCanvas.SetActive(false);
        itemSelsectCanvas.SetActive(true);

        itemFrame[0].Select();

        _nowMode = MODE.ItemSelect;
    }

    //4
    public void ItemSelect()
    {
        if (Input.GetButtonDown("Cancel"))
        {

            cursorManager.ItemCnacel();
            if(cursorManager.ItemSelectCount == 0)
            {
                _nowMode = MODE.CharaSet;
            }
        }
    }
    public void ItemNextMode()
    {
        _nowMode = MODE.NextScene;
    }

    //5
    public void NextScene(string name)
    {
        
        SceneManager.LoadScene(name);
    }

}
