using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public GameObject skyCamera;

    void Update()
    {
        transform.forward = skyCamera.transform.forward;
    }
}
