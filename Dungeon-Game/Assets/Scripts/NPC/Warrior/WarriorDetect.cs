using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorDetect : MonoBehaviour
{
    WarriorState WS;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            WS.Detected(true);
        }
        
    }

}
