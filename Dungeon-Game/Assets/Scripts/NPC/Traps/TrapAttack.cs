using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : BasicAttack
{
    private SpikeCollided spikeCollider;
    public GameObject spikePieces;
    [SerializeField] private int damage;

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        spikeCollider.MeleeAttack(attackPower, null);
        
    }

    public void MeleeAttack(float AttackPower, AttackDebuffs debuffs)
    {
        this.debuffs = debuffs;
        damage = (int)(damage * AttackPower);
    }


}
