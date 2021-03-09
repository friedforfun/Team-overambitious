using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public abstract class Debuff : StatusEffect
{
    public Debuff(float duration, bool stackable) : base(duration, stackable)
    {

    }
}

public abstract class Buff : StatusEffect
{
    public Buff(float duration, bool stackable) : base(duration, stackable)
    {

    }
}

public class SlowStatus: Debuff
{
    private int slowAmount;

    public SlowStatus(int slowAmount, float duration) : base(duration, true)
    {
        this.slowAmount = slowAmount;
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


public class PoisonedStatus : Debuff
{
    private int damageTick;

    public PoisonedStatus(int damageTick, float duration) : base(duration, false)
    {
        this.damageTick = damageTick;
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