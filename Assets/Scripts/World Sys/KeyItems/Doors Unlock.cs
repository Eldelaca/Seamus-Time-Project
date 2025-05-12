using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorUnlockWithKeys : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public PlayerMovement playerMovement;
    public string requiredKeyName;
    public GameObject doorObject;
    public bool playerInRange;

    public GameObject player;
    
    [Header("Audio")]
    public AudioSource doorSound;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerMovement != null &&  playerMovement.canDrag) //detects a click from RMB
        {
            TryUnlockDoor();
        }
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
        doorSound.Play();
        
        if (doorObject != null)
        {
            Destroy(doorObject); // Delete the door object
        }
    }
}
