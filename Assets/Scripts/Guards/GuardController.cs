using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VisionSensor))]
public class GuardController : MonoBehaviour, IKillable
{
    private GuardEntity entity;
    private VisionSensor visionSensor;
    private readonly float checkFrequency = 0.1f;
    private float visibleTime = 0f;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(DetectionCheck), 1f, checkFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {   
        Debug.Log("Guard died");
        CancelInvoke();
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
}
