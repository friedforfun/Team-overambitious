using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : BasicAttack
{
    [SerializeField] Animator animator;

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        animator.SetTrigger("Attack");

    }



}
