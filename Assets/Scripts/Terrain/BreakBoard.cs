using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoard : MonoBehaviour
{
    public GameObject dashsign;
    public GameObject dashtutorialbox;
    public cameramanagerbehavior cm;
    public GameObject focusassis;
    // Start is called before the first frame update
    void Start()
    {
        dashtutorialbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("attack:" + other.gameObject.layer);
        if (other.gameObject.layer == 19)
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
            focusassis.transform.position=dashsign.transform.position;
            dashtutorialbox.SetActive(true);
            cm.startfocus(focusassis,3f,dashtutorialbox);
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("attack:" + other.gameObject.layer);
        if (other.gameObject.layer == 19)
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }


}
