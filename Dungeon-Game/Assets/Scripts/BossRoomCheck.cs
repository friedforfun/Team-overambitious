using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossRoomCheck : MonoBehaviour
{
    public bool isBossRoom = false;
    public GameObject BossRoom;
    private RoomTemplates Templates;
    private float xpos;
    private float zpos;
    private float distancefromspawn;
    // Start is called before the first frame update
    void Start()
    {
        Templates = GameObject.Find("ScriptHolder").GetComponent<RoomTemplates>();
        xpos = Math.Abs(transform.position.x);
        zpos = Math.Abs(transform.position.z);
        distancefromspawn = xpos + zpos;
        
    }

    // Update is called once per frame
    void Update()
    {
        if((Templates.BossRoomX == transform.position.x) && (Templates.BossRoomZ == transform.position.z))
        {
            Instantiate(BossRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        else if (Templates.GenerationComplete == true)
        {
            if(distancefromspawn > Templates.FurthestPoint)
            {
                Templates.FurthestPoint = distancefromspawn;
                Templates.BossRoomX = transform.position.x;
                Templates.BossRoomZ = transform.position.z;
            }
        }
    }
}
