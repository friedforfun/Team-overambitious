using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRoom : MonoBehaviour
{
    [SerializeField] GameObject PlayerR;
    private RoomTemplates Templates;
    public GameObject SceneLeft;
    public bool SceneFlipped = false;

    private Vector3 spawnPoint = new Vector3(1000, 0, 0);

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
            GameObject SceneRight = Instantiate(SceneLeft, spawnPoint, Quaternion.identity);
            GameObject playerR = Instantiate(PlayerR, new Vector3(spawnPoint.x, spawnPoint.y + 0.5f, spawnPoint.z), Quaternion.identity);
            SceneFlipped = true;

            DungeonManager dm = SceneRight.GetComponent<DungeonManager>();
            dm.AssignPlayer(playerR);
            dm.AssignTeam(Team.RIGHT);

            // Emit procedural gen finished event
            EventManager.TriggerEvent("GenerationComplete");
        }
    }
}
