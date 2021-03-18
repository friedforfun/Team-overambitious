using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : BasicAttack
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform launchPoint;

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        GameObject newProjectile = Instantiate(projectile, launchPoint.position, Quaternion.LookRotation(direction, Vector3.up));
        newProjectile.GetComponent<IProjectile>().Fire(direction, attackPower, gameObject);
    }

}
