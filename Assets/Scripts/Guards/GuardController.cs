using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VisionSensor))]
public class GuardController : MonoBehaviour, IKillable, IResetable
{
    [SerializeField] private GameObject corpse;
    
    private GuardEntity entity;
    private VisionSensor visionSensor;
    private VisionMesh visionMesh;

    private readonly float checkFrequency = 0.1f;
    private float visibleTime = 0f;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGuardFunctions();
    }

    // Update is called once per frame
    void Update()
    {
        
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

            if (response.ShouldTriggerAlert(visibleTime, entity))
                entity.EnterAlertState(detectable, "Detected!");
        }
    }

    /// <summary>
    /// Starts the detection check and the vision mesh construction
    /// </summary>
    private void StartGuardFunctions()
    {
        InvokeRepeating(nameof(DetectionCheck), 0f, checkFrequency);
        InvokeRepeating(nameof(UpdateVisionMesh), 0f, checkFrequency);
    }

    private void UpdateVisionMesh() => visionMesh.ConstructVisionMesh(entity.DetectionRange, entity.HalfAngle);

    public void ResetObject()
    {
        transform.SetPositionAndRotation(OriginalPosition, OriginalRotation);
        corpse.SetActive(false);
        gameObject.SetActive(true);
        StartGuardFunctions();
    }
}
