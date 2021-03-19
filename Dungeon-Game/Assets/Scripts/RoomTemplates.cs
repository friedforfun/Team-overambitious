using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] FrontRooms;
    public GameObject[] BottomRooms;
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;

    public int RoomCount = 0;
    public int MinimumRooms;
    public int MaximumRooms;
    private int RoomCountTemp = 0;
    public int MaxFramesNoRooms;
    private int FramesNoRooms = 0;
    public bool GenerationComplete = false;
    public float FurthestPoint = 0;
    public GameObject LeftScene;
    public bool BossSpawned = false;

    public float BossRoomX;
    public float BossRoomZ;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(RoomCount);
        if (RoomCountTemp == RoomCount)
        {
            FramesNoRooms += 1;
            if (FramesNoRooms >= MaxFramesNoRooms) {
                GenerationComplete = true;
                if (RoomCount < MinimumRooms || RoomCount > MaximumRooms)
            {

                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        RoomCountTemp = RoomCount;
        
        
    }
}
