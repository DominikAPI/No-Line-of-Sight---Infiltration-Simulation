using System;

public class GuardEntity
{
    public float DetectionRange { get; private set; }
    public float DetectionTime { get; private set; }

    /// <summary>
    /// Half of the vision cone's apex angle
    /// </summary>
    public float HalfAngle { get; private set; }

    /// <summary>
    /// Event fired when a detectable object gets detected
    /// </summary>
    public Action<IDetectable, string> OnTargetDetected;

    /// <summary>
    /// Consturcts a guard entity
    /// </summary>
    /// <param name="detectionRange">Range of the guard's vision</param>
    /// <param name="detectionTime">Time it takes in seconds to detect an object</param>
    /// <param name="halfAngle">Half of the vision cone's apex angle</param>
    public GuardEntity(float detectionRange, float detectionTime, float halfAngle = 45f)
    {
        DetectionRange = detectionRange;
        DetectionTime = detectionTime;
        HalfAngle = halfAngle;
    }


    /// <summary>
    /// Invokes the <see cref="OnTargetDetected"/> event with the given parameters
    /// </summary>
    /// <param name="detected">The detected object</param>
    /// <param name="message">Message to pass</param>
    public void EnterAlertState(IDetectable detected, string message) => OnTargetDetected?.Invoke(detected, message);
}
