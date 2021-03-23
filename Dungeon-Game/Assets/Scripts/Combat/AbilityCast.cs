using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityCast : MonoBehaviour
{
    [SerializeField] protected BaseStatus stats; // for damage/fire-rate ect

    protected AttackDebuffs debuffs;
    private bool canCast = true;

    // child classes should override this method to define ability behaviour (melee, shoot projectile, teleport ect)
    public abstract void PerformAbility(Vector3 direction, float attackPower);

    public void Cast(Vector3 direction)
    {
        if (canCast)
        {
            canCast = false;

            PerformAbility(direction, stats.DamageModifier());

            StartCoroutine(enableAttack());
        }
    }

    private IEnumerator enableAttack()
    {
        yield return new WaitForSeconds(stats.AttackInterval());
        canCast = true;
    }
}
