using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour
{
    public bool[] switchLst = { false, false, false, false };
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

        if (switchLst[0] && switchLst[1] && switchLst[2] && switchLst[3])
        {
            solved = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = triggered;
            GameObject.Find("SuccessGate").SetActive(false);
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

}
