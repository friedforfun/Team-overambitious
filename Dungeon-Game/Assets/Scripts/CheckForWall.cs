using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForWall : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }
        }
    }

