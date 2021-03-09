using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseStatus : MonoBehaviour, IDamagable, IHealable, IKillable
{
    // Upper bound on stats, value needs tuning
    private int _statLimiter = 5;

    public int HP;
    public int MaxHp;

    public int MoveSpeed // Movement speed modifier
    {
        get
        {
            return statLimiter(_MoveSpeed);
        }
        set
        {
            _MoveSpeed = value;
        }
    }
    private int _MoveSpeed;
    private float moveSpeedPointValue = 0.1f;


    public int AttackPower // Outgoing damage modifier
    {
        get
        {
            return statLimiter(_AttackPower);
        }
        set
        {
            _AttackPower = value;
        }
    }
    private int _AttackPower;
    private float attackPointValue = 0.1f;

    public int Defense // Incoming damage modifier
    {
        get
        {
            return statLimiter(_Defense);
        }
        set
        {
            _Defense = value;
        }
    }
    private int _Defense;
    private float defPointValue = 0.1f;
    
    public List<Debuff> Debuffs;
    public List<Buff> Buffs;

    /// <summary>
    /// Add debuff to entity
    /// </summary>
    /// <typeparam name="T">Type implementing Debuff</typeparam>
    /// <param name="debuff">The debuff being added</param>
    public void AddDebuff<T>(T debuff) where T : Debuff
    {
        if (!debuff.Stackable())
        {
            // Debuff is not stackable, remove the current application of this debuff
            foreach (T d in Debuffs.OfType<T>())
            {
                d.ClearStatus(this);
                Debuffs.Remove(d);
            }
        }
        // Apply debuff and add it to debuffs on character
        debuff.ApplyStatus(this);
        Debuffs.Add(debuff);
    }

    /// <summary>
    /// Apply damage to entity, will be modified by defence stat, and triggers entity death
    /// </summary>
    /// <param name="damageTaken"></param>
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

    /// <summary>
    /// Apply heal to entity
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(int healAmount)
    {
        HP += healAmount;
        if (HP < MaxHp)
            HP = MaxHp;
    }

    /// <summary>
    /// Kill entity
    /// </summary>
    public void Kill()
    {
        Debug.Log("TODO: Kill this unit");
    }

    void Start()
    {
        StartCoroutine(checkStatusEffects());
        StartCoroutine(applyContinousEffects());
    }

    /// <summary>
    /// Checks if a debuff/buff has expired and clears it
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Applies continuous effects from buffs then debuffs, for example damage from a poison effect might be here
    /// </summary>
    /// <returns></returns>
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

            yield return new WaitForSeconds(1f); //Tick rate
        }
    }

    /// <summary>
    /// Keep stats within upper and lower bound
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    private int statLimiter(int stat)
    {
        if (stat < 0)
        {
            return 0;
        }
        else if (stat > _statLimiter)
        {
            return _statLimiter;
        }
        return stat;
    }
}
