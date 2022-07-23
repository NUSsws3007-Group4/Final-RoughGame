using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class energybarcontrol : MonoBehaviour
{
    public int maxenergy=5;
    private const float alterspeed=3f;
    private const float effectalterspeed=2f;

    public Image energybarentity=null;
    public Image energybareffect=null;
    public Image energybarframe=null;
    public Image energyicon=null;
    public RectTransform canvasrt;

    private float energyvolume;
    private float effectvolume;
    private int targetvolume;
    private float entitydeltavolume;
    private float effectdeltavolume;
    private Vector2 entitysize;
    private Vector2 effectsize;
    private float rate;

    public Text showtext;
    public Text deltatext;
    private float timer;

    private Vector3 deltatextpos;
    private Color deltatextcolor;
    private float deltastaytime=1.5f;

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


    public void changemaxenergy(int delta)
    {
        maxenergy+=delta;
    }

    public float getvolume()
    {
        return targetvolume;
    }

    public void setvolume(int delta)
    {
        if(delta<0) delta=0;
        if(delta>maxenergy) delta=maxenergy;
        targetvolume=delta;
    }

    public void increasevolume(int delta)
    {
        targetvolume=targetvolume+delta;

        showtext.text=inttostring(targetvolume);
        timer=deltastaytime;
        deltatext.text="+"+inttostring(delta);
        deltatext.color=new Color(0f,1f,0f,0f);
        deltatext.gameObject.SetActive(true);
        deltatext.GetComponent<RectTransform>().anchoredPosition=new Vector3(10f,-20f,0f);
        deltatextpos=new Vector3(10f,-20f,0f);
        deltatextcolor=new Color(0f,1f,0,0f);

        if(targetvolume<0) targetvolume=0;
        if(targetvolume>maxenergy) targetvolume=maxenergy;
        if(targetvolume>energyvolume) entitydeltavolume=alterspeed; else entitydeltavolume=-alterspeed;

    }

    public void decreasevolume(int delta)
    {
        targetvolume=targetvolume-delta;

        showtext.text=inttostring(targetvolume);
        timer=deltastaytime;
        deltatext.text="-"+inttostring(delta);
        deltatext.color=new Color(1f,0f,0f,0f);
        deltatext.gameObject.SetActive(true);
        deltatext.GetComponent<RectTransform>().anchoredPosition=new Vector3(10f,-20f,0f);
        deltatextpos=new Vector3(10f,-20f,0f);
        deltatextcolor=new Color(1f,0f,0,0f);

        if(targetvolume<0) targetvolume=0;
        if(targetvolume>maxenergy) targetvolume=maxenergy;
        if(targetvolume>energyvolume) entitydeltavolume=alterspeed; else entitydeltavolume=-alterspeed;
    }

    public void setsize()
    {
        entitysize=canvasrt.sizeDelta;
        entitysize.x/=5f;
        entitysize.y/=30f;
        energybarframe.GetComponent<RectTransform>().sizeDelta=entitysize;        
        energybarentity.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.x*energyvolume/maxenergy,entitysize.y);
        energybareffect.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.x*effectvolume/maxenergy,entitysize.y);

        showtext.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.y*4f,entitysize.y);
        deltatext.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.y*4f,entitysize.y);
        showtext.fontSize=(int)(entitysize.y*8.5f/10f);
        deltatext.fontSize=showtext.fontSize;
        //energybarframe.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        //energybarentity.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        //energybareffect.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        entitysize.x=entitysize.y;
        energyicon.GetComponent<RectTransform>().sizeDelta=entitysize;
        energyicon.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y,-entitysize.y*2-10f,0f);

        entitysize=energybarentity.GetComponent<RectTransform>().sizeDelta;
        effectsize=energybareffect.GetComponent<RectTransform>().sizeDelta;
        rate=energybarframe.GetComponent<RectTransform>().sizeDelta.x/maxenergy;
    }

    void Start()
    {
        maxenergy=10;
        //initiate size and position
        energyvolume=maxenergy;
        effectvolume=maxenergy;
        targetvolume=maxenergy;

        showtext.text=inttostring(maxenergy);
        deltatext.text="";
        deltatext.gameObject.SetActive(false);

        setsize();
        entitydeltavolume=0f;
        effectdeltavolume=0f;
        entitysize=energybarframe.GetComponent<RectTransform>().sizeDelta;
        effectsize=entitysize;
        rate=entitysize.x/maxenergy;
        timer=0f;
    }
    
    void Update()
    {
        if(energyvolume!=targetvolume)
        {
            energyvolume+=entitydeltavolume*Time.unscaledDeltaTime;
            if((entitydeltavolume>0 && energyvolume>targetvolume) || (entitydeltavolume<0 && energyvolume<targetvolume))
            {
                energyvolume=targetvolume;
            }
        }
        if(effectvolume!=energyvolume)
        {
            if(energyvolume<effectvolume) effectdeltavolume=-effectalterspeed; else effectdeltavolume=effectalterspeed;
            effectvolume+=effectdeltavolume*Time.unscaledDeltaTime;
            if(energyvolume==targetvolume && ((effectdeltavolume>0 && effectvolume>energyvolume) || (effectdeltavolume<0 && effectvolume<energyvolume)))
            {
                effectvolume=energyvolume;
            }
        }

        entitysize.x=energyvolume*rate;
        effectsize.x=effectvolume*rate;
        energybarentity.GetComponent<RectTransform>().sizeDelta=entitysize;
        energybareffect.GetComponent<RectTransform>().sizeDelta=effectsize;

        if(timer>0)
        {
            timer-=Time.unscaledDeltaTime;
            if(timer>=deltastaytime-0.5f)
            {
                deltatextpos.y+=Time.unscaledDeltaTime*(10f/0.5f);
                deltatextcolor.a+=Time.unscaledDeltaTime*(1f/0.5f);
                deltatext.GetComponent<RectTransform>().anchoredPosition=deltatextpos;
                deltatext.color=deltatextcolor;
            }
            else
            {
                deltatext.GetComponent<RectTransform>().anchoredPosition=new Vector3(10f,0f,0f);
                deltatextcolor.a=1f;
                deltatext.color=deltatextcolor;
            }
        }
        if(timer<0)
        {
            timer=0;
            deltatext.gameObject.SetActive(false);
        }
    }
}