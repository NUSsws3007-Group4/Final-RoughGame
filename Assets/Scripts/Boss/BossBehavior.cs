using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BossBehavior : MonoBehaviour
{

    private const float pi = 3.14159f;
    public bool triggered0 = false, triggered1 = false;
    private int bossHP = 200;//生命值
    private int bossShield = 200;//护盾值（不会恢复）
    private float bossSpeed = 5f;//移动速度
    private int iceThornNum = 0;//冰刺数目产生
    private float iceThornTimer = 0f;//冰刺攻击间隔计数器
    private float iceThornInterval = 0f;//冰刺攻击间隔设置
    //private bool coldnessProduce = false;//是否开始产生寒冷值
    private float distanceToPlayer = 0f;//Boss和Player之间的距离
    private float iceCrystalTimer = 0f;//冰棱攻击间隔计数器
    private float barrageTimer = 0f;//弹幕攻击计数器
    private bool isFlameAttack = false;//是否收到火焰攻击
    private float flameAttackTimer = 0f;//收到火焰攻击时间计数器
    private float recoverTimer = 0f;//回血计数器
    private bool stopReviveTag = false;//终止重生标记
    private float reviveTimer = 0f;//重生计数器
    private float peaceTimer = 0f;//躲避计数器
    private List<Vector3> iceThornPos = new List<Vector3>();//开设冰刺位置数组
    private int attackedCount = 0;//友好状态受击计数
    private float attackedTimer = 0f;
    private int playerAttackDamage = 0;

    private GameObject mHero, nextStage;
    private EndingJudgement judge;
    public Transform playerTransform;
    private Animator anim;
    public int endingNum;
    public float status;
    private DialogueRunner dialogueRunner;


    void Start()
    {
        mHero = GameObject.Find("hero");
        nextStage = GameObject.Find("BossPhase2");
        anim = GetComponent<Animator>();
        judge = mHero.GetComponent<EndingJudgement>();
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        transform.right = -transform.right;
    }

    void Update()
    {
        if (anim.GetBool("Attacked"))
        {
            attackedTimer += Time.deltaTime;
            if (attackedTimer >= 2f)
            {
                anim.SetBool("Attacked", false); attackedTimer = 0;
            }
        }
        dialogueRunner.GetComponent<VariableStorageBehaviour>().TryGetValue<float>("$ready", out status);
        endingNum = 6;//普通结局
        if (!triggered0 && status == 0)
        {
            triggered0 = true;
            dialogueRunner.Stop();
            if (mHero.GetComponent<HeroBehavior>().getFriendship() == 100)
            {
                dialogueRunner.StartDialogue("FriendlyEndingBegin");
                if (judge.f1 && judge.f2 && judge.f3)
                {
                    if (judge.usedCount < 5)
                        endingNum = 0;//真和平
                    else
                        endingNum = 1;//伪和平
                }
            }
            else
            {
                dialogueRunner.StartDialogue("FightEndingBegin");
                if (!judge.f1 && !judge.f2 && !judge.f3)
                {
                    endingNum = 2; //重生
                    if (judge.friendshipTipSlimeKilled || judge.friendAttacked >= 5)
                        endingNum = 3;//全灭
                }
            }

        }

        if (status == 1f)
        {
            if (endingNum <= 1)
            {
                peaceTimer += Time.deltaTime;
                if (peaceTimer >= 30f)
                {
                    triggered1 = true;
                    peaceTimer = -2000f;
                    dialogueRunner.Stop();
                    switch (endingNum)
                    {
                        case 0:
                            dialogueRunner.StartDialogue("TrueFriendlyEnding");
                            break;
                        case 1:
                            dialogueRunner.StartDialogue("FakeFriendlyEnding");
                            break;
                    }
                }
                if (!triggered1)
                {
                    iceThornNum = Random.Range(2, 3);
                    iceThornInterval = 1f;
                    IceThornAttack(iceThornNum, iceThornInterval);
                }
            }//和平线 躲避
            else if (bossHP > 0)
            {
                if (bossShield > 0)
                {
                    PhaseOne();//有护盾阶段
                }
                else if (bossShield == 0)
                {
                    Debug.Log("Phase 2");
                    PhaseTwo();//无护盾 移动阶段
                }
            }//打斗线 打第一阶段
        }
        if (status == 2f)
        {
            nextStage.SetActive(true);
            nextStage.GetComponent<BossPhase2>().endingNum = endingNum;
            BossDeath();
        }

    }

    private void PhaseOne()//有护盾阶段 不移动
    {
        iceThornNum = Random.Range(3, 5);
        iceThornInterval = 3f;
        IceThornAttack(iceThornNum, iceThornInterval);
    }

    private void PhaseTwo()//无护盾 移动阶段
    {
        //coldnessProduce = true;//开始产生寒冷值
        playerTransform = GameObject.Find("hero").transform;//取出player.transform
        distanceToPlayer = CalDistanceToPlayer();//计算距离以判定冰棱攻击

        if (distanceToPlayer >= 3f)
        {
            MovementToPlayer();//调用移动
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (distanceToPlayer >= 3.0f)
        {
            //Debug.Log("Attacking with Ice Crystal!");
            IceCrystalAttack();
        }
        else
        {
            iceThornNum = Random.Range(4, 6);
            iceThornInterval = 2f;
            IceThornAttack(iceThornNum, iceThornInterval);
        }
    }

    private void BossHurt(int damage)
    {
        if (bossShield > 0)
        {
            bossShield = bossShield > damage ? bossShield - damage : 0;
        }
        else if (bossShield == 0)
        {
            bossHP = bossHP > damage ? bossHP - damage : 0;
        }
        if (bossHP <= 0)
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("Phase1Beaten");
        }
    }

    private void BossDeath()
    {
        Destroy(transform.gameObject);
    }

    private void IceThornAttack(int spawnNum, float spawnInterval)
    {
        iceThornTimer += Time.smoothDeltaTime;
        if (iceThornTimer >= spawnInterval)
        {
            iceThornTimer = 0f;//清空计时器

            iceThornPos.Clear();//清空
            playerTransform = GameObject.Find("hero").transform;

            WarningSignSpawn(spawnNum);
            anim.SetTrigger("Attacking");
            Invoke("IceThornSpawn", 1f);
        }
    }

    private void WarningSignSpawn(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)//生成警告标志
        {
            GameObject warningSign = Instantiate(Resources.Load("Prefabs/WarningSign") as GameObject);
            Vector3 p = playerTransform.localPosition;
            p.x += Random.Range(-10.0f, 10.0f);
            p.y += 1f;
            warningSign.transform.localPosition = p;
            iceThornPos.Add(p);
        }
    }

    private void IceThornSpawn()
    {
        for (int i = 0; i < iceThornPos.Count; i++)//生成冰棱
        {
            GameObject tempIceThorn = Instantiate(Resources.Load("Prefabs/IceThorn") as GameObject);
            tempIceThorn.transform.localPosition = iceThornPos[i];
        }
    }


    private void IceCrystalAttack()
    {
        iceCrystalTimer += Time.smoothDeltaTime;
        if (iceCrystalTimer >= 3.0f)
        {
            anim.SetTrigger("Attacking");
            iceCrystalTimer = 0f;
            GameObject iceCrystal = Instantiate(Resources.Load("Prefabs/IceCrystal") as GameObject);
            iceCrystal.transform.localPosition = transform.localPosition;
        }
    }

    private float CalDistanceToPlayer()
    {
        return Mathf.Sqrt(Mathf.Abs(playerTransform.transform.localPosition.x - transform.localPosition.x)
        * Mathf.Abs(playerTransform.transform.localPosition.x - transform.localPosition.x)
        + Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y)
        * Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y));

    }

    private void MovementToPlayer()
    {
        float dotRes = Vector3.Dot(transform.right, (playerTransform.transform.localPosition - transform.localPosition).normalized);

        if ((dotRes >= -0.1f && dotRes <= 0.1f))
        {
            bossSpeed = 0f;//不移动
            anim.SetBool("Walking", false);
        }
        else
        {
            bossSpeed = 5f;
            anim.SetBool("Walking", true);
            if (dotRes < -0.1f)
            {
                transform.right = -transform.right;//转向
            }
        }
        /*移动Boss*/
        Vector3 p = transform.localPosition;
        p += transform.right * bossSpeed * Time.smoothDeltaTime;
        transform.localPosition = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))//受到近战攻击
        {
            anim.SetBool("Attacked", true);
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;

            BossHurt(playerAttackDamage);//伤害结算       
            if (endingNum <= 1)
            {
                dialogueRunner.Stop();
                switch (attackedCount)
                {
                    case 0:
                        ++attackedCount;
                        dialogueRunner.StartDialogue("BossWarning");
                        break;
                    case 1:
                        ++attackedCount;
                        dialogueRunner.StartDialogue("EndingTransform");
                        endingNum = 3;
                        break;

                }
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("RemoteAttack"))//受到远程攻击 不算火焰攻击
        {
            anim.SetBool("Attacked", true);
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            BossHurt(playerAttackDamage);//伤害结算       
            if (endingNum <= 1)
            {
                dialogueRunner.Stop();
                switch (attackedCount)
                {
                    case 0:
                        ++attackedCount;
                        dialogueRunner.StartDialogue("BossWarning");
                        break;
                    case 1:
                        ++attackedCount;
                        dialogueRunner.StartDialogue("EndingTransform");
                        endingNum = 3;
                        break;

                }
            }
        }
    }

}
