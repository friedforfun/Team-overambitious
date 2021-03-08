using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : MonoBehaviour, IDamagable, IHealable, IKillable
{
    public int HP;
    public int MaxHp;

    public int MoveSpeed; // Movement speed modifier
    private float moveSpeedPointValue = 0.1f;

    public int AttackPower; // Outgoing damage modifier
    private float attackPointValue = 0.1f;

    public int Defense; // Incoming damage modifier
    private float defPointValue = 0.1f;
    
    public List<Debuff> Debuffs;
    public List<Buff> Buffs;

    public void Damage(int damageTaken)
    {
        float modifiedDamage = damageTaken * (1 - (defPointValue * Defense));
        if (modifiedDamage < 0)
            modifiedDamage = 0f;
        
        HP -= (int) modifiedDamage;
        if (HP <= 0)
        {
            Kill();
        }
    }

    public void Heal(int healAmount)
    {
        HP += healAmount;
        if (HP < MaxHp)
            HP = MaxHp;
    }

    public void Kill()
    {
        Debug.Log("TODO: Kill this unit");
    }

    void Start()
    {
        StartCoroutine(checkStatusEffects());
        StartCoroutine(applyContinousEffects());
    }

    private IEnumerator checkStatusEffects()
    {
        for (; ; )
        {
            foreach (Debuff debuff in Debuffs)
            {
                if (debuff.Expired())
                {
                    debuff.ClearStatus(this);
                    Debuffs.Remove(debuff);
                }
            }
            foreach (Buff buff in Buffs)
            {
                if (buff.Expired())
                {
                    buff.ClearStatus(this);
                    Buffs.Remove(buff);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator applyContinousEffects()
    {
        for (; ; )
        {
            foreach (Buff buff in Buffs)
            {
                buff.ContinuousEffect(this);
            }
            foreach (Debuff debuff in Debuffs)
            {
                debuff.ContinuousEffect(this);               
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
