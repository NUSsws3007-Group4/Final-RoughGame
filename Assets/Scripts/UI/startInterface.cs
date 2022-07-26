using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startInterface : MonoBehaviour
{

    private Animator anim;
    private bool Loading = false;
    private float loadTimer = 0f;
    public void Load()
    {
        anim.SetTrigger("Load");
        Loading = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Eyes_0").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Loading)
            loadTimer += Time.deltaTime;
        if (loadTimer >= 2.5f)
            SceneManager.LoadScene("Level1");

    }
}
