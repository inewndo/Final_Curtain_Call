using System;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class CCPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float WalkSpeed = 5f;
    public float RunSpeed = 9f;
    public float JumpForce = 5f;
    private bool inputDisabled = false;

    private bool _JumpReady;
    private bool _IsRunning;
    private Rigidbody _rb;

    private Vector2 moveInput;

    [Header("Ground check")]
    public LayerMask groundLayer;
    public float groundCheckRadius = .5f;
    public float groundCheckDistance = .5f;
    public bool isGrounded;
    public Transform groundCheck;

    [Header("Camera")]
    public Transform camTransform;
    public float LookSens;
    private Vector2 lookInput;
    private float _yaw;
    private float _pitch;

    [Header("Interactable")]
    public Image reticleImage;
    public bool interactPressed;
    public Interactable currentInteractable;
    public static event Action<ObjectData> OnDescriptionRequested;

    [Header("Health")]
    public int startHealth = 40;
    public int currentHealth;
    [SerializeField] private PlayerHpBar healthbar;
   

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = startHealth;
        healthbar.UpdateHpBar(startHealth, currentHealth);
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(r: 0, g: 0, b: 0, a: 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            CameraLook();
            CheckInteract();
            HandleInteract();
        }
        CheckGround();
        

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("Lose");
            Destroy(gameObject);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
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

    void CheckInteract()
    {
        //reset reticle image to normal color first
        if (reticleImage != null) reticleImage.color = new Color(0, 0, 0, .7f);
        //make a ray that goes straight out of the camera(center of screen)
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        //RaycastHit hit;
        //asking unity if it hit something within 3 units
        //hit stores what we hit like the collider
        //bool didHit = Physics.Raycast(ray, out hit, 3);
        //if (!didHit) return;//if we didn't hit anything start here
        //if we hit something tagged interactable
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
            if (currentInteractable != null && reticleImage != null)
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(camTransform.position, camTransform.forward * 3, Color.blue);
            }
            else
            {
                Debug.DrawRay(camTransform.position, camTransform.forward * 3, Color.blue);
            }
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

    #region PLAYERINPUT
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

    #endregion

    public void RequestDescription(ObjectData objectData)
    {
        OnDescriptionRequested?.Invoke(objectData);
    }
    public void TakeDamage(int attackPower)
    {
        currentHealth -= attackPower;
        healthbar.UpdateHpBar(startHealth, currentHealth);
    }

    public void DisableInput()
    {
        inputDisabled = true;

        moveInput = Vector2.zero;
        lookInput = Vector2.zero;

        _rb.linearVelocity = Vector2.zero;

        _rb.isKinematic = true;

        if (reticleImage != null)
            reticleImage.color = new Color(0, 0, 0, 0f);

    }
    public void EnableInput()
    {
        inputDisabled = false;
        _rb.isKinematic = false;

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

