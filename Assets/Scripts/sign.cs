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
            Debug.Log("����e��");
            dialogboxtext.text = signtext;
            dialogbox.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("���뷶Χ");
        playerinsign = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�뿪��Χ");
        playerinsign = false;
        dialogbox.SetActive(false);
    }
}
