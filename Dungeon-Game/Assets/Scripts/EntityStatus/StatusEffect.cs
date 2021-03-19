using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract status effect class
/// </summary>
public abstract class StatusEffect
{
    protected bool stackable;
    private float startTime;
    private float duration;

    public StatusEffect(float duration, bool stackable)
    {
        this.stackable = stackable;
        this.duration = duration;
        startTime = Time.time;
    }

    public bool Expired()
    {
        if (Time.time - startTime > duration)
        {
            return true;
        }
        return false;
    }

    public bool Stackable()
    {
        return stackable;
    }

    protected virtual void overwrite(List<StatusEffect> statuses) { }

    public virtual void ContinuousEffect(BaseStatus status) { }

    public abstract void ApplyStatus(BaseStatus status);

    public abstract void ClearStatus(BaseStatus status);
}

/// <summary>
/// Parent debuff class, negative status effects
/// </summary>
public abstract class Debuff : StatusEffect
{
    public Sprite icon;
    public GameObject iconObject;

    public Debuff(float duration, bool stackable) : base(duration, stackable)
    {

    }
}

/// <summary>
/// Parent of all buff classes, positive status effects
/// </summary>
public abstract class Buff : StatusEffect
{
    public Buff(float duration, bool stackable) : base(duration, stackable)
    {

    }
}


/// <summary>
/// Reduce player movespeed for duration, stackable
/// </summary>
public class SlowStatus: Debuff
{
    private int slowAmount;

    public SlowStatus(int slowAmount, float duration) : base(duration, true)
    {
        this.slowAmount = slowAmount;
        icon = (Sprite)Resources.Load("Images/SlowIcon", typeof(Sprite));
    }

    public override void ApplyStatus(BaseStatus status)
    {
        status.MoveSpeed -= slowAmount;
    }

    public override void ClearStatus(BaseStatus status)
    {
        status.MoveSpeed += slowAmount;
    }
}

/// <summary>
/// Poisoned status, does ticking damage for duration
/// </summary>
public class PoisonedStatus : Debuff
{
    private int damageTick;

    public PoisonedStatus(int damageTick, float duration) : base(duration, false)
    {
        this.damageTick = damageTick;
        icon = (Sprite)Resources.Load("Images/PoisonIcon", typeof(Sprite));
    }

    public override void ContinuousEffect(BaseStatus status)
    {
        base.ContinuousEffect(status);
        status.Damage(damageTick);
    }

    public override void ApplyStatus(BaseStatus status)
    {

    }

    public override void ClearStatus(BaseStatus status)
    {

    }
}

/// <summary>
/// Reduces outgoing damage debuff for duration
/// </summary>
public class WeakenStatus : Debuff
{
    private int attackReduction;
    public WeakenStatus(int attackReduction, float duration) : base(duration, false)
    {
        this.attackReduction = attackReduction;
    }

    public override void ApplyStatus(BaseStatus status)
    {
        
    }

    public override void ClearStatus(BaseStatus status)
    {
        throw new System.NotImplementedException();
    }
}
