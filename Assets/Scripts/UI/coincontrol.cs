using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coincontrol : MonoBehaviour
{
    public Text showtext;
    public Image coinicon;
    public RectTransform canvasrt;
    private Vector2 entitysize;
    private int currentcoinnumber; //for animation
    private int coinnumber; //real number
    private int delta;
    private float speedct;

    private string inttostring(int k)
    {
        string m="";
        if(k==0) m="0";
        while(k>0)
        {
            m=(char)(k%10+48)+m;
            k/=10;
        }
        return m;
    }

    private void getdelta()
    {
        if(coinnumber>currentcoinnumber)
        {
            delta=1;
        }
        else if(coinnumber<currentcoinnumber)
        {
            delta=-1;
        }
        else
        {
            delta=0;
        }
    }

    public void earn(int num)
    {
        coinnumber+=num;
        getdelta();
    }

    public bool pay(int num)
    {
        if(coinnumber>=num)
        {
            coinnumber-=num;
            getdelta();
            return true;
        }
        else
        {
            getdelta();
            return false;
        }
    }

    public void setcoinnum(int num)
    {
        coinnumber=num;
        getdelta();
    }

    public int getcoinnum()
    {
        return coinnumber;
    }

    public void setsize()
    {
        entitysize=canvasrt.sizeDelta;
        float theight=entitysize.y/=30f;
        showtext.GetComponent<RectTransform>().sizeDelta=new Vector2(theight*4f,theight);
        showtext.GetComponent<RectTransform>().anchoredPosition=new Vector3(theight*2f+10f,-theight*3-20f,0f);
        showtext.fontSize=(int)(theight*9f/10f);
        coinicon.GetComponent<RectTransform>().sizeDelta=new Vector2(theight,theight);
        coinicon.GetComponent<RectTransform>().anchoredPosition=new Vector3(theight,-theight*3-20f,0f);
    }

    void Start()
    {
        setsize();
        coinnumber=500;
        currentcoinnumber=500;
        showtext.text=inttostring(currentcoinnumber);
        delta=0;
        speedct=0;
    }

    void Update()
    {
        if(currentcoinnumber!=coinnumber)
        {
            speedct+=Time.smoothDeltaTime;
            if(speedct>0.02f)
            {
                currentcoinnumber+=delta;
                showtext.text=inttostring(currentcoinnumber);
                speedct=0f;
            }
        }
        else
        {
            speedct=0f;
        }
    }
}
