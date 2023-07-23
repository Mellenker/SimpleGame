using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    private float sprintSpeed;
    private float defaultSpeed;

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    bool readytoJump;

    [SerializeField] private PlayerControls controls;
    private KeyCode jumpKey;
    private KeyCode sprintKey;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform groundCheck;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation= true;
        readytoJump = true;

        defaultSpeed = moveSpeed;
        sprintSpeed = moveSpeed * sprintMultiplier;

        jumpKey = controls.getJumpKey();
        sprintKey = controls.getSprintKey();

    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        // Handle drag
        if (IsGrounded())
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readytoJump && IsGrounded())
        {
            Debug.Log("jump");
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        
        if (Input.GetKey(sprintKey))
        {
            Debug.Log("sprint");
            moveSpeed = sprintSpeed;
        }

        else
        {
            moveSpeed = defaultSpeed;
        }

    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10);

        // On ground
        if (IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
            

        // In air
        else if(!IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
            
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocitty if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    } 

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readytoJump = true;
    }

    
    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, groundLayer);
    }

}

