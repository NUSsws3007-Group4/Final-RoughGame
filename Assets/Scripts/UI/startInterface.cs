using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startInterface : MonoBehaviour
{

    private Animator anim;
    private bool Loading = false;
    private float loadTimer = 0f;
    public GameObject cover;
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
        {
            cover.SetActive(true);
            cover.GetComponent<CanvasGroup>().alpha += 0.8f * Time.deltaTime;
        }
        if (loadTimer >= 4.0f)
            SceneManager.LoadScene("Level1");

    }
}
