using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startInterface : MonoBehaviour
{

    private Animator anim;
    public void Load()
    {
        anim.SetTrigger("Load");
        Invoke("LoadScene", 2.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Eyes_0").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
