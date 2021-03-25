using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickup : MonoBehaviour
{

    private CapsuleCollider myCollider;
    private Buff thisBuff;
    public int buffType;

    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        switch (buffType)
        {
            case 0:
                thisBuff = new DamageUpStatus(100000f);
                break;
            case 1:
                thisBuff = new DefenseUpStatus(100000f);
                break;
            case 2:
                thisBuff = new SpeedUpStatus(100000f);
                break;
        }
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
            applyHeal.AddBuff(thisBuff);
        Destroy(gameObject);
    }

}
