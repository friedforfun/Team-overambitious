using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForDoor : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
        }
    }

