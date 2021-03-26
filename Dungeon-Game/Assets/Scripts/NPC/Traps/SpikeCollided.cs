using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollided : MonoBehaviour, IMeleeHit
{
   
    public Trap_SpikeState spike;
    [SerializeField] private int damage;
    
    private AttackDebuffs debuffs;

    // Start is called before the first frame update

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {

        Debug.Log("HIT THE PLAYER");
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

    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("HIT THE PLAYER");
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
    }
    


    public void MeleeAttack(float AttackPower, AttackDebuffs debuffs)
    {
        this.debuffs = debuffs;
        damage = (int)(damage * AttackPower);
    }

    /*        if (collision.tag == "Player")
        {
            spike.PlayerSpiked(true, collision.gameObject);
            target = collision.gameObject; 
        }*/
}
