using UnityEngine;
using UnityEngine.InputSystem;

public class DoorUnlockWithKeys : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public string requiredKeyName;
    public GameObject doorObject;
    public bool playerInRange;

    public InputSystem_Actions inputSystemActions;
    public InputAction inputAction;
    public GameObject player;

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

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {

        TryUnlockDoor();
    }

    private void TryUnlockDoor()
    {
        if (playerInRange && player != null)
        {
            string itemHoldingName = inventorySystem.itemHoldingName; 

            if (itemHoldingName == requiredKeyName)
            {
                DeleteDoor(); // Delete the door if the player holds the correct key
            }
        }
    }

    private void DeleteDoor()
    {
        
        if (doorObject != null)
        {
            Destroy(doorObject); // Delete the door object
        }
    }
}
