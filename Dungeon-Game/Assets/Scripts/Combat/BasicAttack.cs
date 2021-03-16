using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles frequency of attacks and damage
/// </summary>
public abstract class BasicAttack : MonoBehaviour
{

    [SerializeField] protected BaseStatus stats; // for damage/fire-rate ect

    // child classes should override this method to define attack behaviour (melee, shoot projectile ect)
    public abstract void PerformAttack(Vector3 direction, float attackPower);


    private bool canAttack = true;

    public void Attack(Vector3 direction)
    {
        if (canAttack)
        {
            canAttack = false;

            PerformAttack(direction, stats.DamageModifier());

            StartCoroutine(enableAttack());
        }
    }

    private IEnumerator enableAttack()
    {
        yield return new WaitForSeconds(stats.AttackInterval());
        canAttack = true;
    }
}
