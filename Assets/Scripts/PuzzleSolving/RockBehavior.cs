using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour
{
    public bool l1, l2, l3, l4, solved;
    private GameObject s, keyArea;
    // Start is called before the first frame update
    void Start()
    {
        l1 = false;
        l2 = false;
        l3 = false;
        l4 = false;
        solved = false;
        s=GameObject.Find("Switch");
    }

    // Update is called once per frame
    void Update()
    {
        if (l1 && l2 && l3 && l4){
            solved = true;
            s.gameObject.SetActive(false);
            }
    }

}
