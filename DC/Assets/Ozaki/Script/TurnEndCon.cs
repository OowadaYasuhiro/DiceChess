using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndCon : MonoBehaviour
{

    GameSceneDirector sceneDirector;
    // Start is called before the first frame update
    void Start()
    {
        sceneDirector = GameObject.Find("SceneDirector").GetComponent<GameSceneDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnEndButton() {
        Debug.Log("ボタン押された");
    }
}
