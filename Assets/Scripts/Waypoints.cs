using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTransition : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject targetLocation; 
    private bool playerInRange;

    private InputSystem_Actions inputSystemActions;
    private InputAction inputAction; 
    private GameObject player;

    private PlayerMovement playerMovement; 

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        inputAction = inputSystemActions.Player.Input; 
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.performed += OnInputPerformed; 
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.performed -= OnInputPerformed; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = true;
            player = other.gameObject;
            playerMovement = player.GetComponent<PlayerMovement>(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerMovement = null; // Reset the reference
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        TryTeleport();
    }

    private void TryTeleport()
    {
        if (playerInRange && targetLocation != null && player != null)
        {
            player.transform.position = targetLocation.transform.position;
        }
    }
}
