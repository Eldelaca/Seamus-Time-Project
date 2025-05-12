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
    private InputAction skipTextAction;
    [SerializeField] private GroundCheck groundCheck;

    public bool canDrag;
    public bool skipLine;
    public bool wasSkippingLastFrame = false;

    
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
    
    [Header("Inventory System")]
    public InventorySystem inventorySystem;

    private bool isInteracting;
    private bool wasInteracting;
    private bool jumpInputReleased;
    
    
    [Header("Time Travel Input")] 
    // Will change based on what TP Point the player enters, assignment handled by TP Point script
    [HideInInspector] public TP_Connector current_TP_Connector;
    private bool tp_In_Progress;
    
    
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
        skipTextAction = inputSystemActions.Player.SkipLine;
    }
    
    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        crouchAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
        shootAction.Enable();
        skipTextAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        crouchAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
        shootAction.Disable();
        skipTextAction.Disable();
    }

    private void Start()
    {
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        StateController();
        SpeedControl();
        
        if(tp_In_Progress == false)
            MovePlayer();
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void StateController()
    {
        var jumpInput = jumpAction.ReadValue<float>();

        if (jumpInput == 0)
            jumpInputReleased = true;

        if (groundCheck.isGrounded && jumpInput > 0 && readyToJump && jumpInputReleased)
        {
            readyToJump = false;
            jumpInputReleased = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        var crouchInput = crouchAction.ReadValue<float>();
        if (crouchInput > 0)
        {
            moveSpeed = crouchSpeed;
            
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

        bool isInteracting = interactInput > 0;
        if (isInteracting && !wasInteracting) //detects one press
        {
            if (!inventorySystem.holdingItem) inventorySystem.PickUpItem();
            else if (inventorySystem.holdingItem) inventorySystem.DropItem();
        }
        
        wasInteracting = isInteracting;
        
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
            if (current_TP_Connector == null)
                return;
            else
            {
                tp_In_Progress = true;
                rb.linearVelocity = Vector3.zero;
                rb.linearDamping = 0;
                current_TP_Connector.TP_Sorter();
                Invoke(nameof(Reset_TP_Progress), .25f);
            }
            
        }
        
        var skipInput = skipTextAction.ReadValue<float>();
        skipLine = skipInput > 0;
        if (skipLine && !wasSkippingLastFrame)
        {
            DialogueController.instance.SkipLine();
        } 
        wasSkippingLastFrame = skipLine;
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

        if (!WallInFront())
        {
            if (groundCheck.isGrounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
            else
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }

        if (!groundCheck.isGrounded && WallInFront())
        {
            rb.AddForce(Vector3.down * 10f, ForceMode.Force);
        }
        // turn gravity off while on slope
        rb.useGravity = !(OnSlope() && slopeHit.normal.y > 0.1f);
    }
    
    bool WallInFront()
    {
        return Physics.Raycast(transform.position, transform.forward, 1.2f);
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

    private void Reset_TP_Progress()
    {
        tp_In_Progress = false;
    }
    
    
}