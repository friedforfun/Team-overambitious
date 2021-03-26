using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSpawn : MonoBehaviour
{
    public int openingDirection;
    //1 = top
    //2 = bottom
    //3 = right
    //4 = left

    private RoomTemplates Templates;
    private int rand;
    private bool spawned = false;
    private GameObject Scene;

    void Start()
    {
        Templates = GameObject.Find("ScriptHolder").GetComponent<RoomTemplates>();

        if (Templates.GenerationComplete == false)
        {
            Invoke("Spawn", 0.1f);
        }
        Scene = Templates.LeftScene;
    }

    void Spawn()
    {
        if (spawned == false) {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, Templates.BottomRooms.Length);
                Instantiate(Templates.BottomRooms[rand], transform.position, Quaternion.identity, Scene.transform);      //Quaternion.Identity could be changed to allow for different rotations of rooms.
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, Templates.FrontRooms.Length);
                Instantiate(Templates.FrontRooms[rand], transform.position, Quaternion.identity, Scene.transform);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, Templates.LeftRooms.Length);
                Instantiate(Templates.LeftRooms[rand], transform.position, Quaternion.identity, Scene.transform);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, Templates.RightRooms.Length);
                Instantiate(Templates.RightRooms[rand], transform.position, Quaternion.identity, Scene.transform);
            }

            Templates.RoomCount += 1;
            if(Templates.RoomCount > Templates.MaximumRooms)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            spawned = true;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpawnTag") && other.GetComponent<RoomSpawn>().spawned == true)
        {
            Destroy(gameObject);

        }
    }
}
