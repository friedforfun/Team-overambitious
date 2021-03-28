using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollided : MonoBehaviour
{

    [SerializeField] SpikeTrapStatus stats;


    private void OnTriggerEnter(Collider collision)
    {
        IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.Damage((int)(stats.BaseDamage * stats.DamageModifier()));
        }
    }

/*    private void OnTriggerStay(Collider collision)
    {
        IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.Damage((int)(stats.BaseDamage * stats.DamageModifier()));
        }
    }*/


}
