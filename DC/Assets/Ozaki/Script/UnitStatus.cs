using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    //HP
    [SerializeField]
    private int hp = 10;

    public void SetHp(int hp) {
        this.hp = hp;
    }

    public int GetHp() {
        return hp;
    }
}
