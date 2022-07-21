using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tips: MonoBehaviour
{
    public GameObject dialogbox;
    public Text dialogboxtext;
    public string signtext;
    private bool playerinsign1=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogbox.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        dialogboxtext.text = signtext;
        dialogbox.SetActive(true);
        playerinsign1 = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("离开范围");
        playerinsign1 = false;
    }
}
