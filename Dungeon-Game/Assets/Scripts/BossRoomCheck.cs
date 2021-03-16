using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossRoomCheck : MonoBehaviour
{
    public bool isBossRoom = false;
    public GameObject BossRoom;
    private RoomTemplates Templates;
    private MirrorRoom MirrorScript;
    private float xpos;
    private float zpos;
    private float distancefromspawn;
    private GameObject Scene;
    // Start is called before the first frame update
    void Start()
    {
        Templates = GameObject.Find("ScriptHolder").GetComponent<RoomTemplates>();
        xpos = Math.Abs(transform.position.x);
        zpos = Math.Abs(transform.position.z);
        distancefromspawn = xpos + zpos;


        MirrorScript = GameObject.Find("ScriptHolder").GetComponent<MirrorRoom>();
        Scene = Templates.LeftScene;
        
        //adds object to Scene

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("SceneFlipped == " + MirrorScript.SceneFlipped);
        if (MirrorScript.SceneFlipped == false)
        {

            if ((Templates.BossRoomX == transform.position.x) && (Templates.BossRoomZ == transform.position.z))
            {
                Instantiate(BossRoom, transform.position, Quaternion.identity, Scene.transform);
                Debug.Log("run");
                Destroy(gameObject);
                Templates.BossSpawned = true;
            }

            else if (Templates.GenerationComplete == true)
            {
                if (distancefromspawn > Templates.FurthestPoint)
                {
                    Templates.FurthestPoint = distancefromspawn;
                    Templates.BossRoomX = transform.position.x;
                    Templates.BossRoomZ = transform.position.z;
                }
            }

        }
    }
}
