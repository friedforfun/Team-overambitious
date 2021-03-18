using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject owner;
    [SerializeField] private ParticleSystem ImpactEffect;
    [SerializeField] private float liveTime;


    [SerializeField] private int damage;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private float launchForce = 10f;

    public void Fire(Vector3 Direction, float AttackPower, GameObject owner)
    {
        this.owner = owner;
        damage = (int) (damage * AttackPower);
        rbody.AddForce(Direction * launchForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        IDamagable applyDamage = collision.gameObject.GetComponent<IDamagable>();
        if (applyDamage != null)
        {
            applyDamage.Damage(damage);
        }
        ContactPoint hit = collision.GetContact(0);
        Instantiate<ParticleSystem>(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        StartCoroutine(destroySelf());
    }

    private IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }

}
