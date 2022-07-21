using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mousedrag : MonoBehaviour, IDragHandler
{
    private RectTransform rt;
    public Vector3 offset;
    public bool down;

    void Start()
    {
        rt=GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            Vector3 pos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt,eventData.position,eventData.pressEventCamera,out pos))
            {
                rt.position=pos+offset;
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            offset=rt.position-Input.mousePosition;
        }
    }
}