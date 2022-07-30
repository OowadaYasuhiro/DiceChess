using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPanelController : MonoBehaviour
{
    private GameObject[] parentObj;
    [SerializeField]private int myNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetParent();
    }

    public void SetParent()
    {

        parentObj = GameObject.FindGameObjectsWithTag("ParentPanel");
        foreach(GameObject Obj in parentObj)
        {
            var animCon = Obj.GetComponent<animation>();
            if(animCon.ChessPanelNumGet == myNum)
            {
                this.gameObject.transform.parent = Obj.transform;
                this.gameObject.transform.position = Obj.transform.position;
                break;
            }
            else
            {
                this.gameObject.transform.parent = GameObject.Find("ChildPanelGroup").transform;
                //this.gameObject.transform.position = Obj.transform.position;
                
            }
        }
    }
}
