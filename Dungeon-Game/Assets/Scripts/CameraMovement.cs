using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Player;
    public float distance;

    void Start()
    {
        if(distance == 9.87f) GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("Minimap")); // Shows everything but items on the Minimap layer
        else GetComponent<Camera>().cullingMask = 1792; // Shows only the Minimap, Wall and Ground layers
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + distance, Player.transform.position.z); // Causes the camera to follow the player at a fixed distance above
    }
}
