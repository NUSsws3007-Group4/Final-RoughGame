using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackHurt : MonoBehaviour
{
    public int hurt = 0;//普通攻击伤害
    public int powerUpCoef = 1;

    public void IncreaseAttack(int powerUpSet)
    {
        powerUpCoef += powerUpSet;
    }
}
