using UnityEngine;
using Yarn.Unity;

public class EndingJudgement : MonoBehaviour
{
    public int usedCount = 0;
    public int friendAttacked = 0;
    public bool attackFriends = false;
    public bool f1 = false, f2 = false, f3 = false;
    public bool friendshipTipSlimeKilled = false;
    public int ending;
    public bool BossDead = false;
    private DialogueRunner dialogueRunner;
    public GameObject cover;
    public Color c;
    public float status;
    public GameObject e0, e1, e2;

    void Start()
    {
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        cover.GetComponent<CanvasGroup>().alpha = 0;
        cover.SetActive(false);
        e0.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }
    void Update()
    {
        dialogueRunner.GetComponent<VariableStorageBehaviour>().TryGetValue<float>("$ready", out status);
        if (BossDead)
        {
            BossDead = false;
            dialogueRunner.Stop();
            switch (ending)
            {
                case 6:
                    dialogueRunner.StartDialogue("NormalEnding");
                    break;
                case 3:
                    dialogueRunner.StartDialogue("SlaughterEnding");
                    break;
                case 2:
                    dialogueRunner.StartDialogue("RespawnEnding");
                    break;
                case 1:
                    dialogueRunner.StartDialogue("FakeFriendlyEnding");
                    break;
                case 0:
                    dialogueRunner.StartDialogue("TrueFriendlyEnding");
                    break;
            }
        }
        if (status == 4)
        {
            cover.SetActive(true);
            if (cover.GetComponent<CanvasGroup>().alpha <= 1)
            {
                cover.GetComponent<CanvasGroup>().alpha += 0.3f * Time.deltaTime;
            }
        }
        if (status == 5)
        {
            switch (ending)
            {
                case 0:
                    e0.SetActive(true);
                    break;
                case 1:
                    e1.SetActive(true);
                    break;
                case 2:
                    e2.SetActive(true);
                    break;
            }
        }
    }
}
