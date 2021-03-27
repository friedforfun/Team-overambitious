using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            animator.SetBool("DoorOpen", true);
            Debug.Log("open plsss");
        }
        else
        {
            Debug.Log(other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("DoorOpen", false);
        }
        
    }
}
