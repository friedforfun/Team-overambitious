﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles frequency of attacks and damage
/// </summary>
public class BasicAttackShooter : BasicAttack
{
    bool detected;
    GameObject target;
    public Transform enemy;

    public GameObject bullet;
    public Transform shootPoint;
    public ParticleSystem MuzzleFlash;

    public float shootSpeed;

    // child classes should override this method to define attack behaviour (melee, shoot projectile ect)
    public override void PerformAttack(Vector3 direction, int attackPower)
    {
        MuzzleFlash.Play();
        GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        // Get damage modifier from stats.DamageModifier() and increase bullet damage
        // BulletBehaviour bh = current.GetComponent<BulletBehviour>();
        // bh.Fire()
        Rigidbody rig = currentBullet.GetComponent<Rigidbody>();
        rig.AddForce(direction * shootSpeed, ForceMode.VelocityChange);
        Destroy(currentBullet, 5f);
    } 

    private void Update()
    {
        if (detected)
        {
            enemy.LookAt(target.transform);
            Attack(transform.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // transition to enterCombat
            detected = true;
            target = other.gameObject;
        }
    }
}
