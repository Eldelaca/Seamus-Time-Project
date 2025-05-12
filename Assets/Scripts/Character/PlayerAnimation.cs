using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    private void Update()
    {
        float speed = playerMovement.moveDirection.magnitude;

        animator.SetFloat("speed", speed);
        animator.SetBool("isJumping", !playerMovement.groundCheck.isGrounded);
        animator.SetBool("isSprinting", Mathf.Approximately(playerMovement.moveSpeed, playerMovement.sprintSpeed));
    }
}
