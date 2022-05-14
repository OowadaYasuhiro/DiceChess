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
    GameObject selectedObj;

    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(Button1);
        Piece.gameObject.SetActive(false);
        Piece1.gameObject.SetActive(false);
        Piece2.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        selectedObj = EventSystem.current.currentSelectedGameObject;
        if(Button == selectedObj)
        {
            Button.GetComponent<buttonblink>().setBool();
            Button1.GetComponent<buttonblink>().notBool();
            Button2.GetComponent<buttonblink>().notBool();
            Piece.gameObject.SetActive(true);
            Piece1.gameObject.SetActive(false);
            Piece2.gameObject.SetActive(false);
            
        }
        if (Button1 == selectedObj)
        {
            Button.GetComponent<buttonblink>().notBool();
            Button1.GetComponent<buttonblink>().setBool();
            Button2.GetComponent<buttonblink>().notBool();
            Piece.gameObject.SetActive(false);
            Piece1.gameObject.SetActive(true);
            Piece2.gameObject.SetActive(false);
            
        }
        if (Button2 == selectedObj)
        {
            Button.GetComponent<buttonblink>().notBool();
            Button1.GetComponent<buttonblink>().notBool();
            Button2.GetComponent<buttonblink>().setBool();
            Piece.gameObject.SetActive(false);
            Piece1.gameObject.SetActive(false);
            Piece2.gameObject.SetActive(true);
            
        }

        
    }
}
