using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class energybarcontrol : MonoBehaviour
{
    private float maxenergy=10f;
    private const float alterspeed=3f;
    private const float effectalterspeed=2f;

    public Image energybarentity=null;
    public Image energybareffect=null;
    public Image energybarframe=null;
    public Image energyicon=null;
    public RectTransform canvasrt;

    private float energyvolume;
    private float effectvolume;
    private float targetvolume;
    private float entitydeltavolume;
    private float effectdeltavolume;
    private Vector2 entitysize;
    private Vector2 effectsize;
    private float rate;

    public void changemaxenergy(float delta)
    {
        maxenergy+=delta;
    }

    public float getvolume()
    {
        return targetvolume;
    }

    public void setvolume(float delta)
    {
        if(delta<0f) delta=0f;
        if(delta>maxenergy) delta=maxenergy;
        targetvolume=delta;
    }

    public void increasevolume(float delta)
    {
        targetvolume=targetvolume+delta;
        if(targetvolume<0f) targetvolume=0f;
        if(targetvolume>maxenergy) targetvolume=maxenergy;
        if(targetvolume>energyvolume) entitydeltavolume=alterspeed; else entitydeltavolume=-alterspeed;

    }

    public void decreasevolume(float delta)
    {
        targetvolume=targetvolume-delta;
        if(targetvolume<0f) targetvolume=0f;
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
        energybarframe.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        energybarentity.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        energybareffect.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y*2-10f,0f);
        entitysize.x=entitysize.y;
        energyicon.GetComponent<RectTransform>().sizeDelta=entitysize;
        energyicon.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y,-entitysize.y*2-10f,0f);

        entitysize=energybarentity.GetComponent<RectTransform>().sizeDelta;
        effectsize=energybareffect.GetComponent<RectTransform>().sizeDelta;
        rate=energybarframe.GetComponent<RectTransform>().sizeDelta.x/maxenergy;
    }

    void Start()
    {
        //initiate size and position
        setsize();

        energyvolume=maxenergy;
        effectvolume=maxenergy;
        targetvolume=maxenergy;
        entitydeltavolume=0f;
        effectdeltavolume=0f;
        entitysize=energybarframe.GetComponent<RectTransform>().sizeDelta;
        effectsize=entitysize;
        rate=entitysize.x/maxenergy;
    }

    void Update()
    {
        if(energyvolume!=targetvolume)
        {
            energyvolume+=entitydeltavolume*Time.smoothDeltaTime;
            if((entitydeltavolume>0 && energyvolume>targetvolume) || (entitydeltavolume<0 && energyvolume<targetvolume))
            {
                energyvolume=targetvolume;
            }
        }
        if(effectvolume!=energyvolume)
        {
            if(energyvolume<effectvolume) effectdeltavolume=-effectalterspeed; else effectdeltavolume=effectalterspeed;
            effectvolume+=effectdeltavolume*Time.smoothDeltaTime;
            if(energyvolume!=targetvolume && ((effectdeltavolume>0 && effectvolume>energyvolume) || (effectdeltavolume<0 && effectvolume<energyvolume)))
            {
                effectvolume=energyvolume;
            }
        }

        entitysize.x=energyvolume*rate;
        effectsize.x=effectvolume*rate;
        energybarentity.GetComponent<RectTransform>().sizeDelta=entitysize;
        energybareffect.GetComponent<RectTransform>().sizeDelta=effectsize;
    }
}