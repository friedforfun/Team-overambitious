using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : BasicAttack
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform launchPoint;

    private void Start()
    {
        // Create some debuffs that will be applied on attack
        AttackDebuffs poisonDebuff = () => { return new PoisonedStatus(5, 10); };
        AttackDebuffs slowDebuff = () => { return new SlowStatus(1, 10); };

        // Add them to the debuff delegate
        debuffs = poisonDebuff + slowDebuff;
    }

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        GameObject newProjectile = Instantiate(projectile, launchPoint.position, Quaternion.LookRotation(direction, Vector3.up));
        newProjectile.GetComponent<IProjectile>().Fire(direction, attackPower, gameObject, debuffs);
    }

}
