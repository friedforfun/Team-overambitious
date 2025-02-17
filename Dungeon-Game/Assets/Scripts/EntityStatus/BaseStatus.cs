﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void DamageTakenEvent();
public delegate void OnDeathEvent();

public class BaseStatus : MonoBehaviour, IDamagable, IHealable, IKillable
{
    // Upper/lower bound on stats, value needs tuning/removing
    private int _statLimiter = 8;
    protected GameObject damageText, statusIcon;
    public int HP; // current hp
    public int MaxHp = 100; // max hp

    public OnDeathEvent OnDeath;

    public int MoveSpeed // Movement speed stat points
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
    [SerializeField] private int _MoveSpeed = 0; // MoveSpeed backing field
    private float moveSpeedPointValue = 0.2f; // The value of each point of MoveSpeed


    public int AttackPower // Outgoing damage stat
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
    [SerializeField] private int _AttackPower = 0;
    private float attackPointValue = 0.1f; // The value of each point of AttackPower

    [SerializeField] private float AttackSpeed = 1f; // Number of basic attacks per second (Not a stat but could be)

    public int Defense // Incoming damage mitigation stat
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
    [SerializeField] private int _Defense = 0;
    private float defPointValue = 0.1f; // The value of each point of Defense

    public List<Debuff> Debuffs = new List<Debuff>();
    public List<Buff> Buffs = new List<Buff>();

    public bool isDead { get; private set;  }

    void Start()
    {
        isDead = false;
        StartCoroutine(checkStatusEffects());
        StartCoroutine(applyContinousEffects());
        HP = MaxHp;
        SetUp();

    }

    protected virtual void SetUp()
    {
        damageText = (GameObject)Resources.Load("Prefabs/DamageText", typeof(GameObject));
        statusIcon = (GameObject)Resources.Load("Prefabs/StatusIcon", typeof(GameObject));
    }

    protected virtual void DamageUpdate()
    {

    }

    void Update()
    {
        UIUpdate();
    }

    protected virtual void UIUpdate()
    {
        float zModifier = transform.position.z + 1f;
        foreach (Debuff d in Debuffs)
        {
            if(d.iconObject != null) d.iconObject.transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y + 1f, zModifier);
            zModifier -= 0.65f;
        }
    }

    /// <summary>
    /// Add debuff to entity
    /// </summary>
    /// <typeparam name="T">Type implementing Debuff</typeparam>
    /// <param name="debuff">The debuff being added</param>
    public void AddDebuff<T>(T debuff) where T : Debuff
    {
        if (isDead)
            return;

        if (!debuff.Stackable())
        {
            // Debuff is not stackable, remove the current application of this debuff
            if (Debuffs != null)
            {
                foreach (T d in Debuffs.OfType<T>())
                {
                    Destroy(d.iconObject);
                    d.ClearStatus(this);
                    Debuffs.Remove(d);
                }
            }
            
        }
        // Apply debuff and add it to debuffs on character
        debuff.ApplyStatus(this);
        Debuffs.Add(debuff);
        debuff.iconObject = Instantiate(statusIcon, new Vector3(transform.position.x - 1.5f, transform.position.y + 1f, transform.position.z + 1f), Quaternion.identity);
        debuff.iconObject.GetComponent<SpriteRenderer>().sprite = debuff.icon;
        debuff.iconObject.transform.Rotate(90, 0, 0);
    }

    public void ClearDebuffs()
    {
        if (isDead)
            return;

        Debuffs = new List<Debuff>();
    }

    public void ClearBuffs()
    {
        if (isDead)
            return;

        Buffs = new List<Buff>();
    }

    /// <summary>
    /// Add buff to entity
    /// </summary>
    /// <typeparam name="T">Type implementing Buff</typeparam>
    /// <param name="buff">The buff being added</param>
    public void AddBuff<T>(T buff) where T : Buff
    {
        if (!buff.Stackable())
        {
            // Debuff is not stackable, remove the current application of this buff
            foreach (T d in Buffs.OfType<T>())
            {
                d.ClearStatus(this);
                Buffs.Remove(d);
            }
        }
        // Apply buff and add it to buffs on character
        buff.ApplyStatus(this);
        Buffs.Add(buff);
    }

    /// <summary>
    /// Apply damage to this entity, will be modified by defence stat, and triggers entity death when hp reaches 0
    /// </summary>
    /// <param name="damageTaken"></param>
    public void Damage(int damageTaken)
    {
        if (isDead)
            return;

        float modifiedDamage = damageTaken * (1 - (defPointValue * Defense));
        if (modifiedDamage < 0)
            modifiedDamage = 0f;
        
        HP -= (int) modifiedDamage;
        GameObject newDamageText = Instantiate(damageText, transform.position, Quaternion.identity);
        newDamageText.GetComponent<TextMesh>().text = "-" + ((int)modifiedDamage).ToString();
        if (HP <= 0)
        {
            Kill();
        }
        DamageUpdate();      
    }

    /// <summary>
    /// Apply heal to entity
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(int healAmount)
    {
        HP += healAmount;
        GameObject newHealText = Instantiate(damageText, transform.position, Quaternion.identity);
        newHealText.GetComponent<TextMesh>().text = "+" + healAmount.ToString();
        if (HP > MaxHp)
            HP = MaxHp;
    }

    /// <summary>
    /// Kill entity
    /// </summary>
    public void Kill()
    {
        isDead = true;
        if (OnDeath != null)
            OnDeath();
        //Debug.Log("TODO: Kill this unit");
    }

    public void Resurrect()
    {
        ClearBuffs();
        ClearDebuffs();
        Heal(10000);
        isDead = false;
    }

    /// <summary>
    /// Value to modify movespeed
    /// </summary>
    /// <returns></returns>
    public float MoveSpeedModifier() 
    {
        return 1.0f + (MoveSpeed * moveSpeedPointValue);
    }

    /// <summary>
    /// Value to modifier outgoing damage
    /// </summary>
    /// <returns></returns>
    public float DamageModifier()
    {
        return 1.0f + (AttackPower * attackPointValue);
    }

    /// <summary>
    /// The time in seconds between each attack
    /// </summary>
    /// <returns></returns>
    public float AttackInterval()
    {
        return 1f / AttackSpeed;
    }

    /// <summary>
    /// Checks if a debuff/buff has expired and clears it
    /// </summary>
    /// <returns></returns>
    private IEnumerator checkStatusEffects()
    {
        for (; ; )
        {
            if (isDead)
            {
                for (int i = 0; i < Debuffs.Count; i++)
                    if(Debuffs.ElementAt(i).iconObject != null) Destroy(Debuffs.ElementAt(i).iconObject);
                break;
            }


            if (Debuffs != null)
            {
                List<Debuff> debuffsToRemove = new List<Debuff>();
                for (int i = 0; i < Debuffs.Count; i++)
                {
                    if (Debuffs.ElementAt(i).Expired())
                    {
                        Destroy(Debuffs.ElementAt(i).iconObject);
                        Debuffs.ElementAt(i).ClearStatus(this);
                        debuffsToRemove.Add(Debuffs.ElementAt(i));
                    }
                }
                foreach (Debuff debuff in debuffsToRemove)
                {
                    Debuffs.Remove(debuff);
                }
            }

            if (Buffs != null)
            { 
                List<Buff> buffsToRemove = new List<Buff>();
                for (int i = 0; i < Buffs.Count; i++)
                {
                    if (Buffs.ElementAt(i).Expired())
                    {
                        Buffs.ElementAt(i).ClearStatus(this);
                        buffsToRemove.Add(Buffs.ElementAt(i));
                    }
                }
                foreach (Buff buff in buffsToRemove)
                {
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
            if (isDead)
                break;



            if (Buffs != null)
            {
                foreach (Buff buff in Buffs.ToList())
                {
                    buff.ContinuousEffect(this);
                }


                foreach (Debuff debuff in Debuffs.ToList())
                {
                    debuff.ContinuousEffect(this);
                }
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
        if (stat < -_statLimiter)
        {
            return -_statLimiter;
        }
        else if (stat > _statLimiter)
        {
            return _statLimiter;
        }
        return stat;
    }
}
