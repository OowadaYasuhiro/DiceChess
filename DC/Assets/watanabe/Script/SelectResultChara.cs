using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine .UI;
using UnityEngine.EventSystems;


public class SelectResultChara : MonoBehaviour
{
    public void OnSelected(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name +"was selected");
    }
}
