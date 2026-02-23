using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionMesh : MonoBehaviour
{
    [SerializeField] private LayerMask visionMask;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    public Mesh Mesh { get => mesh; }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
    }

    public void DestroyVisionMesh()
    {
        meshRenderer.enabled = false;
    }

    /// <summary>
    /// Constructs a vision mesh with the given range and angle
    /// </summary>
    /// <param name="range">Range of the vision</param>
    /// <param name="halfAngle">Half of the vision angle</param>
    /// <param name="raysPerhalfAnlge">Number of ray casted in the half of the angle</param>
    /// <param name="offset">Optinal, adds this offset to the hit points, 0 by default</param>
    public void ConstructVisionMesh(float range, float halfAngle, int raysPerhalfAnlge, float offset = 0)
    {
        meshRenderer.enabled = true;
        List<Vector3> visionPoints = GetVisionPoints(range, halfAngle, raysPerhalfAnlge, offset);
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
    /// <param name="halfAngle">Half of the vision's angle</param>
    /// <param name="offset">Adds this offset to the hit points</param>
    /// <returns>List of the vertices in local space</returns>
    private List<Vector3> GetVisionPoints(float range, float halfAngle, int raysPerHalfAngle, float offset)
    {
        List<Vector3> visionPoints = new()
        {
            Vector3.zero            //Local space origin
        };
        float step = halfAngle / raysPerHalfAngle;

        RaycastHit2D oldHit = new();
        Vector2 oldRayDirection = new();

        for (float angle = -halfAngle; angle <= halfAngle + 0.00001f; angle += step)
        {
            Vector2 rayDirection = MathHelper.RotateVector(transform.up, angle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, range, visionMask);

            if (oldHit && !hit) visionPoints.Add(transform.InverseTransformPoint(FindEdge(oldHit, rayDirection, range)));
            if (!oldHit && hit) visionPoints.Add(transform.InverseTransformPoint(FindEdge(hit, oldRayDirection, range)));
            oldHit = hit;
            oldRayDirection = rayDirection;

            Vector3 point = hit ? hit.point + rayDirection * offset : transform.position + (Vector3)rayDirection * range;
            visionPoints.Add(transform.InverseTransformPoint(point));      
        }

         return visionPoints;
    }

    /// <summary>
    /// Finds the closest edge of a collider to the hit point
    /// </summary>
    /// <param name="hit">The last hit that hit the collider</param>
    /// <param name="missDirection">Direction of the first miss</param>
    /// <param name="range">Range of the vision</param>
    /// <returns>Close approximation of the coordinate of the edge</returns>
    private Vector3 FindEdge(RaycastHit2D hit, Vector2 missDirection, float range)
    {
        int maxIterations = 5;
        Vector2 hitDir = (hit.point - (Vector2)transform.position).normalized;
        Vector2 missDir = missDirection.normalized;

        for (int i = 0; i < maxIterations; i++)
        {
            Vector2 midDir = ((hitDir + missDir) * 0.5f).normalized;
            RaycastHit2D newHit = Physics2D.Raycast(transform.position, midDir, range, visionMask);

            if (newHit) hitDir = midDir;
            else missDir = midDir;
        }

        RaycastHit2D finalHit = Physics2D.Raycast(transform.position, hitDir, range, visionMask);
        return finalHit ? finalHit.point : (Vector3)((Vector2)transform.position + hitDir * range);
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