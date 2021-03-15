using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetetion : MonoBehaviour
{
    public BasicAttackShooter b;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // transition to enterCombat
            b.SetDetected(true, other.gameObject);
        }
    }
}
