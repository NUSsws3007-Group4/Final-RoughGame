using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject sign;
    public GameObject tutorialbox;
    public cameramanagerbehavior cm;
    public GameObject focusassis;
    // Start is called before the first frame update

    public void showBox()
    {
        focusassis.transform.position = sign.transform.position;
        if (tutorialbox)
        {
            tutorialbox.SetActive(true);
            cm.startfocus(focusassis, 3f, tutorialbox);
        }
    }

    void Start()
    {
        if(tutorialbox) tutorialbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

}
