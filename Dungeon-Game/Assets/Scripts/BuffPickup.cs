using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickup : MonoBehaviour
{

    private CapsuleCollider myCollider;
    public int buffType;

    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        transform.Rotate(90,0,0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.TryGetComponent(out PlayerStatus pstats))
        {
            Physics.IgnoreCollision(myCollider, col);
            return;
        }

        IHealable applyHeal = col.gameObject.GetComponent<IHealable>();
        if (applyHeal != null)
        {
            switch (buffType)
            {
                case 0:
                    applyHeal.AddBuff(new DamageUpStatus(100000f));
                    break;
                case 1:
                    applyHeal.AddBuff(new DefenseUpStatus(100000f));
                    break;
                case 2:
                    applyHeal.AddBuff(new SpeedUpStatus(100000f));
                    break;
            }
            Destroy(gameObject);
        }
    }

}
