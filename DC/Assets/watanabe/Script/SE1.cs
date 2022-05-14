using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE1 : MonoBehaviour
{
    public AudioClip suond1;
    public AudioClip suond2;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //上
        if (Input.GetKey(KeyCode.UpArrow))
        {
            audioSource.PlayOneShot(suond1);
        }

        //下
        if (Input.GetKey(KeyCode.DownArrow))
        {
            audioSource.PlayOneShot(suond2);
        }
    }
}
