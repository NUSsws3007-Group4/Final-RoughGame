using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    //public GameObject doublejumpsign;
    //public GameObject doublejumptutorial;
    //public cameramanagerbehavior cm;
    // Start is called before the first frame update
    public bool destroied;

    void Start()
    {
        destroied=false;
        //doublejumptutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer==8)
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
            Debug.Log("Trap Destoryed");
            gameObject.SetActive(false);
            destroied=true;
            //Destroy(transform.gameObject);
            //if(doublejumpsign!=null)
            //{
            //    doublejumptutorial.SetActive(true);
            //    cm.startfocus(doublejumpsign,3f,doublejumptutorial);
            //}
        }
    }
}
