using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    private const float pi = 3.14159f;
    private int bossHP = 10;//生命值
    private int bossShield = 0;//护盾值（不会恢复）
    private float bossSpeed = 5f;//移动速度
    private int iceThornNum = 0;//冰刺数目产生
    private float iceThornTimer = 0f;//冰刺攻击间隔计数器
    private float iceThornInterval = 0f;//冰刺攻击间隔设置
    private bool coldnessProduce = false;//是否开始产生寒冷值
    private float distanceToPlayer = 0f;//Boss和Player之间的距离
    private float iceCrystalTimer = 0f;//冰棱攻击间隔计数器
    private float barrageTimer = 0f;//弹幕攻击计数器
    private bool isFlameAttack = false;//是否收到火焰攻击
    private float flameAttackTimer = 0f;//收到火焰攻击时间计数器
    private float recoverTimer = 0f;//回血计数器
    private bool stopReviveTag = false;//终止重生标记
    private float reviveTimer = 0f;//重生计数器
    private List<Vector3> iceThornPos = new List<Vector3>();//开设冰刺位置数组
    
    
    public int playerAttackDamage = GameObject.Find("hero").GetComponent<HeroBehavior>().ATK;/*调用player攻击力*/
    public Transform playerTransform;

    void Start()
    {
        
    }

    void Update()
    {
        flameAttackTimer += Time.smoothDeltaTime;

        if(flameAttackTimer >= 10f)//超越10s未受到火焰伤害
        {
            flameAttackTimer = 0f;
            isFlameAttack = false;
        }

        if(bossShield > 0)
        {
            PhaseOne();//有护盾阶段
        }
        else if(bossShield==0 && bossHP > 500 && bossHP <= 1000)
        {
            PhaseTwo();//无护盾 移动阶段
        }
        else if(bossShield==0 && bossHP > 0 && bossHP <= 500)
        {
            PhaseThree();//无护盾 移动阶段 不受火焰伤害时回血
        }
        else if(bossHP == 0)
        {
            BossRevive();
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
        coldnessProduce = true;//开始产生寒冷值
        playerTransform = GameObject.Find("hero").transform;//取出player.transform
        distanceToPlayer = CalDistanceToPlayer();//计算距离以判定冰棱攻击

        if(distanceToPlayer >= 10f)
        {
            MovementToPlayer();//调用移动
        }
        

        iceThornNum = Random.Range(4, 6);
        iceThornInterval = 2f;
        IceThornAttack(iceThornNum, iceThornInterval);

        if(distanceToPlayer >= 5.0f)
        {
            //Debug.Log("Attacking with Ice Crystal!");
            IceCrystalAttack();
        }
    }

    private void PhaseThree()//无护盾 移动阶段 不受火焰伤害时回血
    {
        playerTransform = GameObject.Find("hero").transform;
        distanceToPlayer = CalDistanceToPlayer();
        
        if(distanceToPlayer >= 10f)
        {
            MovementToPlayer();
        }
        

        iceThornNum = Random.Range(4, 6);
        iceThornInterval = 1.5f;
        IceThornAttack(iceThornNum, iceThornInterval);

        if(distanceToPlayer <= 20f)
        {
            IceCrystalAttack();
        }

        barrageTimer += Time.smoothDeltaTime;
        if(barrageTimer >= 4.0f)
        {
            barrageTimer = 0f;
            BarrageAttack();
        }

        recoverTimer += Time.smoothDeltaTime;
        //如果10s没有受到火焰伤害&&回复冷却完成&&不会超过50%血量
        if(!isFlameAttack && recoverTimer >= 1.0f && bossHP+20 <= 500)
        {
            recoverTimer = 0f;
            bossHP += 20;
        }

        Debug.Log(bossHP);
        
    }

    private void BossRevive()
    {
        reviveTimer += Time.smoothDeltaTime;
        if(!stopReviveTag)//重生被终止 结算死亡
        {
            reviveTimer = 0f;
            BossDeath();
        }
        if(reviveTimer >= 20f)//超过20s未被火焰属性打断
        {
            reviveTimer = 0f;
            bossHP = 500;
        }
    }

    private void BossHurt(int damage)
    {
        if(bossShield > 0)
        {
            bossShield = bossShield>damage ? bossShield-damage : 0;
        }
        else if(bossShield == 0)
        {
            bossHP = bossHP>damage ? bossHP-damage : 0;
        }
    }

    private void BossDeath()
    {
        Destroy(transform.gameObject);
    }

    private void IceThornAttack(int spawnNum, float spawnInterval)
    {
        iceThornTimer += Time.smoothDeltaTime;
        if(iceThornTimer >= spawnInterval)
        {
            iceThornTimer = 0f;//清空计时器
            
            
            iceThornPos.Clear();//清空
            playerTransform = GameObject.Find("hero").transform;

            WarningSignSpawn(spawnNum);
            Invoke("IceThornSpawn",1f);    
            
        }
    }

    private void WarningSignSpawn(int spawnNum)
    {
        for(int i = 0;i < spawnNum;i++)//生成警告标志
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
        for(int i = 0;i < iceThornPos.Count;i++)//生成冰棱
        {
            GameObject tempIceThorn = Instantiate(Resources.Load("Prefabs/IceThorn") as GameObject);
            tempIceThorn.transform.localPosition = iceThornPos[i];
        }
    }


    private void IceCrystalAttack()
    {
        iceCrystalTimer += Time.smoothDeltaTime;
        if(iceCrystalTimer >= 3.0f)
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
        *Mathf.Abs(playerTransform.transform.localPosition.x - transform.localPosition.x)
        +Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y)
        *Mathf.Abs(playerTransform.transform.localPosition.y - transform.localPosition.y));

    }

    private void MovementToPlayer()
    {
        float dotRes = Vector3.Dot(transform.right, (playerTransform.transform.localPosition - transform.localPosition).normalized);

        if(dotRes >= -0.1f && dotRes <= 0.1f)
        {
            bossSpeed = 0f;//不移动
        }
        else
        {
            bossSpeed = 5f;
            if(dotRes < -0.1f)
            {
                transform.right = -transform.right;//转向
            }
        }
        /*移动Boss*/
        Vector3 p = transform.localPosition;
        p += transform.right * bossSpeed * Time.smoothDeltaTime;
        transform.localPosition = p;
    }

    public bool IsColdnessProduced()//寒冷值接口
    {
        return coldnessProduce;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))//受到攻击
        {
            if(GameObject.Find("Hero").GetComponent<HeroBehavior>().isFlameEnchanted)
            {
                stopReviveTag = true;
                isFlameAttack = true;
                flameAttackTimer = 0f;
            }
            
            BossHurt(playerAttackDamage);//伤害结算       
        }
    }

}
