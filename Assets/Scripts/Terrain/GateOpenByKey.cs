using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpenByKey : MonoBehaviour
{
    private bagmanager bgm;
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.Find("Canvas").GetComponent<bagmanager>();
    }

    // Update is called once per frame

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && bgm.key.itemnum > 0)
        {
            bgm.key.itemnum--;
            bgm.refreshitem(bgm.key);
            Destroy(transform.gameObject);
        }
    }
}
