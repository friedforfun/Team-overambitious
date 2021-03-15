using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public GameObject skyCamera;

    // Update is called once per frame
    void Update()
    {
        transform.forward = skyCamera.transform.forward;
    }
}
