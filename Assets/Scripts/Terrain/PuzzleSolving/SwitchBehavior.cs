using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour
{
    public int switchCode1, switchCode2, switchCode3;
    public RockBehavior con;
    bool isStay = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStay&& Input.GetKeyDown(KeyCode.E))
        {
            con.switchLst[switchCode1] = !con.switchLst[switchCode1];
            con.switchLst[switchCode2] = !con.switchLst[switchCode2];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            isStay = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            isStay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            isStay = false;
    }
}
