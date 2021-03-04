using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;


    private Vector2 mousePosition;
    private bool mouseDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseDown)
        {
            moveToMouse();
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnClickToMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            mouseDown = true;
        }
        if (context.canceled)
        {
            mouseDown = false;
        }
    }
    
    private void moveToMouse()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawRay(cam.transform.position, cam.transform.position - hit.point, Color.red);
            transform.LookAt(hit.point);
            agent.destination = hit.point;
        }
        
    }
}
