using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BossPhase2 : MonoBehaviour
{

    private const float pi = 3.14159f;
    private int bossHP = 500;//生命值
    private int bossShield = 0;//护盾值（不会恢复）
    private float bossSpeed = 5f;//移动速度
    private int iceThornNum = 0;//冰刺数目产生
    private float iceThornTimer = 0f;//冰刺攻击间隔计数器
    private float iceThornInterval = 0f;//冰刺攻击间隔设置
    //private bool coldnessProduce = false;//是否开始产生寒冷值
    private float distanceToPlayer = 0f;//Boss和Player之间的距离
    private float iceCrystalTimer = 0f;//冰棱攻击间隔计数器
    private float barrageTimer = 0f;//弹幕攻击计数器
    private bool stopReviveTag = false;//终止重生标记
    private float reviveTimer = 0f;//重生计数器
    private List<Vector3> iceThornPos = new List<Vector3>();//开设冰刺位置数组
    private int playerAttackDamage = 0;

    private GameObject mHero;
    public Transform playerTransform;
    public int endingNum;
    public float status;
    private DialogueRunner dialogueRunner;


    void Start()
    {
        mHero = GameObject.Find("hero");
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }

    void Update()
    {
        dialogueRunner.GetComponent<VariableStorageBehaviour>().TryGetValue<float>("$ready", out status);

        if (status == 3f)
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
        if (bossShield > 0)
        {
            bossShield = bossShield > damage ? bossShield - damage : 0;
        }
        else if (bossShield == 0)
        {
            bossHP = bossHP > damage ? bossHP - damage : 0;
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
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            if (mHero.gameObject.GetComponent<HeroBehavior>().withFlame)
                playerAttackDamage *= 3;
            BossHurt(playerAttackDamage);//伤害结算       
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("RemoteAttack"))//受到远程攻击 不算火焰攻击
        {
            playerAttackDamage = mHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                  mHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            BossHurt(playerAttackDamage);//伤害结算       
        }
    }

}
