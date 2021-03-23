using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRoom : MonoBehaviour
{
    private RoomTemplates Templates;
    public GameObject SceneLeft;
    public bool SceneFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        Templates = GameObject.Find("ScriptHolder").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Templates.GenerationComplete == true && SceneFlipped == false && Templates.BossSpawned == true )
        {
            Templates.MaximumRooms = Templates.MaximumRooms * 2;
            GameObject SceneRight = Instantiate(SceneLeft, new Vector3(500, 0, 0), Quaternion.identity);
            SceneRight.name = "SceneRight";
            SceneRight.transform.parent = GameObject.Find("Scenes").transform;
            SceneFlipped = true;
        }
    }
}
