using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Button���g�p���邽�ߒǉ�

public class buttonblink : MonoBehaviour
{
    
    // �{�^���̃R���|�[�l���g
    Button button;

    // �J�E���^
    int cnt;

    // �_�ł̑�����ݒ�(60�̏ꍇ�C30�t���[�����ƂɐF���ς��)
    public int MAX_COUNT = 60;

    // �_�ŐF�̐ݒ�
    public List<Color> colors = new List<Color>() { new Color(1, 1, 1, 1), new Color(1, 1, 1, 0) };


     bool Blink = false;
    // Start is called before the first frame update
    void Start()
    {
        //�{�^���̃R���|�[�l���g��ݒ�
        button = GetComponent<Button>();
        //�J�E���^�̏����l��0�ɐݒ�
        cnt = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        {
            cnt++;
            cnt %= MAX_COUNT;
            var cls = button.colors;
            cls.normalColor = colors[cnt / (MAX_COUNT / colors.Count)];
            button.colors = cls;
        }
        
    }

    public void setBool()
    {
        Blink = true;
    }

    public void notBool()
    {
        Blink = false;
    }
}
