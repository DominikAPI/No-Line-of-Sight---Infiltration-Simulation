using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionMesh : MonoBehaviour
{
    [SerializeField] private LayerMask visionMask;
    private readonly int raysPerHalfAngle = 10;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void DestroyVisionMesh() => meshFilter.mesh = null;

    
    /// <summary>
    /// Constructs a vision mesh with the given range and angle
    /// </summary>
    /// <param name="range">Range of the vision</param>
    /// <param name="halfAngle">Half of the vision angle</param>
    public void ConstructVisionMesh(float range, float halfAngle)
    {
        Mesh mesh = new Mesh();
        List<Vector3> visionPoints = GetVisionPoints(range, halfAngle);
        List<int> triangleFan = GetTriangleFan(visionPoints);

        mesh.vertices = visionPoints.ToArray();
        mesh.triangles = triangleFan.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    /// <summary>
    /// Gets the vertices of the vision mesh
    /// </summary>
    /// <param name="range">Range of the vision</param>
    /// <param name="halfAngle">Half of the vision angle</param>
    /// <returns>List of the vertices in local space</returns>
    private List<Vector3> GetVisionPoints(float range, float halfAngle)
    {
        List<Vector3> visionPoints = new()
        {
            Vector3.zero            //Local space origin
        };
        float step = (halfAngle * 2f) / raysPerHalfAngle;

        for (float angle = -halfAngle; angle <= halfAngle; angle += step)
        {
            Vector2 rayDirection = MathHelper.RotateVector(transform.up, angle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, range, visionMask);

            Vector3 point = hit ? hit.point : transform.position + (Vector3)rayDirection * range;
            visionPoints.Add(transform.InverseTransformPoint(point));
        }

         return visionPoints;
    }

    /// <summary>
    /// Constructs the indexes of a triangle fan
    /// </summary>
    /// <param name="visionPoints">List of the mesh's vertices</param>
    /// <returns>List of the indexes</returns>
    private List<int> GetTriangleFan(List<Vector3> visionPoints)
    {
        List<int> triangleFan = new();

        for (int i = 1;i < visionPoints.Count - 1; i++)
        {
            triangleFan.Add(0);
            triangleFan.Add(i);
            triangleFan.Add(i + 1);
        }

        return triangleFan;
    }
}
