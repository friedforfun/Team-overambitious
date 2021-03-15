using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerStatus playerStats;


    private Vector2 orientDirection;
    private Vector2 moveInput;
    private bool AttackButtonDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        orientCharacter();
        applyMove();
        if (AttackButtonDown)
        {
            // Call Attack here
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnOrientPlayer(InputAction.CallbackContext context)
    {
        orientDirection = context.ReadValue<Vector2>();
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackButtonDown = true;
        }
        if (context.canceled)
        {
            AttackButtonDown = false;
        }
    }

    /// <summary>
    /// Called every frame to apply the move supplied by move input
    /// </summary>
    private void applyMove()
    {
        controller.SimpleMove((new Vector3(moveInput.x, 0, moveInput.y) /* * Time.deltaTime */ * baseMoveSpeed) * Mathf.Abs(playerStats.MoveSpeedModifier()));
    }

    /// <summary>
    /// Orient player in the direction of analog stick/mouse movement
    /// </summary>
    private void orientCharacter()
    {
        // Look towards direction of vector
        Vector3 desiredDirection = new Vector3(orientDirection.x, 0, orientDirection.y);

        // Only look in the direction of movement
        if (desiredDirection.x != 0f && desiredDirection.z != 0f)
        {
            Quaternion rotationToDirection = Quaternion.LookRotation(desiredDirection, Vector3.up);

            float rate = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(rotationToDirection, transform.rotation, rate);
        }
    }

    /// <summary>
    /// Draw a gizmo to show the direction the player is facing
    /// </summary>
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.4f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.4f);
    }

}
