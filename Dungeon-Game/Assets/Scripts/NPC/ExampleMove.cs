using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField] private ContextSteering steer;
    [SerializeField] private CharacterController controller;
    [SerializeField] private NPCStatus stats;

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = steer.GetMoveDirection().normalized;
        moveDir = new Vector3(moveDir.x, 0, moveDir.z); // Remove any movement in y axis
        controller.Move(moveDir * Time.deltaTime * stats.MoveSpeedModifier());
    }
}
