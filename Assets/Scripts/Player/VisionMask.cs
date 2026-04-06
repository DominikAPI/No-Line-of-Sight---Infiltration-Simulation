using UnityEngine;

[RequireComponent(typeof(VisionMesh))]
public class VisionMask : MonoBehaviour
{
    [SerializeField] private Transform followObject;

    private readonly float hitOffset = -0.15f;
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
        ConstructMask(followObject.position);
    }

    public void ConstructMask(Vector3 position)
    {
        transform.position = position;
        visionMesh.ConstructVisionMesh(range, halfAngle, raysPerHalfAnlge, hitOffset);
    }
}
