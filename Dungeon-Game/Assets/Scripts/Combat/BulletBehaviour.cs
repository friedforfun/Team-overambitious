using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPoint;
    /*    BaseStatus stats = new BaseStatus();*/
    public BaseStatus stats;
    
    

    public float shootSpeed;
    public void Fire(Vector3 direction) 
    {
        GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);

        Rigidbody rig = currentBullet.GetComponent<Rigidbody>();
        rig.AddForce(direction * shootSpeed, ForceMode.VelocityChange);
        Destroy(currentBullet, 5f);
    }

    void Update()
    {
        
    }
}
