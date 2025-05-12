using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerDragging playerDragging;
    [SerializeField] private Animator animator;

    private bool wasHoldingItemLastFrame;
    
    private void Update()
    {
        float speed = playerMovement.moveDirection.magnitude;

        animator.SetFloat("speed", speed);
        animator.SetBool("isJumping", !playerMovement.groundCheck.isGrounded);
        animator.SetBool("isSprinting", Mathf.Approximately(playerMovement.moveSpeed, playerMovement.sprintSpeed));
        
        animator.SetBool("isHolding", playerDragging.holding);
        
        bool isPickingUp = playerMovement.inventorySystem.holdingItem && !wasHoldingItemLastFrame;
        if (isPickingUp)
        {
            animator.SetTrigger("PickUp");
        }

        wasHoldingItemLastFrame = playerMovement.inventorySystem.holdingItem;
    }
}
