using UnityEngine;
using System.Collections.Generic;

public interface IProjectile
{
    void Fire(Vector3 Direction, float AttackPower, GameObject owner, AttackDebuffs debuffs);
}
