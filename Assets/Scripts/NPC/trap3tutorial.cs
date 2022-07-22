using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap3tutorial : MonoBehaviour
{
    public GameObject doublejumptutorial;
    public cameramanagerbehavior cm;
    public GameObject trap3;
    public GameObject followassis;

    private bool start;
    // Start is called before the first frame update
    void Start()
    {
        doublejumptutorial.SetActive(false);
        start=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(trap3.GetComponent<Destructable>().destroied && start)
        {
            start=false;
            doublejumptutorial.SetActive(true);
            followassis.transform.position=gameObject.transform.position;
            cm.startfocus(followassis,3f,doublejumptutorial);
        }
    }
}
