using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{

    public GameObject player;

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
