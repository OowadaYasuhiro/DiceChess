using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundController : MonoBehaviour
{
    private BGM _sound;
    private EventSystem _es;
    private GameObject g;

    // Start is called before the first frame update
    void Start()
    {
        _sound = GameObject.Find("SoundGameObject").GetComponent<BGM>();
        _es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        
        Invoke("playBGM", 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        decisionSE();

        moveSE();
    }

    public void decisionSE() {
        if(Input.GetButtonDown("Submit") && g.name != "MessageFrame") {
            _sound.playSE(1);
        }
        if(Input.GetButtonDown("Submit") && g.name == "SortieButton") {
            _sound.playSE(3);
        }
    }

    public void moveSE() {      
        if(_es.currentSelectedGameObject != g) {
            g = _es.currentSelectedGameObject;
            if(g.name == "MessageFrame") {
                _sound.playSE(2);
            } else {
                _sound.playSE(0);
            }
            
        }
    }

    public void playBGM() {
        _sound.playBGM(0);
    }
}
