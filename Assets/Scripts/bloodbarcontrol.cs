using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bloodbarcontrol : MonoBehaviour
{
    public float maxblood=5f;
    private const float alterspeed=3f;
    private const float effectalterspeed=2f;

    public Image bloodbarentity=null;
    public Image bloodbareffect=null;
    public Image bloodbarframe=null;
    public Image bloodicon=null;
    public RectTransform canvasrt;

    private float bloodvolume;
    private float effectvolume;
    private float targetvolume;
    private float entitydeltavolume;
    private float effectdeltavolume;
    private Vector2 entitysize;
    private Vector2 effectsize;
    private float rate;

    public void changemaxblood(float delta)
    {
        maxblood+=delta;
    }

    public float getvolume()
    {
        return targetvolume;
    }

    public void setvolume(float delta)
    {
        if(delta<0f) delta=0f;
        if(delta>maxblood) delta=maxblood;
        targetvolume=delta;
    }

    public void increasevolume(float delta)
    {
        targetvolume=targetvolume+delta;
        if(targetvolume<0f) targetvolume=0f;
        if(targetvolume>maxblood) targetvolume=maxblood;
        if(targetvolume>bloodvolume) entitydeltavolume=alterspeed; else entitydeltavolume=-alterspeed;

    }

    public void decreasevolume(float delta)
    {
        targetvolume=targetvolume-delta;
        if(targetvolume<0f) targetvolume=0f;
        if(targetvolume>maxblood) targetvolume=maxblood;
        if(targetvolume>bloodvolume) entitydeltavolume=alterspeed; else entitydeltavolume=-alterspeed;
    }

    public void setsize()
    {
        entitysize=canvasrt.sizeDelta;
        entitysize.x/=5f;
        entitysize.y/=30f;
        bloodbarframe.GetComponent<RectTransform>().sizeDelta=entitysize;
        bloodbarentity.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.x*bloodvolume/maxblood,entitysize.y);//entitysize*bloodvolume/maxblood;
        bloodbareffect.GetComponent<RectTransform>().sizeDelta=new Vector2(entitysize.x*effectvolume/maxblood,entitysize.y);
        bloodbarframe.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y,0f);      
        bloodbarentity.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y,0f);
        bloodbareffect.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y*2f+10f,-entitysize.y,0f);
        entitysize.x=entitysize.y;
        bloodicon.GetComponent<RectTransform>().sizeDelta=entitysize;
        bloodicon.GetComponent<RectTransform>().anchoredPosition=new Vector3(entitysize.y,-entitysize.y,0f);

        entitysize=bloodbarentity.GetComponent<RectTransform>().sizeDelta;
        effectsize=bloodbareffect.GetComponent<RectTransform>().sizeDelta;
        rate=bloodbarframe.GetComponent<RectTransform>().sizeDelta.x/maxblood;
    }

    void Start()
    {
        //initiate size and position
        bloodvolume=maxblood;
        effectvolume=maxblood;
        targetvolume=maxblood;
        setsize();
        entitydeltavolume=0f;
        effectdeltavolume=0f;
        entitysize=bloodbarframe.GetComponent<RectTransform>().sizeDelta;
        effectsize=entitysize;
        rate=entitysize.x/maxblood;
    }

    void Update()
    {
        if(bloodvolume!=targetvolume)
        {
            bloodvolume+=entitydeltavolume*Time.smoothDeltaTime;
            if((entitydeltavolume>0 && bloodvolume>targetvolume) || (entitydeltavolume<0 && bloodvolume<targetvolume))
            {
                bloodvolume=targetvolume;
            }
        }
        if(effectvolume!=bloodvolume)
        {
            if(bloodvolume<effectvolume) effectdeltavolume=-effectalterspeed; else effectdeltavolume=effectalterspeed;
            effectvolume+=effectdeltavolume*Time.smoothDeltaTime;
            if(bloodvolume!=targetvolume && ((effectdeltavolume>0 && effectvolume>bloodvolume) || (effectdeltavolume<0 && effectvolume<bloodvolume)))
            {
                effectvolume=bloodvolume;
            }
        }

        entitysize.x=bloodvolume*rate;
        effectsize.x=effectvolume*rate;
        bloodbarentity.GetComponent<RectTransform>().sizeDelta=entitysize;
        bloodbareffect.GetComponent<RectTransform>().sizeDelta=effectsize;
    }
}