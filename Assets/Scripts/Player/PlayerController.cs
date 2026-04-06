using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class PlayerController : MonoBehaviour, IDetectable, IResetable
{
    [SerializeField] private float speed;
    [SerializeField] private float slideForce;
    [SerializeField] private AudioClip footsteps;
    private readonly float slideCooldown = 2.0f;
    private float timeSinceSlide = 2.0f;

    private PlayerInputActions playerInputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector2 previousLookInput;
    private AudioSource audioSource;
    private Rigidbody2D rigidBody;
    private Camera mainCamera;
    private PistolController gun;
    private DetectionResponse detectionResponse;
    private IInteractable interactable;
    private bool isMoving = false;

    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rigidBody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        gun = GetComponentInChildren<PistolController>();
        detectionResponse = new PlayerDetectionResponse();
        audioSource = GetComponent<AudioSource>();

        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
        audioSource.clip = footsteps;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.performed += GetMoveInput;
        playerInputActions.Player.Move.canceled += GetMoveInput;
        playerInputActions.Player.Slide.performed += PerformSlide;
        playerInputActions.Player.Look.performed += GetLookInput;
        playerInputActions.Player.Look.canceled += GetLookInput;
        playerInputActions.Player.Attack.performed += PerformShoot;
        playerInputActions.Player.Interact.performed += PerformInteract;

        EnablePlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSlide += Time.deltaTime;
        HandleMovement();  
        HandleLook();
    }

    /// <summary>
    /// Moves the player based on the move input
    /// </summary>
    private void HandleMovement()
    {
        Vector3 movement = (Vector3)moveInput;

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        bool wasMoving = isMoving;
        isMoving = movement != Vector3.zero;
        HandleFootstepSound(isMoving, wasMoving);

        transform.position += speed * Time.deltaTime * movement;
    }

    private void HandleFootstepSound(bool moving, bool wasMoving)
    {
        if (moving)
        {
            if (!wasMoving)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (wasMoving)
            {
                audioSource.loop = false;
                audioSource.Stop();
            }
        }
    }

    /// <summary>
    /// Rotates the player based on the mouse pointer's position on the screen
    /// </summary>
    private void HandleLook()
    {
        if (previousLookInput == lookInput) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(lookInput);
        mouseWorldPos.z = 0f;           //just to be sure

        Vector2 direction = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void GetMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Slides the player
    /// </summary>
    /// <param name="context">Input action callback context</param>
    private void PerformSlide(InputAction.CallbackContext context)
    {
        if (timeSinceSlide < slideCooldown) return;
        Vector2 forceInput = moveInput == Vector2.zero ? Vector2.right : moveInput;

        rigidBody.AddForce(slideForce * forceInput, ForceMode2D.Impulse);
        timeSinceSlide = 0f;
    }

    public void SetCheckpoint()
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
    }

    private void GetLookInput(InputAction.CallbackContext context)
    {
        previousLookInput = lookInput;
        lookInput = context.ReadValue<Vector2>();
    }

    private void PerformShoot(InputAction.CallbackContext context)
    {
        gun.Fire();
    }

    public DetectionResponse GetDetectionResponse() => detectionResponse;

    public void FocusOn() { }

    public Vector3 GetPosition() => transform.position;

    private void PerformInteract(InputAction.CallbackContext context) => interactable?.Interact(this);

    public void SetInteractable(IInteractable interactable) => this.interactable = interactable;

    public void RemoveInteractable(IInteractable interactable)
    {
        if (this.interactable != interactable) return;

        this.interactable = null;
    }

    private void OnDisable()
    {
        DisablePlayerControls();
    }

    public void ResetObject()
    {
        timeSinceSlide = 2f;
        rigidBody.linearVelocity = Vector3.zero;
        transform.SetPositionAndRotation(OriginalPosition, OriginalRotation);
    }

    public void EnablePlayerControls()
    {
        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Slide.Enable();
        playerInputActions.Player.Look.Enable();
        playerInputActions.Player.Attack.Enable();
        playerInputActions.Player.Interact.Enable();
    }

    public void DisablePlayerControls()
    {
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Slide.Disable();
        playerInputActions.Player.Look.Disable();
        playerInputActions.Player.Attack.Disable();
        playerInputActions.Player.Interact.Disable();
    }
}
