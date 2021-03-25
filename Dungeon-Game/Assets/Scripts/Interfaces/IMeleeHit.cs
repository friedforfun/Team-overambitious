using UnityEngine;
using System.Collections.Generic;
public interface IMeleeHit
{
    void MeleeAttack(float AttackPower, AttackDebuffs debuffs);
}
