using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MessageController : MonoBehaviour
{
    [SerializeField]TextAsset _csvFile;
    List<string[]> csvDatas = new List<string[]>();

    [SerializeField] private CursorManager _cursorManager;
    [SerializeField] private SelectController _selectController;

    [SerializeField] private Button[] _charaFrame;
    [SerializeField] private Button[] _itemFrame;

    [SerializeField] private GameObject _messageFrame;
    [SerializeField] private Button _messageFrameButton;
    [SerializeField] private Text _skillName;
    [SerializeField] private Text _mainText;

    // Start is called before the first frame update
    void Start()
    {
        StringReader reader = new StringReader(_csvFile.text);

        while(reader.Peek() != -1) {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }

        Debug.Log(csvDatas[1][1]);

        _messageFrame.SetActive(false);
    }

    public void Update() {
        if(Input.GetButtonDown("Cancel") && _selectController.messageFlag == true) {
            outMessage();
            _selectController.messageFlag = false;

        }
    }

    public void setMessage() {
        var sc = _selectController;
        if(_cursorManager.CharaSelectCount == 0 || _cursorManager.ItemSelectCount == 0 || _cursorManager.ItemSelectCount == 1) {
            _messageFrame.transform.position = new Vector3(230, 540, 0);
        }
        if(_cursorManager.CharaSelectCount == 1 || _cursorManager.ItemSelectCount == 2 || _cursorManager.ItemSelectCount == 3) {
            _messageFrame.transform.position = new Vector3(1690, 540, 0);
        }

        
        if(sc.nowMode == 2) {
            switch(sc.nowSelectChara) {
                case 0:
                    _skillName.text = csvDatas[1][0];
                    _mainText.text = csvDatas[1][1];
                    break;
                case 1:
                    _skillName.text = csvDatas[3][0];
                    _mainText.text = csvDatas[3][1];
                    break;
                case 2:
                    _skillName.text = csvDatas[5][0];
                    _mainText.text = csvDatas[5][1];
                    break;
                case 3:
                    _skillName.text = csvDatas[7][0];
                    _mainText.text = csvDatas[7][1];
                    break;
            }
        }

        if(sc.nowMode == 4) {
            _skillName.text = "";
            switch(sc.nowSelectItem) {
                case 0:
                    _mainText.text = csvDatas[9][0];
                    break;
                case 1:
                    _mainText.text = csvDatas[11][0];
                    break;
                case 2:
                    _mainText.text = csvDatas[13][0];
                    break;
                case 3:
                    _mainText.text = csvDatas[15][0];
                    break;
            }
        }

        _messageFrame.SetActive(true);
        _messageFrameButton.Select();

        CancelButton();
    }

    public void CancelButton() {
        
    }

    public void outMessage() {
        var sc = _selectController;

        _messageFrame.SetActive(false);
        if(sc.nowMode == 2) {
            switch(sc.nowSelectChara) {
                case 0:
                    _charaFrame[0].Select();
                    break;
                case 1:
                    _charaFrame[1].Select();
                    break;
                case 2:
                    _charaFrame[2].Select();
                    break;
                case 3:
                    _charaFrame[3].Select();
                    break;
            }

            _selectController.CharaSelect();
        }
        else if(sc.nowMode == 4) {
            switch(sc.nowSelectItem) {
                case 0:
                    _itemFrame[0].Select();
                    break;
                case 1:
                    _itemFrame[1].Select();
                    break;
                case 2:
                    _itemFrame[2].Select();
                    break;
                case 3:
                    _itemFrame[3].Select();
                    break;
            }

            _selectController.ItemSelect();
        }


    }
}
