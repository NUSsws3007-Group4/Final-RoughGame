using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Camera mTheCamera;
    public Bounds camerabounds;
    public GameObject myhero;
    private float ymax;
    private float xmax;
    private float xsize;
    private float ysize;
    private Vector3 cameracentre;
    GameObject newenemy;

    public void exitgame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public Vector3 getheropos()
    {
        return myhero.transform.localPosition;
    }

    public string inttostring(int k)
    {
        string m = "";
        if (k == 0) m = "0";
        while (k > 0)
        {
            m = (char)(k % 10 + 48) + m;
            k /= 10;
        }
        return m;
    }

    public bool boundsintersect(Bounds b1, Bounds b2)
    {
        return (b1.min.x < b2.max.x) && (b1.max.x > b2.min.x) && (b1.min.y < b2.max.y) && (b1.max.y > b2.min.y);
    }

    public bool boundscontainspoint(Bounds b, Vector3 pt)
    {
        return ((b.min.x < pt.x) && (b.max.x > pt.x) && (b.min.y < pt.y) && (b.max.y > pt.y));
    }
    /*
    public void createnewenemy()
    {
        newenemy=Instantiate(Resources.Load("enemy") as GameObject);
        float xrt=Random.Range(-0.9f,0.9f);
        float yrt=Random.Range(-0.9f,0.9f);
        float newx=gameObject.GetComponent<managerbehavior>().xmax;
        float newy=gameObject.GetComponent<managerbehavior>().ymax;
        Vector3 cent=gameObject.GetComponent<managerbehavior>().cameracentre;

        cent.x+=xrt*newx;
        cent.y+=yrt*newy;
        cent.z=0;
        newenemy.transform.localPosition=cent;
        
        xrt=Random.Range(0,360);
        cent.x=0;
        cent.y=0;
        cent.z=xrt;
        newenemy.transform.rotation=Quaternion.Euler(cent);
    }
    */

    void Awake()
    {
        mTheCamera = Camera.main.GetComponent<Camera>();
        ymax = mTheCamera.orthographicSize;
        xmax = mTheCamera.orthographicSize * mTheCamera.aspect;
        xsize = 2 * xmax;
        ysize = 2 * ymax;
        cameracentre = mTheCamera.transform.position;
        cameracentre.z = 0f;
        camerabounds.center = cameracentre;
        camerabounds.size = new Vector3(xsize, ysize, 2f);
    }

    void Start()
    {

    }

    void Update()
    {
    }
}
