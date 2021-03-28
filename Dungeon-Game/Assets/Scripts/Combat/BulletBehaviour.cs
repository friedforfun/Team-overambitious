using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject owner;
    [SerializeField] private ParticleSystem ImpactEffect;
    [SerializeField] private float liveTime;

    [SerializeField] private int maxBounces = 5;
    [SerializeField] private int damage;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private float launchForce = 10f;

    private AttackDebuffs debuffs;
    private Vector3 currentVelocity = Vector3.zero;


    void Start()
    {
        StartCoroutine(destroySelf());
    }

    void Update()
    {
        currentVelocity = rbody.velocity;
    }

    public void Fire(Vector3 Direction, float AttackPower, GameObject owner, AttackDebuffs debuffs)
    {
        this.debuffs = debuffs;
        this.owner = owner;
        damage = (int) (damage * AttackPower);
        rbody.AddForce(Direction.normalized * launchForce, ForceMode.Impulse);
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
            if (debuffs != null)
            {
                //applyDamage.AddDebuff(debuffs());
                foreach (AttackDebuffs debuff in debuffs.GetInvocationList())
                {
                    Debuff d = debuff();
                    applyDamage.AddDebuff(d);
                    Debug.Log($"Added Debuff {d}");
                }
            }

        }
        ContactPoint hit = collision.GetContact(0);
        Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));

        if (maxBounces > 0)
        {
            maxBounces--;
            Vector3 newDirection = Vector3.Reflect(currentVelocity, hit.normal);
            rbody.AddForce(newDirection, ForceMode.Impulse);
        }

    }

    private IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }

}
