using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Fields")]
    private InputSystem_Actions inputSystemActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction shootAction;
    [SerializeField] private GroundCheck groundCheck;

    public bool canDrag;
    
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float rotateSpeed;

    public float groundDrag;
    [HideInInspector] public Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    [SerializeField] private float movementForce = 1f;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputSystemActions = new InputSystem_Actions();
        
        canDrag = false;
        
        moveAction = inputSystemActions.Player.Move;
        jumpAction = inputSystemActions.Player.Jump;
        crouchAction = inputSystemActions.Player.Crouch;
        sprintAction = inputSystemActions.Player.Sprint;
        interactAction = inputSystemActions.Player.Interact;
        shootAction = inputSystemActions.Player.Shoot;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        crouchAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        crouchAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
        shootAction.Disable();
    }

    private void Start()
    {
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        StateController();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void StateController()
    {
        // when to jump
        var jumpInput = jumpAction.ReadValue<float>();
        
        if (groundCheck.isGrounded && jumpInput > 0 && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        var crouchInput = crouchAction.ReadValue<float>();
        if (crouchInput > 0)
        {
            moveSpeed = crouchSpeed;
            print("crouching");
            
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if (crouchInput <= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        var sprintInput = sprintAction.ReadValue<float>();
        if (sprintInput > 0)
        {
            print("sprinting");
            moveSpeed = sprintSpeed;
        }
        
        var interactInput = interactAction.ReadValue<float>(); //right click
        canDrag = interactInput > 0;

        if (sprintInput <= 0 && crouchInput <= 0 && groundCheck.isGrounded)
        {
            moveSpeed = walkSpeed;
            rb.linearDamping = groundDrag;
        }
        else
        {
            // in the air
            rb.linearDamping = 0;
        }
        
        var shootInput = shootAction.ReadValue<float>();
        if (shootInput > 0)
        {
            print("SHOOTING");
        }
    }

    private void MovePlayer()
    {
        var moveInput = moveAction.ReadValue<Vector2>().x; //reading which way to move on horizontal
        moveDirection = new Vector3(moveInput * movementForce, 0f, 0f); 
        
        if (moveDirection.magnitude > 0.1f) // Only update when moving
        {
            lastMoveDirection = moveDirection.normalized;
            Quaternion rotation = Quaternion.LookRotation(lastMoveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        else if(groundCheck.isGrounded)
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force); //walking

        else if(!groundCheck.isGrounded)
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force); //in air

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        float playerHeight = 2f;
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}