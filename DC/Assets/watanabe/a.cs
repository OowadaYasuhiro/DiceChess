using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class a : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject Button;
    [SerializeField] GameObject Button1;
    [SerializeField] GameObject Button2;
    [SerializeField] GameObject Piece;
    [SerializeField] GameObject Piece1;
    [SerializeField] GameObject Piece2;
    [SerializeField] GameObject Cursor;
    [SerializeField] GameObject Cursor1;
    [SerializeField] GameObject Cursor2;
    GameObject selectedObj;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(Button1);
        Piece.gameObject.SetActive(false);
        Piece1.gameObject.SetActive(false);
        Piece2.gameObject.SetActive(false);
        Cursor.gameObject.SetActive(false);
        Cursor1.gameObject.SetActive(false);
        Cursor2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        selectedObj = EventSystem.current.currentSelectedGameObject;
        if(Button == selectedObj)
        {
            Piece.gameObject.SetActive(true);
            Piece1.gameObject.SetActive(false);
            Piece2.gameObject.SetActive(false);
            Cursor.gameObject.SetActive(true);
            Cursor1.gameObject.SetActive(false);
            Cursor2.gameObject.SetActive(false);

        }
        if (Button1 == selectedObj)
        {
            Piece.gameObject.SetActive(false);
            Piece1.gameObject.SetActive(true);
            Piece2.gameObject.SetActive(false);
            Cursor.gameObject.SetActive(false);
            Cursor1.gameObject.SetActive(true);
            Cursor2.gameObject.SetActive(false);
        }
        if (Button2 == selectedObj)
        {
            Piece.gameObject.SetActive(false);
            Piece1.gameObject.SetActive(false);
            Piece2.gameObject.SetActive(true);
            Cursor.gameObject.SetActive(false);
            Cursor1.gameObject.SetActive(false);
            Cursor2.gameObject.SetActive(true);

        }
    }
}
