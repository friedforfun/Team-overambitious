using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : BasicAttack
{
    [SerializeField] private GameObject Hitpoint;
    [SerializeField] private int damage;
    WarriorState WS;

    public override void PerformAttack(Vector3 direction, float attackPower)
    {

        MeleeAttack(attackPower, null);

    }

    public void MeleeAttack(float AttackPower, AttackDebuffs debuffs)
    {
        this.debuffs = debuffs;
        damage = (int)(damage * AttackPower);
    }

}
