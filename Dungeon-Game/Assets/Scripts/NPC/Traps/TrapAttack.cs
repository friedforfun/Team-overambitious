using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : BasicAttack
{
    [SerializeField] Animator animator;

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        animator.SetBool("Active", true);
        
    }



}
