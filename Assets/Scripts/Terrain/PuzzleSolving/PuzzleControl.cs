using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleControl : MonoBehaviour
{
    public Camera cam;
    private bool puzzleEnabled = false;
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
                cam.depth = 6;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            cam.depth = -10;
        }
    }
}
