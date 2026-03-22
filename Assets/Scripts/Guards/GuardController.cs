using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VisionSensor))]
public class GuardController : MonoBehaviour, IKillable, IResetable, IPatrol
{
    private static readonly WaitForSeconds _waitForSeconds1_0 = new(1f);
    [SerializeField] private GameObject corpse;
    
    private GuardEntity entity;
    private VisionSensor visionSensor;
    private VisionMesh visionMesh;

    private readonly float checkFrequency = 0.1f;
    private float visibleTime = 0f;
    private readonly int raysPerHalfAnlge = 18;

    private Vector3 moveDir;
    private bool isTurning = false;

    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }

    /// <summary>
    /// Initialize the guard with a <see cref="GuardEntity"/>
    /// </summary>
    /// <param name="entity">The entity</param>
    public void Initialize(GuardEntity entity)
    {
        this.entity = entity;
    }

    private void Awake()
    {
        visionSensor = GetComponent<VisionSensor>();
        visionMesh = GetComponentInChildren<VisionMesh>();
        corpse.SetActive(false);
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
        moveDir = transform.up;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGuardFunctions();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        corpse.transform.position = transform.position;
    }

    public void Die()
    {   
        Debug.Log("Guard died");
        CancelInvoke();
        visionMesh.DestroyVisionMesh();
        corpse.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Check for detectable objects
    /// </summary>
    private void DetectionCheck()
    {
        if (entity == null) return;

        List<IDetectable> detectedObjects = visionSensor.Scan(entity.DetectionRange, entity.HalfAngle, transform.up);

        if (detectedObjects.Count == 0)
        {
            visibleTime = 0f;
            return;
        }

        visibleTime += checkFrequency;

        foreach (var detectable in detectedObjects)
        {
            DetectionResponse response = detectable.GetDetectionResponse();
            Debug.Log(visibleTime);
            if (response.ShouldTriggerAlert(visibleTime, entity))
                entity.EnterAlertState(detectable, "Detected!");
        }
    }

    public void Turn(float angle) => StartCoroutine(TurnRoutine(angle));
    private IEnumerator TurnRoutine(float angle)
    {
        isTurning = true;
        yield return _waitForSeconds1_0;

        transform.Rotate(Vector3.forward, angle);
        isTurning = false;
    }

    public void Patrol()
    {
        if (entity.Speed == 0f || isTurning) return;
        transform.Translate(entity.Speed * Time.deltaTime * moveDir);
    }

    /// <summary>
    /// Starts the detection check and the vision mesh construction
    /// </summary>
    private void StartGuardFunctions()
    {
        InvokeRepeating(nameof(DetectionCheck), 0f, checkFrequency);
        InvokeRepeating(nameof(UpdateVisionMesh), 0f, checkFrequency);
    }

    private void UpdateVisionMesh() => visionMesh.ConstructVisionMesh(entity.DetectionRange, entity.HalfAngle, raysPerHalfAnlge);

    public void ResetObject()
    {
        visibleTime = 0;
        Debug.Log(visibleTime);
        transform.SetPositionAndRotation(OriginalPosition, OriginalRotation);
        corpse.SetActive(false);
        corpse.transform.SetPositionAndRotation(OriginalPosition, OriginalRotation);
        gameObject.SetActive(true);
        StartGuardFunctions();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
