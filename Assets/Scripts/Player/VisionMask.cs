using UnityEngine;

[RequireComponent(typeof(VisionMesh))]
public class VisionMask : MonoBehaviour
{
    [SerializeField] private Transform followObject;

    private readonly float hitOffset = 0.1f;
    private readonly int raysPerHalfAnlge = 180;
    private readonly float range = 12.0f;
    private readonly float halfAngle = 180f;

    private VisionMesh visionMesh;

    private void Awake()
    {
        visionMesh = GetComponent<VisionMesh>();
    }

    private void Update()
    {
        transform.position = followObject.position;
        visionMesh.ConstructVisionMesh(range, halfAngle, raysPerHalfAnlge, hitOffset);
    }
}
