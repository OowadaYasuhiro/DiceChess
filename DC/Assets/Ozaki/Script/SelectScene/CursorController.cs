using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [SerializeField] private int _cursorNum;
    private Image _image;
    [SerializeField] private float _blinkingSpeed = 0.5f;
    private float _red;
    private float _blue;
    private float _green;
    private float _alpha;
    private bool _isFadeIn = false;
    private bool _isFadeOut = false;

    private void Update()
    {
        
    }

    //点滅設定
    public void blinkingCursorOn()
    {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha >= 1.0f)
        {
            _isFadeOut = false;
            _alpha = 1.0f;
            _isFadeIn = true;

        }
    }
    public void blinkingCursorOff()
    {
        _alpha -= Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha <= 0.5f)
        {
            _isFadeIn = false;
            _alpha = 0.5f;

            _isFadeOut = true;

        }
    }
    public void blickingCursorEnd()
    {
        _alpha += Time.deltaTime / _blinkingSpeed;

        this._image.color = new Color(_red, _green, _blue, _alpha);
        if (_alpha >= 1.0f)
        {
            _isFadeOut = false;
            _isFadeIn = false;
            _alpha = 1.0f;

        }
    }

    //カーソル位置ずらし
    public void cursorSameOn()
    {
        if (_cursorNum == 1)
        {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_01").transform;
        }
        if (_cursorNum == 2)
        {
            this.gameObject.transform.parent = GameObject.Find("SameCursor_02").transform;
        }
    }
    public void cursorSameOff()
    {
        //親子関係削除
        this.gameObject.transform.parent = GameObject.Find("CursorCanvas").transform;
    }
}
