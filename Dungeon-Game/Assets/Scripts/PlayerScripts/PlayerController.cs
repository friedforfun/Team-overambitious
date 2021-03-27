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
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Animator animator;

    private Vector2 orientDirection;
    private Vector2 moveInput;
    private bool AttackButtonDown = false;
    private bool AbilityButtonDown = false;


    // Update is called once per frame
    void Update()
    {
        orientCharacter();
        applyMove();
        if (AttackButtonDown)
        {
            playerAttack.Attack(new Vector3(orientDirection.x, 0, orientDirection.y));
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnOrientPlayer(InputAction.CallbackContext context)
    {
        Vector2 tempDir = context.ReadValue<Vector2>();
        if (tempDir.x != 0 || tempDir.y != 0)
            orientDirection = tempDir.normalized;
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

    public void OnCastAbility(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AbilityButtonDown = true;
        }
        if (context.canceled)
        {
            AbilityButtonDown = false;
        }
    }

    /// <summary>
    /// Called every frame to apply the move supplied by move input
    /// </summary>
    private void applyMove()
    {
        controller.SimpleMove((new Vector3(moveInput.x, 0, moveInput.y) /* * Time.deltaTime */ * baseMoveSpeed) * Mathf.Abs(playerStats.MoveSpeedModifier()));
        Vector3 relativeVelocity = transform.InverseTransformVector(controller.velocity);
        animator.SetFloat("ForwardSpeed", relativeVelocity.z);
        animator.SetFloat("StrafeSpeed", relativeVelocity.x);

        //Debug.Log($"Velocity: {transform.InverseTransformVector(controller.velocity)}");
    }

    /// <summary>
    /// Orient player in the direction of analog stick/mouse movement
    /// </summary>
    private void orientCharacter()
    {
        // Look towards direction of vector
        Vector3 desiredDirection = new Vector3(orientDirection.x, 0, orientDirection.y);

        // Only look in the direction of movement
        if (desiredDirection.x != 0f || desiredDirection.z != 0f)
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
