using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class a1 : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.StartDialogue("HelloYarn");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
