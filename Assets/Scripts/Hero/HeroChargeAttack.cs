using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private bool isDown = false;
    private float chargeTimer = 0f;
    private int deltaATK = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        ChargeAttack();
    }

    private void CheckState()
    {
        if(Input.GetKeyDown(KeyCode.I))//开始蓄力攻击
        {
            isDown = true;
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
        
        if(!isDown && chargeTimer >= 0.5f)//触发蓄力效果
        {
            chargeTimer = chargeTimer > 3.0f ? 3.0f : chargeTimer;//限制时长为3s
            deltaATK = (int)((chargeTimer-0.5f)/0.1f);//计算加成攻击力
            transform.GetChild(5).gameObject.GetComponent<ChargeAttackIndicator>().ExtraATK(deltaATK);
            transform.GetChild(5).gameObject.SetActive(true);
            chargeTimer = 0f;
        }
        
        if(!isDown && chargeTimer <= 0.5f)//取消蓄力效果
        {
            transform.GetChild(5).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.GetComponent<ChargeAttackIndicator>().ExtraATK(0);
        }
    }
}
