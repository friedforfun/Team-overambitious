using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    bool DoorIsOpen = false;


    private void OnTriggerEnter(Collider other)
    {
        if (DoorIsOpen)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            LeanTween.moveLocalY(gameObject, -5f, 0.5f).setEaseOutQuad();
            //Debug.Log("open plsss");
            DoorIsOpen = true;
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!DoorIsOpen)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            LeanTween.moveLocalY(gameObject, 0f, 0.5f).setEaseOutQuad();
            DoorIsOpen = false;
        }
        
    }


}
