using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour
{
    public bool[] switchLst = { false, false, false, false, false };
    public GameObject p1, p2, p3, p4,l1,l2,l3,l4,l5;
    public bool solved;
    public Sprite triggered;
    private GameObject s, keyArea;
    // Start is called before the first frame update
    void Start()
    {
        solved = false;
        s=GameObject.Find("Switch");
    }

    // Update is called once per frame
    void Update()
    {
        p1.SetActive(!switchLst[1]);
        p2.SetActive(!switchLst[2]);
        p3.SetActive(!switchLst[3]);
        p4.SetActive(!switchLst[4]);
        l4.SetActive(switchLst[4]);
        l3.SetActive(switchLst[4] && switchLst[3]);
        l2.SetActive(switchLst[4] && switchLst[3] && switchLst[2]);
        l1.SetActive(switchLst[4] && switchLst[3] && switchLst[2] && switchLst[1]);
        if (switchLst[4] && switchLst[1] && switchLst[2] && switchLst[3])
        {
            solved = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = triggered;

            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

}
