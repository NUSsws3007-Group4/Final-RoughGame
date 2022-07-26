using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroChargeAttack : MonoBehaviour
{
    private const float cooldownTime = 1f;
    private float cooldownTimer = 00f;
    private bool isDown = false;
    private float chargeTimer = 0f;
    private int deltaATK = 0;
    private Animator mAnimator;
    void Start()
    {
        mAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer = cooldownTimer - Time.deltaTime < 0 ? 0 : cooldownTimer - Time.deltaTime;
        CheckState();
        ChargeAttack();
        mAnimator.SetBool("IsChargeing", isDown);
        mAnimator.SetBool("IsChargToGo", chargeTimer >= 0.5f);
    }

    private void CheckState()
    {
        if(Input.GetKeyDown(KeyCode.I))//开始蓄力攻击
        {
            
            if (cooldownTimer <= 0)
            {
                isDown = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.I))//释放蓄力攻击
        {
            isDown = false;
        }
    }

    private void ChargeAttack()
    {
        if(isDown)//蓄力中
        {
            chargeTimer += Time.deltaTime;
        }
        else if(!isDown && chargeTimer >= 0.5f)//触发蓄力效果
        {
            gameObject.GetComponent<AudioManager>().PlayAudio("Hero/skillAttacking");
            cooldownTimer = cooldownTime * gameObject.GetComponent<HeroAttackHurt>().coldDownCoef + 0.6f;
            chargeTimer = chargeTimer > 3.0f ? 3.0f : chargeTimer;//限制时长为3s
            deltaATK = (int)((chargeTimer-0.5f)/0.1f);//计算加成攻击力
            mAnimator.SetTrigger("ChargeAttack");
            transform.GetChild(5).gameObject.GetComponent<ChargeAttackIndicator>().ExtraATK(deltaATK);
            transform.GetChild(5).gameObject.SetActive(true);
            chargeTimer = 0f;
        }
        else if(!isDown && chargeTimer < 0.5f)//取消蓄力效果
        {

            chargeTimer = 0f;
            transform.GetChild(5).gameObject.GetComponent<ChargeAttackIndicator>().ExtraATK(0);
        }
    }
}
