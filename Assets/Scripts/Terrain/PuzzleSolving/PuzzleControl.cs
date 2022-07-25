using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleControl : MonoBehaviour
{
    public Camera cam;
    private bool puzzleEnabled = false;
    private GameObject muim;
    /// <summary>
    /// 设置是否能解密
    /// </summary>
    /// <param name="_enabled">
    /// 是否启用解密
    /// </param>
    public void setPuzzleOpen(bool _enabled)
    {
        puzzleEnabled = _enabled;
    }
    // Start is called before the first frame update
    void Start()
    {
        muim=GameObject.Find("UImanager");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform i in transform)
        {
            i.gameObject.SetActive(puzzleEnabled);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 colliNormal = collision.contacts[0].normal;
            if (colliNormal.y < -0.9f)
            {
                cam.depth = 6;
                muim.GetComponent<bloodbarcontrol>().setactiveall(false);
                muim.GetComponent<energybarcontrol>().setactiveall(false);
                muim.GetComponent<coincontrol>().setactiveall(false);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            muim.GetComponent<bloodbarcontrol>().setactiveall(true);
            muim.GetComponent<energybarcontrol>().setactiveall(true);
            muim.GetComponent<coincontrol>().setactiveall(true);
            cam.depth = -10;
        }
    }
}
