using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDragging : MonoBehaviour
{
    private PlayerMovement playerMovement;
    
    private bool holding;
    private GameObject objectToDrag;
    public float momentumMultiplier = 5f;
    
    private Vector3 lastPlayerPosition;
    private Rigidbody rb;
    
    private Rigidbody objectRb;

    private bool exitedTrigger;
    
    private Vector3 initialOffset;

    

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        holding = false;
    }

    private void Update()
    {
        if (playerMovement.canDrag && objectToDrag != null) // if right click is being pressed and there is something to hold onto
        {
            if (!holding) // if not already holding something
            {
                holding = true;
                objectRb = objectToDrag.GetComponent<Rigidbody>();
                lastPlayerPosition = transform.position;
            }
            
            if (exitedTrigger)
            {
                //Vector3 movementDelta = transform.position - lastPlayerPosition;
                objectToDrag.transform.position = transform.position + initialOffset; // move the object by the same amount the player moves

                lastPlayerPosition = transform.position;
            }
            print(exitedTrigger);
        }
        else if (objectToDrag != null && holding)// if interact is released while holding something
        {
            if (objectRb != null)
            {
                Vector3 momentum = (transform.position - lastPlayerPosition) * momentumMultiplier;
                //objectRb.linearVelocity = momentum;
            }

            holding = false;
            objectToDrag = null;
            objectRb = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Draggable") && objectToDrag == null) // if its draggable, store its object (if theres nothing being stored rn)
        {
            objectToDrag = other.transform.root.gameObject; 
            exitedTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!holding && objectToDrag == other.transform.root.gameObject) // if were not holding this object, get rid of whats being stored
        {
            objectToDrag = null;
        }
        
        if (other.transform.root.gameObject == objectToDrag) // if holding the right object, allow it to be moved as we exit the trigger area
        {
            exitedTrigger = true;
            initialOffset = objectToDrag.transform.position - transform.position;
        }
    }
}
