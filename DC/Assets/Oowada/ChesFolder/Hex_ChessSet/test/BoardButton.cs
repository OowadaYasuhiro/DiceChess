using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardButton : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    public Button FirstSelectButton;
    // Start is called before the first frame update
    void Start()
    {
        FirstSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        //eventSystem.currentSelectedGameObject
    }
}
