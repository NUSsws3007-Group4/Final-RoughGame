using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    private int extraATKFromCharge = 0;

    protected const float OriginCD = 0.6f;//初始设定CD
    protected float attackTimer = 0f;
    protected float normalAttackColdDown;
    void Start()
    {
        attackTimer = 0f;       
        normalAttackColdDown = OriginCD * gameObject.transform.parent.GetComponent<HeroAttackHurt>().coldDownCoef;
    }

    void Update()
    {
        gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt = 25 + extraATKFromCharge;//融合蓄力攻击基数

        attackTimer += Time.deltaTime;
        if(attackTimer >= normalAttackColdDown)
        {
            gameObject.SetActive(false);
            attackTimer = 0f;
        }
        
    }

    public void ExtraATK(int _damage)//获取蓄力的额外攻击力
    {
        extraATKFromCharge = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Attacking enemy");
            gameObject.SetActive(false);
        }
    }
}
