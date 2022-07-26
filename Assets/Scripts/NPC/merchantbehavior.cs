using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class merchantbehavior : MonoBehaviour
{
    private bool flag=true;
    public GameObject dialoguebox;
    private float timer;
    private cameramanagerbehavior cm;
    private GameObject hero;
    private storemanager stg;
    private bagmanager bg;
    private DialogueRunner dialogueRunner;

    void Start()
    {
        timer=0f;
        flag=true;
        if(dialoguebox!=null) dialoguebox.SetActive(false);
        if(!hero)
        {
            hero = GameObject.Find("hero");
        }
        if(!stg||!bg)
        {
            GameObject canvas = GameObject.Find("Canvas");
            stg = canvas.GetComponent<storemanager>();
            bg = canvas.GetComponent<bagmanager>();
            GameObject cam = GameObject.Find("Cameramanager");
            cm = cam.GetComponent<cameramanagerbehavior>();
        }
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag=="Player")
        {
            stg.canopenstore=true;
            bg.cansell=true;
            if(flag)
            {
                flag=false;
                if(dialoguebox!=null) dialoguebox.SetActive(true);
                cm.startfocus(gameObject,3f,dialoguebox);
                dialogueRunner.Stop();
                dialogueRunner.StartDialogue("Merchant");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag=="Player")
        {
            stg.canopenstore=false;
            bg.cansell=false;
            stg.closemystore();
            bg.judgebutton();
        }
    }
}
