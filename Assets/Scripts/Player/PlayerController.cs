using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float slideForce = 2.0f;
    private PlayerInputActions playerInputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody2D rigidBody;
    private Camera mainCamera;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rigidBody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.performed += MovePlayer;
        playerInputActions.Player.Move.canceled += MovePlayer;
        playerInputActions.Player.Slide.performed += PerformSlide;
        playerInputActions.Player.Look.performed += PerformLook;
        playerInputActions.Player.Look.canceled += PerformLook;

        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Slide.Enable();
        playerInputActions.Player.Look.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();  
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector3 movement = (Vector3)moveInput;

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        transform.position += speed * Time.deltaTime * movement;
    }

    private void HandleLook()
    {
        if (lookInput == Vector2.zero) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(lookInput);
        mouseWorldPos.z = 0f;

        Vector2 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void PerformSlide(InputAction.CallbackContext context)
    {
        Vector2 forceInput = moveInput == Vector2.zero ? Vector2.right : moveInput;

        rigidBody.AddForce(slideForce * forceInput, ForceMode2D.Impulse);
    }

    private void PerformLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Slide.Disable();
        playerInputActions.Player.Look.Disable();
    }
}
