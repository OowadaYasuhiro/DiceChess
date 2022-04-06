using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    GameSceneDirector sceneDirector;

    enum BattleMode
    {
        NONE = 0,
        BATTLE_SET = 1,
        DICE,
        ATTACK_TIME
    }

    BattleMode battleMode;

    Animator panelAnim;
    Animator textAnim;

    int rnd;

    // Start is called before the first frame update
    void Start()
    {
        sceneDirector = GameObject.Find("Main Camera").GetComponent<GameSceneDirector>();
        panelAnim = GameObject.Find("Panel").GetComponent<Animator>();
        textAnim = GameObject.Find("Text").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(battleMode) {
            case BattleMode.BATTLE_SET:
                battleSetMode();
                break;
            case BattleMode.DICE:
                diceTime();
                break;
            case BattleMode.ATTACK_TIME:

                break;
        }
    }
    public void switchMode1() {
        battleMode = BattleMode.BATTLE_SET;
    }


    public void battleSetMode() {
        panelAnim.SetTrigger("in");
        textAnim.SetTrigger("in");

        Invoke("switchMode2", 1f);
    }

    public void diceTime() {
        if(Input.GetMouseButton(0)) {
            rnd = Random.Range(1,11);

            battleMode = BattleMode.ATTACK_TIME;
        }
    }

    public void attackTime() {

    }

    void switchMode2() {
        battleMode = BattleMode.DICE;
    }

}
