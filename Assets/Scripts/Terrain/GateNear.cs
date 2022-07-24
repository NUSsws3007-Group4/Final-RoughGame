using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateNear : MonoBehaviour
{
    private bagmanager bgm;

    void Start()
    {
        bgm = GameObject.Find("Canvas").GetComponent<bagmanager>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bgm.nearthedoor = true;
        }
    }

    void OnTriggeExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bgm.nearthedoor = false;
        }
    }
}
