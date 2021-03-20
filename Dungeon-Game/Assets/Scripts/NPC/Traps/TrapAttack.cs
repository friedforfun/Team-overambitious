using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : BasicAttack
{
    [SerializeField] public SpikeCollided spikeCollider;

    

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        spikeCollider.MeleeAttack(attackPower, null);
        
    }


}
