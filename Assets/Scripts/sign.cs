using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sign : MonoBehaviour
{
    public GameObject dialogbox;
    public Text dialogboxtext;
    public string signtext;
    private bool playerinsign;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && playerinsign)
        {
            Debug.Log("按下e键");
            dialogboxtext.text = signtext;
            dialogbox.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("进入范围");
        playerinsign = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("离开范围");
        playerinsign = false;
        dialogbox.SetActive(false);
    }
}
