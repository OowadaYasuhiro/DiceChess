using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] _effPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //prefabで設定したTransformの内容でエフェクトを出現させる
    public void setEff(int num) {
        Instantiate(_effPrefabs[num]);
    }

    //位置を決めてエフェクトを出現させる
    public void setPositionEff(int num, int x, int y, int z) {
        Instantiate(_effPrefabs[num], new Vector3(x,y,z), Quaternion.identity);
    }

    //誰かの位置に出現させる
    public void enemyPositionEff(int num, Vector3 target) {

        Instantiate(_effPrefabs[num], target, Quaternion.identity);
    }
}
