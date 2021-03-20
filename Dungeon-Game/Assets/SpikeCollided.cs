using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCollided : MonoBehaviour, IDamagable
{

    public Trap_SpikeState spike;
    public int damage;
    GameObject target;

    public void AddDebuff<T>(T debuff) where T : Debuff
    {
        throw new System.NotImplementedException();
    }

    public void Damage(int damageTaken)
    {
        damage = damageTaken;
    }



    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            spike.PlayerSpiked(true, other.gameObject);
            target = other.gameObject;


        }
    }
}
