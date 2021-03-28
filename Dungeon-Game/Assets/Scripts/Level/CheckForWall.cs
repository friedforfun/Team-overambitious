using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForWall : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.CompareTag("Wall"))
        {
            //Debug.Log(other.transform.parent.gameObject.name);
            Destroy(other.transform.parent.gameObject);
        }
        else if (other.CompareTag("Door"))
        {
            //Destroy(other);
            
            //Debug.Log((other.transform.parent.transform.parent.name));
           
            
           if (int.Parse(other.transform.parent.name) > int.Parse(gameObject.transform.parent.name))
            {
                //Debug.Log(other.name);
                Destroy(other.gameObject);
            }
        }
        else
        {
            //Debug.Log(other.tag);
        }
        }
    }

