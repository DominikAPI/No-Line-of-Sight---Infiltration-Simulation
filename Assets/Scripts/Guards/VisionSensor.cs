using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] private Transform visionPoint;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask visionMask;
    private readonly int raysPerHalfAngle = 10;

    /// <summary>
    /// Scans for detectable objects within the cone defined by the parameters
    /// </summary>
    /// <param name="range">Range of the detection</param>
    /// <param name="halfAngle">Half of the vision cone's apex angle</param>
    /// <param name="visionDirection">Main(centre) direction of thevision <param>
    /// <returns></returns>
    public List<IDetectable> Scan(float range, float halfAngle, Vector2 visionDirection)
    {
        List<IDetectable> detectables = new();
        List<Collider2D> colliders = GetDetectablesInRange(range);
        colliders.RemoveAll(collider => !WithinAngle(collider, halfAngle, visionDirection));

        if (colliders.Count == 0) return detectables;

        return TryDetectObjects(range, halfAngle, visionDirection);
    }

    /// <summary>
    /// Get all of the detectable colliders with the given <see cref="detectionMask"/> in range
    /// </summary>
    /// <param name="range">Range of the cast</param>
    /// <returns>List of the cast results</returns>
    private List<Collider2D> GetDetectablesInRange(float range)
    {
        ContactFilter2D contactFilter = new()
        {
            layerMask = detectionMask,
            useLayerMask = true
        };
        List<Collider2D> results = new();

        Physics2D.OverlapCircle(visionPoint.position, range, contactFilter, results);

        return results;
    }

    /// <summary>
    /// Check if a Collider is within the right angle
    /// </summary>
    /// <param name="collider">The checked collider</param>
    /// <param name="halfAngle">Half of the vision cone's apex angle</param>
    /// <param name="visionDirection">Main(centre) direction of thevision</param>
    /// <returns></returns>
    private bool WithinAngle(Collider2D collider, float halfAngle, Vector2 visionDirection)
    {
        Vector2 toCollider = (collider.transform.position - visionPoint.position).normalized;

        float dot = Vector2.Dot(visionDirection.normalized, toCollider);
        float cosHalfFov = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

        return dot >= cosHalfFov;
    }

    /// <summary>
    /// Search for detectable objects withing a cone of vision
    /// </summary>
    /// <param name="range">Range of the rays</param>
    /// <param name="halfAngle">Half of the vision cone's apex angle</param>
    /// <param name="visionDirection">Main(centre) direction of thevision</param>
    /// <returns>The list of the detected objects</returns>
    private List<IDetectable> TryDetectObjects(float range, float halfAngle, Vector2 visionDirection)
    {
        HashSet<IDetectable> results = new();
        float step = (halfAngle * 2f) / raysPerHalfAngle;

        for (float angle = -halfAngle; angle <= halfAngle; angle += step)
        {
            Vector2 rayDirection = MathHelper.RotateVector(visionDirection, angle);

            RaycastHit2D hit = Physics2D.Raycast(visionPoint.position, rayDirection, range, visionMask);

            if (hit.collider != null && hit.collider.TryGetComponent<IDetectable>(out var detectable)) results.Add(detectable);
        }

        return results.ToList();
    }
}