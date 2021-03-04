using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPSys : MonoBehaviour
{
    [SerializeField] private ParticleSystem glow;
    // Start is called before the first frame update
    void Start()
    {
        glow.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
