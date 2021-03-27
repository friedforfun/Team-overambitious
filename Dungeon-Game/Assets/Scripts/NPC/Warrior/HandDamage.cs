using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDamage : MonoBehaviour
{
    [SerializeField] WarriorStatus stats;

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
        if(damageable != null)
        {
            damageable.Damage((int) (stats.BaseDamage * stats.DamageModifier()));
        }
    }
}
