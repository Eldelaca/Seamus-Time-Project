using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    public float groundCheckDistance;
    private float groundCheckBuffer = 0.2f;

    private void Update()
    {
        groundCheckDistance = (this.GetComponent<CapsuleCollider>().height / 2) + groundCheckBuffer;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
            isGrounded = true;
        else
            isGrounded = false;
    }
}
