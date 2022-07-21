using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setpos : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public RectTransform canvasrt;

    public void setposition(float x,float y)
    {
        panel.GetComponent<RectTransform>().anchoredPosition=new Vector2(x,y);
    }

    public void setlefttop()
    {
        setposition(0f,0f);
        gameObject.GetComponent<bloodbarcontrol>().setsize();
        gameObject.GetComponent<energybarcontrol>().setsize();
        gameObject.GetComponent<coincontrol>().setsize();
    }

    public void setrighttop()
    {
        setposition(canvasrt.sizeDelta.x*4f/5f-canvasrt.sizeDelta.y*3f/30f-10f,0f);
        gameObject.GetComponent<bloodbarcontrol>().setsize();
        gameObject.GetComponent<energybarcontrol>().setsize();
        gameObject.GetComponent<coincontrol>().setsize();
    }

    public void setleftbottom()
    {
        setposition(0f,20f-canvasrt.sizeDelta.y*25f/30f);
        gameObject.GetComponent<bloodbarcontrol>().setsize();
        gameObject.GetComponent<energybarcontrol>().setsize();
        gameObject.GetComponent<coincontrol>().setsize();
    }

    public void setrightbottom()
    {
        setposition(canvasrt.sizeDelta.x*4f/5f-canvasrt.sizeDelta.y*3f/30f-10f,20f-canvasrt.sizeDelta.y*25f/30f);
        gameObject.GetComponent<bloodbarcontrol>().setsize();
        gameObject.GetComponent<energybarcontrol>().setsize();
        gameObject.GetComponent<coincontrol>().setsize();
    }

    void Start()
    {
        setlefttop();
    }

    void Update()
    {
        
    }
}
