using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float slideForce;
    private readonly float slideCooldown = 2.0f;
    private float timeSinceSlide = 2.0f;

    private PlayerInputActions playerInputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private Rigidbody2D rigidBody;
    private Camera mainCamera;
    private PistolController gun;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rigidBody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        gun = GetComponentInChildren<PistolController>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.performed += MovePlayer;
        playerInputActions.Player.Move.canceled += MovePlayer;
        playerInputActions.Player.Slide.performed += PerformSlide;
        playerInputActions.Player.Look.performed += PerformLook;
        playerInputActions.Player.Look.canceled += PerformLook;
        playerInputActions.Player.Attack.performed += PerformShoot;

        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Slide.Enable();
        playerInputActions.Player.Look.Enable();
        playerInputActions.Player.Attack.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSlide += Time.deltaTime;
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
        if (timeSinceSlide < slideCooldown) return;
        Vector2 forceInput = moveInput == Vector2.zero ? Vector2.right : moveInput;

        rigidBody.AddForce(slideForce * forceInput, ForceMode2D.Impulse);
        timeSinceSlide = 0f;
    }

    private void PerformLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void PerformShoot(InputAction.CallbackContext context)
    {
        gun.Fire();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Slide.Disable();
        playerInputActions.Player.Look.Disable();
        playerInputActions.Player.Attack.Disable();
    }
}
