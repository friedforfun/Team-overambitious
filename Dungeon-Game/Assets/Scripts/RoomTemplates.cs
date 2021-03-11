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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(RoomCount);
       if(RoomCountTemp == RoomCount)
        {
            FramesNoRooms += 1;
            if(FramesNoRooms >= MaxFramesNoRooms && (RoomCount < MinimumRooms || RoomCount > MaximumRooms))
            {
           
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        RoomCountTemp = RoomCount;
        
        
    }
}
