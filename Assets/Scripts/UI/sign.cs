using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sign : MonoBehaviour
{
    public GameObject dialogbox;
    public Text dialogboxtext;
    public string signtext;
    private bool playerinsign=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && playerinsign)
        {
            dialogboxtext.text = signtext;
            dialogbox.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==8)
        playerinsign = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
         playerinsign = false;
            dialogbox.SetActive(false); 
    }
}
