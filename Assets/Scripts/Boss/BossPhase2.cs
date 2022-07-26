using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BossPhase2 : MonoBehaviour
{

    private const float pi = 3.14159f;
    private int bossHP = 500;//生命值
    private int bossShield = 0;//护盾值（不会恢复）
    private int iceThornNum = 0;//冰刺数目产生
    private float iceThornTimer = 0f;//冰刺攻击间隔计数器
    private float iceThornInterval = 0f;//冰刺攻击间隔设置
    //private bool coldnessProduce = false;//是否开始产生寒冷值
    private float distanceToPlayer = 0f;//Boss和Player之间的距离
    private float iceCrystalTimer = 0f;//冰棱攻击间隔计数器
    private float barrageTimer = 0f;//弹幕攻击计数器
    private List<Vector3> iceThornPos = new List<Vector3>();//开设冰刺位置数组
    private int playerAttackDamage = 0;
    private bool attacked = false;
    private float attackedTimer = 0;
    private int blinkCount = 0;

    private GameObject mHero;
    private EndingJudgement judge;
    public Transform playerTransform;
    private DialogueRunner dialogueRunner;


    void Start()
    {
        mHero = GameObject.Find("hero");
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        judge = mHero.GetComponent<EndingJudgement>();
    }

    void Update()
    {
        if (attacked)
        {
            attackedTimer += Time.deltaTime;
            if (attackedTimer >= 0.2f)
            {
                Color c = gameObject.GetComponent<SpriteRenderer>().color;
                if (c.a == 1)
                    c.a = 0.6f;
                else c.a = 1f;
                gameObject.GetComponent<SpriteRenderer>().color = c;
                attackedTimer = 0;
                blinkCount++;
            }
            if (blinkCount == 4)
            {
                attacked = false;
                blinkCount = 0;
            }
        }

        if (judge.status == 2f)
        {
            PhaseThree();
        }
    }

    private void PhaseThree()
    {
        playerTransform = GameObject.Find("hero").transform;
        distanceToPlayer = CalDistanceToPlayer();

        iceThornNum = Random.Range(4, 6);
        iceThornInterval = 1.5f;
        IceThornAttack(iceThornNum, iceThornInterval);

        if (distanceToPlayer <= 20f)
        {
            IceCrystalAttack();
        }

        barrageTimer += Time.smoothDeltaTime;
        if (barrageTimer >= 4.0f)
        {
            barrageTimer = 0f;
            BarrageAttack();
        }

    }

    private void BossHurt(int damage)
    {
        bossHP = bossHP > damage ? bossHP - damage : 0;
        if (bossHP <= 0)
            BossDeath();

    }

    private void BossDeath()
    {
        judge.BossDead = true;
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
            iceCrystalTimer = 0f;
            GameObject iceCrystal = Instantiate(Resources.Load("Prefabs/IceCrystal") as GameObject);
            iceCrystal.transform.localPosition = transform.localPosition;
        }
    }

    private void BarrageAttack()
    {
        Vector3 tvec, direction;
        for (float i = -30f; i <= 30f; i += 15f)
        {
            GameObject barrageAttack = Instantiate(Resources.Load("Prefabs/Barrage") as GameObject);
            barrageAttack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            barrageAttack.transform.localPosition = transform.localPosition;

            tvec = playerTransform.transform.localPosition - transform.localPosition;
            direction.x = tvec.x * Mathf.Cos(i / 180f * pi) + tvec.y * Mathf.Sin(i / 180f * pi);
            direction.y = -tvec.x * Mathf.Sin(i / 180f * pi) + tvec.y * Mathf.Cos(i / 180f * pi);
            direction.z = 0;
            barrageAttack.transform.up = direction.normalized;
        }
    }

    private float CalDistanceToPlayer()
    {
        return Mathf.Sqrt(Mathf.Abs(playerTransform.transform.localPosition.x - transform.localPosition.x)
        * Mathf.Abs(playerTransform.transform.localPosition.x - transform.localPosition.x)
        + Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y)
        * Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y));

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))//受到近战攻击
        {
            attacked = true;
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            if (mHero.gameObject.GetComponent<HeroBehavior>().withFlame)
                playerAttackDamage *= 3;
            BossHurt(playerAttackDamage);//伤害结算       
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("RemoteAttack"))//受到远程攻击 不算火焰攻击
        {
            attacked = true;
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            BossHurt(playerAttackDamage);//伤害结算       
        }
    }

}
