using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForWall : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log(other.gameObject.transform.parent.name);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        }
    }

