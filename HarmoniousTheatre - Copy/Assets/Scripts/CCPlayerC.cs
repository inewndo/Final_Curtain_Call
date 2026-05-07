using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CCPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float WalkSpeed = 5f;
    public float RunSpeed = 9f;
    public float JumpForce = 5f;

    private bool _JumpReady;
    private bool _IsRunning;
    private float _yaw;
    private float _pitch;
    private Rigidbody _rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    [Header("Ground check")]
    public LayerMask groundLayer;
    public float groundCheckRadius = .5f;
    public float groundCheckDistance = .5f;
    public bool isGrounded;
    public Transform groundCheck;

    [Header("Camera")]
    public Transform camTransform;
    public float LookSens;

    public bool interactPressed;
    public Interactable currentInteractable;
    public static event Action<ObjectData> OnDescriptionRequested;
    public int startHealth = 10;
    public int currentHealth;
    [SerializeField] private PlayerHpBar healthbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        currentHealth = startHealth;
        healthbar.UpdateHpBar(startHealth, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CameraLook();
        HandleInteract();
        //camTransform.position = transform.position;
    }

    void FixedUpdate()
    {
        //movement switching from run to walk
        float currentSpeed;
        if (_IsRunning)
        {
            currentSpeed = RunSpeed;
        }
        else
        {
            currentSpeed = WalkSpeed;
        }

        Vector3 move = transform.forward * moveInput.y * currentSpeed +
            transform.right * moveInput.x * currentSpeed;

        _rb.linearVelocity = new Vector3(move.x, _rb.linearVelocity.y, move.z);

        if (_JumpReady && isGrounded)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            _JumpReady = false;
        }

        if (!isGrounded && _JumpReady)
        {
            _JumpReady = false;
        }
    }
    public void CameraLook()
    {
        if (camTransform == null) return;

        float mouseX = lookInput.x * LookSens * Time.deltaTime;
        float mouseY = lookInput.y * LookSens * Time.deltaTime;

        //left and right
        _yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);

        //vertical rotation (cam only) 
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        camTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
    void HandleInteract()
    {
        //if the player did not press interact this frame do nothing
        if (!interactPressed) return;
        //consume the input so one click only triggers one interactions
        //this changes next frame
        interactPressed = false;
        if (currentInteractable == null) return;
        currentInteractable.Interact(this);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _IsRunning = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _JumpReady = true;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) interactPressed = true;
    }

    public void RequestDescription(ObjectData objectData)
    {
        OnDescriptionRequested?.Invoke(objectData);
    }
    public void TakeDamage(int attackPower)
    {
        currentHealth -= attackPower;
        healthbar.UpdateHpBar(startHealth, currentHealth);
    }
    private void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        //inside of this sphere is checking groundcheckpos & radius, distance, and layermask, 
        // then will be either true or false 
        isGrounded = Physics.SphereCast(groundCheck.position, groundCheckRadius, Vector3.down,
            out RaycastHit hit, groundCheckDistance, groundLayer);
    }
}
