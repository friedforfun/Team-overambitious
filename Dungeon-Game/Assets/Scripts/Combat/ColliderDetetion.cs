using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetetion : MonoBehaviour
{
    public BasicAttackShooter BAS;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // transition to enterCombat
            BAS.SetDetected(true, other.gameObject);
        }
    }
}
